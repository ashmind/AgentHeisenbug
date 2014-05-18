using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.QuickFixes.UsageChecking;

namespace AgentHeisenbug.QuickFixes {
    [QuickFix]
    public class MakeFieldReadOnlyFix : MakeReadonlyFix {
        public MakeFieldReadOnlyFix([NotNull] IForMutableField highlighiting) : base(highlighiting.FieldDeclaration.DeclaredElement) {
        }
    }
}
