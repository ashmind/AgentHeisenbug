using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.AnnotationFixSupport;
using AgentHeisenbug.QuickFixes;
using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.TextControl;
using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.QuickFixes {
    [TestFixture]
    public class AddThreadSafeAttributeFixTest : CSharpQuickFixTestBase<AddAttributeFix> {
        protected override string RelativeTestDataPath {
            get { return @"QuickFixes\" + typeof(AddAttributeFix).Name + ".ThreadSafe"; }
        }

        [TestCase("AddToBase_Simple.cs")]
        [TestCase("AddToSelf_Simple.cs")]
        [TestCase("AddToReference_Simple.cs")]
        [TestCase("AddToReference_GenericParameter.cs")]
        [TestCase("AddToReference_StaticMethod.cs")]
        public void Test(string fileName) {
            DoTestFiles(fileName);
        }

        protected override Func<IList<IntentionAction>, IBulbAction> GetBulbItemSelector(ITextControl textControl) {
            return items => items.Select(i => i.BulbAction)
                                 .OfType<AddAttributeBulbAction>()
                                 .FirstOrDefault(a => a.AttributeType.ShortName == AnnotationTypeNames.ThreadSafe);
        }
    }
}
