using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;
using JetBrains.Application.Settings;
using NUnit.Framework;

namespace AgentHeisenbug.Tests {
    [TestFixture]
    public class ReadOnlyTests : CSharpHighlightingTestBase {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore) {
            return highlighting.GetType().Name.Contains("ReadOnly");
        }

        protected override string RelativeTestDataPath {
            get { return @"..\..\AgentHeisenbug.Tests\Files"; }
        }

        [Test]
        [TestCase(@"ReadOnly\Fields.cs")]
        public void Test(string testName) {
            DoTestSolution(testName);
        }
    }
}
