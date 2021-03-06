using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using AgentHeisenbug.QuickFixes;

namespace AgentHeisenbug.Tests.Of.QuickFixes {
    [TestFixture]
    public class MakeAutoPropertySetterPrivateFixTests : HeisenbugQuickFixTestBase<MakeAutoPropertySetterPrivateFix> {
        [TestCase("ReadOnlyClass_Simple.cs")]
        [TestCase("ThreadSafeClass_Simple.cs")]
        public void Test(string fileName) {
            DoTestFiles(fileName);
        }
    }
}
