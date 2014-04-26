using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AgentHeisenbug.Indexer {
    public interface IAnnotationProvider {
        [NotNull]
        IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(
            [NotNull] Func<string, bool> assemblyNameFilter,
            [NotNull] Action<double> reportProgress
        );
    }
}