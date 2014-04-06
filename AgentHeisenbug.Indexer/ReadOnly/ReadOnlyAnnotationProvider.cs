using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Utils;
using JetBrains.Util;

namespace AgentHeisenbug.Indexer.ReadOnly {
    public class ReadOnlyAnnotationProvider : IAnnotationProvider {
        private static readonly object CachedTrue = new object();
        private static readonly object CachedFalse = new object();
        private static readonly Annotation ReadOnlyAnnotation = new Annotation("M:JetBrains.Annotations.GeneratedReadOnlyAttribute.#ctor");

        private readonly ConditionalWeakTable<IMetadataTypeInfo, object> typeCache = new ConditionalWeakTable<IMetadataTypeInfo, object>();
        private readonly DirectoryInfo frameworkDirectory;

        public ReadOnlyAnnotationProvider(DirectoryInfo frameworkDirectory) {
            this.frameworkDirectory = frameworkDirectory;
        }

        public IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(Func<string, bool> assemblyNameFilter) {
            var frameworkPath = FileSystemPath.Parse(frameworkDirectory.FullName);
            using (var loader = new MetadataLoader(frameworkPath)) {
                var assemblyPaths = frameworkPath.GetChildFiles("*.dll");
                foreach (var assemblyPath in assemblyPaths) {
                    var assembly = loader.TryLoadFrom(assemblyPath, JetFunc<AssemblyNameInfo>.True);
                    if (assembly == null || assembly.AssemblyName == null || !assemblyNameFilter(assembly.AssemblyName.Name))
                        continue;

                    yield return new AnnotationsByAssembly(assembly.AssemblyName.Name, InferReadOnlyForAssembly(assembly));
                }
            }
        }

        private ICollection<AnnotationsByMember> InferReadOnlyForAssembly(IMetadataAssembly assembly) {
            return assembly.GetTypes()
                           .Where(t => t.IsPublic && !t.IsInterface && !IsObviouslyReadOnly(t))
                           .Where(IsReadOnly)
                           .Select(t => new AnnotationsByMember(GenerateXmlId(t), ReadOnlyAnnotation))
                           .ToList();

        }

        private string GenerateXmlId(IMetadataTypeInfo type) {
            return "T:" + type.FullyQualifiedName;
        }

        private bool IsObviouslyReadOnly(IMetadataTypeInfo type) {
            return type.IsDelegate() || type.IsValueType();
        }

        private bool IsReadOnly(IMetadataTypeInfo type) {
            return typeCache.GetValue(type, t => IsReadOnlyUncached(t) ? CachedTrue : CachedFalse) == CachedTrue;
        }

        private bool IsReadOnlyUncached(IMetadataTypeInfo type) {
            return type.GetFields().All(IsReadOnlyField)
                && (type.Base == null || IsReadOnly(type.Base.Type));
        }

        private bool IsReadOnlyField(IMetadataField field) {
            if (!field.IsInitOnly)
                return false;

            var classType = field.Type as IMetadataClassType;
            if (classType == null || classType.Type == field.DeclaringType)
                return true;

            return IsObviouslyReadOnly(classType.Type)
                || IsReadOnly(classType.Type);
        }
    }
}
