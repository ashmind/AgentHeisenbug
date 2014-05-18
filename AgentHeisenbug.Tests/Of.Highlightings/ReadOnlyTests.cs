using System.IO;
using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.Highlightings {
    [TestFixture]
    [HighlightingFilter("ReadOnly")]
    public class ReadOnlyTests : HeisenbugHighlightingTestBase {
        [Test]
        [TestCase("Fields.cs")]
        [TestCase("Properties.cs")]
        [TestCase("Inheritance.cs")]
        [TestCase("BclTypes.cs")]
        [TestCase("ArrayPointerEtcTypes.cs")]
        [TestCase("GenericTypes.cs")]
        [TestCase("ReadOnlyClass_Simple.cs")]
        public void Test(string testName) {
            DoTestSolution(testName);
        }
    }
}
