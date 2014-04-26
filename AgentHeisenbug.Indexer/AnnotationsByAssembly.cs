using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug.Indexer {
    public class AnnotationsByAssembly {
        [NotNull] public string AssemblyName { get; private set; }
        [NotNull] public ICollection<AnnotationsByMember> Annotations { get; private set; }

        public AnnotationsByAssembly([NotNull] string assemblyName, [NotNull] ICollection<AnnotationsByMember> annotations) {
            AssemblyName = assemblyName;
            Annotations = annotations;
        }
    }
}
