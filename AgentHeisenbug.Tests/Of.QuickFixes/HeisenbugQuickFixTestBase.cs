using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
using JetBrains.ReSharper.Intentions.Extensibility;

namespace AgentHeisenbug.Tests.Of.QuickFixes {
    public abstract class HeisenbugQuickFixTestBase<TQuickFix> : CSharpQuickFixTestBase<TQuickFix>
        where TQuickFix : IQuickFix
    {
        protected override string RelativeTestDataPath {
            get { return Path.Combine("Of.QuickFixes\\Files", typeof(TQuickFix).Name); }
        }
    }
}
