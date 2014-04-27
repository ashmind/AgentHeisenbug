using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace AgentHeisenbug.Tests.Of.Highlightings {
    public class HighlightingFilterAttribute : Attribute {
        [CanBeNull] public Regex Include { get; private set; }
        [CanBeNull] public Regex Exclude { get; private set; }

        public HighlightingFilterAttribute([CanBeNull] string include = null, [CanBeNull] string exclude = null) {
            Include = include != null ? new Regex(include) : null;
            Exclude = exclude != null ? new Regex(exclude) : null;
        }
    }
}
