using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;

namespace AgentHeisenbug.Highlightings.AnnotationFixSupport {
    public interface IFixableByAnnotation : IHighlighting {
        [NotNull]
        IEnumerable<AnnotationCandidate> GetCandidates();
    }
}
