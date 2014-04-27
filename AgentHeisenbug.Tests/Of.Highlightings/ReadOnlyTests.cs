using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.Highlightings {
    [TestFixture]
    [HighlightingFilter("ReadOnly")]
    public class ReadOnlyTests : HeisenbugHighlightingTestBase {
        protected override string RelativeTestDataPath {
            get { return "ReadOnly"; }
        }

        [Test]
        [TestCase("Fields.cs")]
        [TestCase("Properties.cs")]
        [TestCase("Inheritance.cs")]
        [TestCase("BclTypes.cs")]
        [TestCase("ArrayPointerEtcTypes.cs")]
        public void Test(string testName) {
            DoTestSolution(testName);
        }
    }
}
