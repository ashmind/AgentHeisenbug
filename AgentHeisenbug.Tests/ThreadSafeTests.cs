using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;
using JetBrains.Application.Settings;
using NUnit.Framework;

namespace AgentHeisenbug.Tests {
    [TestFixture]
    public class ThreadSafeTests : CSharpHighlightingTestBase {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore) {
            return highlighting.GetType().Name.Contains("ThreadSafe");
        }

        protected override string RelativeTestDataPath {
            get { return @"..\..\AgentHeisenbug.Tests\Files"; }
        }

        [Test]
        [TestCase("ThreadSafe.Fields.cs")]
        [TestCase("ThreadSafe.Properties.cs")]
        public void Test(string testName) {
            DoTestSolution(testName);
        }
    }
}
