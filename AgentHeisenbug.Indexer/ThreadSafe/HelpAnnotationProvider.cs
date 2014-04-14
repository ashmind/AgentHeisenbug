using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Annotations.Generated;
using AshMind.Extensions;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class HelpAnnotationProvider : IAnnotationProvider {
        private static readonly string ThreadSafeConstructorXmlId = "M:" + typeof(GeneratedThreadSafeAttribute).FullName + ".#ctor";
        private const string NonParsedFileName = "NotParsed.Generated.txt";

        private readonly HelpRawReader reader;

        public HelpAnnotationProvider(HelpRawReader reader) {
            this.reader = reader;
        }

        public IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(Func<string, bool> assemblyNameFilter, Action<double> reportProgress) {
            var typesByAssembly = new Dictionary<string, ISet<TypeDescription>>();
            foreach (var type in this.reader.ReadFiles(reportProgress)) {
                foreach (var assemblyName in type.AssemblyNames) {
                    if (!assemblyNameFilter(assemblyName))
                        continue;

                    typesByAssembly.GetOrAdd(assemblyName, _ => new HashSet<TypeDescription>(TypeDescriptionIdEqualityComparer.Default)).Add(type);
                }
            }

            foreach (var pair in typesByAssembly) {
                yield return new AnnotationsByAssembly(pair.Key, GetAnnotations(pair.Value));
            }
        }

        private ICollection<AnnotationsByMember> GetAnnotations(ISet<TypeDescription> types) {
            var annotations = new List<AnnotationsByMember>();
            foreach (var type in types) {
                if (type.ThreadSafety == TypeThreadSafety.NotParsed || type.ThreadSafety == TypeThreadSafety.NotFound || type.ThreadSafety == TypeThreadSafety.NotSafe)
                    continue;

                annotations.Add(new AnnotationsByMember(type.Id, new Annotation(ThreadSafeConstructorXmlId, type.ThreadSafety.ToString())));
            }

            return annotations;
        }
        
        //private void WriteNotParsed(DirectoryInfo directory, TypeDescription description) {
        //    var filePath = Path.Combine(directory.FullName, NonParsedFileName);
        //    using (var writer = new StreamWriter(filePath, true)) {
        //        writer.WriteLine(description.Id);
        //        writer.WriteLine(description.ThreadSafetyText);
        //        writer.WriteLine();
        //    }
        //}
    }
}
