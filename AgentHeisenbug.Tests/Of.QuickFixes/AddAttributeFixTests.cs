using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.TextControl;
using NUnit.Framework;
using AgentHeisenbug.QuickFixes;

namespace AgentHeisenbug.Tests.Of.QuickFixes {
    [TestFixture]
    public class AddAttributeFixTests : HeisenbugQuickFixTestBase<AddAttributeFix> {
        [NotNull] private string _annotationTypeName;

        [TestCase("AddToBase_Simple.cs")]
        [TestCase("AddToSelf_Simple.cs")]
        [TestCase("AddToReference_Simple.cs")]
        [TestCase("AddToReference_GenericParameter.cs")]
        [TestCase("AddToReference_StaticMethod.cs")]
        public void ThreadSafe(string fileName) {
            UseAnnotationType(AnnotationTypeNames.ThreadSafe);
            DoTestFiles(fileName);
        }

        [TestCase("AddToBase_Simple.cs")]
        [TestCase("AddToReference_Simple.cs")]
        [TestCase("AddToReference_GenericParameter.cs")]
        public void ReadOnly(string fileName) {
            UseAnnotationType(AnnotationTypeNames.ReadOnly);
            DoTestFiles(fileName);
        }

        private void UseAnnotationType([NotNull] string typeName) {
            _annotationTypeName = typeName;
        }

        protected override Func<IList<IntentionAction>, IBulbAction> GetBulbItemSelector(ITextControl textControl) {
            return items => items.Select(i => i.BulbAction)
                                 .OfType<AddAttributeBulbAction>()
                                 .FirstOrDefault(a => a.AttributeType.ShortName == _annotationTypeName);
        }

        protected override void DoTestSolution(params string[] fileSet) {
            // ReSharper disable AssignNullToNotNullAttribute
            var annotationName = _annotationTypeName.SubstringBefore("Attribute");
            base.DoTestSolution(fileSet.Select(f => Path.Combine(annotationName, f)).ToArray());
            // ReSharper enable AssignNullToNotNullAttribute
        }
    }
}
