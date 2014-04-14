using System;
using System.Collections.Generic;

namespace AgentHeisenbug.Indexer {
    public interface IAnnotationProvider {
        IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(
            Func<string, bool> assemblyNameFilter,
            Action<double> reportProgress
        );
    }
}