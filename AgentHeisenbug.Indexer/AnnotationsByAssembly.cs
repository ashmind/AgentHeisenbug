using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer {
    public class AnnotationsByAssembly {
        public string AssemblyName { get; private set; }
        public ICollection<AnnotationsByMember> Annotations { get; private set; }

        public AnnotationsByAssembly(string assemblyName, ICollection<AnnotationsByMember> annotations) {
            AssemblyName = assemblyName;
            Annotations = annotations;
        }
    }
}
