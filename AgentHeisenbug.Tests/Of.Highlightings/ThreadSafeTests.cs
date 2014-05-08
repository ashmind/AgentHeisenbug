using NUnit.Framework;

namespace AgentHeisenbug.Tests.Of.Highlightings {
    [TestFixture]
    [HighlightingFilter("ThreadSafe")]
    public class ThreadSafeTests : HeisenbugHighlightingTestBase {
        protected override string RelativeTestDataPath {
            get { return "ThreadSafe"; }
        }

        [Test]
        [TestCase("Fields.cs")]
        [TestCase("Properties.cs")]
        [TestCase("BclTypes.cs")]
        [TestCase("StaticCalls.cs")]
        [HighlightingFilter(exclude: "Inherited|Implemented")]
        public void Test(string testName) {
            DoTestSolution(testName);
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
