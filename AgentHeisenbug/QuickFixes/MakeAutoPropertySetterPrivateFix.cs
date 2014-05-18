using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.QuickFixes;

namespace AgentHeisenbug.QuickFixes {
    [QuickFix]
    public class MakeAutoPropertySetterPrivateFix : MakePrivateFix {
        public MakeAutoPropertySetterPrivateFix([NotNull] IForMutableAutoProperty highlighiting) : base(highlighiting.SetterDeclaration, true) {
        }
    }
}
