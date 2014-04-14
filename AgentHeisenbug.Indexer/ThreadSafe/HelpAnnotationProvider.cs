using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Annotations.Generated;
using AshMind.Extensions;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class HelpAnnotationProvider : IAnnotationProvider {
        private static readonly string ThreadSafeConstructorXmlId = "M:" + typeof(GeneratedThreadSafeAttribute).FullName + ".#ctor";

        private readonly HelpRawReader reader;
        private readonly Action<TypeHelp> reportParsingFailure;

        public HelpAnnotationProvider(HelpRawReader reader, Action<TypeHelp> reportParsingFailure = null) {
            this.reader = reader;
            this.reportParsingFailure = reportParsingFailure ?? (s => { });
        }

        public IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(Func<string, bool> assemblyNameFilter, Action<double> reportProgress) {
            var typesByAssembly = new Dictionary<string, ISet<TypeHelp>>();
            foreach (var type in this.reader.ReadFiles(reportProgress)) {
                foreach (var assemblyName in type.AssemblyNames) {
                    if (!assemblyNameFilter(assemblyName))
                        continue;

                    typesByAssembly.GetOrAdd(assemblyName, _ => new HashSet<TypeHelp>(TypeHelpIdEqualityComparer.Default)).Add(type);
                }
            }

            foreach (var pair in typesByAssembly) {
                yield return new AnnotationsByAssembly(pair.Key, GetAnnotations(pair.Value));
            }
        }

        private ICollection<AnnotationsByMember> GetAnnotations(ISet<TypeHelp> types) {
            var annotations = new List<AnnotationsByMember>();
            foreach (var type in types) {
                if (type.ThreadSafety == TypeThreadSafety.NotParsed) {
                    reportParsingFailure(type);
                    continue;
                }

                if (type.ThreadSafety == TypeThreadSafety.NotFound || type.ThreadSafety == TypeThreadSafety.NotSafe)
                    continue;

                annotations.Add(new AnnotationsByMember(type.Id, new Annotation(ThreadSafeConstructorXmlId, type.ThreadSafety.ToString())));
            }

            return annotations;
        }
    }
}
