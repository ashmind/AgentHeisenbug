using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;

namespace AgentHeisenbug.Highlightings.Common {
    public interface IFixableByAnnotation : IHighlighting {
        [NotNull]
        IEnumerable<AnnotationCandidate> GetCandidates();
    }
}
