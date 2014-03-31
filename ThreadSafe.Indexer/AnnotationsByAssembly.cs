using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer {
    public class AnnotationsByAssembly {
        public string AssemblyName { get; private set; }
        public ICollection<Annotation> Annotations { get; private set; }

        public AnnotationsByAssembly(string assemblyName, ICollection<Annotation> annotations) {
            AssemblyName = assemblyName;
            Annotations = annotations;
        }
    }
}
