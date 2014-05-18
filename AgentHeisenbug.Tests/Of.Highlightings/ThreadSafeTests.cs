using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.Highlightings {
    [TestFixture]
    [HighlightingFilter("ThreadSafe")]
    public class ThreadSafeTests : HeisenbugHighlightingTestBase {
        [Test]
        [TestCase("Fields.cs")]
        [TestCase("Properties.cs")]
        [TestCase("BclTypes.cs")]
        [HighlightingFilter(exclude: "Inherited|Implemented")]
        public void Test(string testName) {
            DoTestSolution(testName);
        }

        [Test]
        [HighlightingFilter(include: @"AgentHeisenbug\..*Static.*")]
        public void ExternalStatic() {
            DoTestSolution("StaticAccess.cs");
        }

        [Test]
        [HighlightingFilter(include: @"AgentHeisenbug\..*Parameter")]
        public void Parameters() {
            DoTestSolution("Parameters.cs");
        }

        [Test]
        public void Inheritance() {
            DoTestSolution("Inheritance.cs");
        }
    }
}
