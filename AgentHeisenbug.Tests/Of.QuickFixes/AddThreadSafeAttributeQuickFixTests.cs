using AgentHeisenbug.QuickFixes;
using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.QuickFixes {
    [TestFixture]
    public class AddThreadSafeAttributeFixTest : CSharpQuickFixTestBase<AddThreadSafeAttributeFix> {
        protected override string RelativeTestDataPath {
            get { return @"QuickFixes\" + typeof(AddThreadSafeAttributeFix).Name; }
        }

        [TestCase("AddToBase_Simple.cs")]
        [TestCase("AddToSelf_Simple.cs")]
        [TestCase("AddToReference_Simple.cs")]
        [TestCase("AddToReference_GenericParameter.cs")]
        [TestCase("AddToReference_StaticMethod.cs")]
        public void Test(string fileName) {
            DoTestFiles(fileName);
        }
    }
}
