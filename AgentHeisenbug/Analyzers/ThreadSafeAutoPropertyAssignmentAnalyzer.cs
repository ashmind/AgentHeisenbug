using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Analyzers.Base;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[] {
        typeof(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAssignmentAnalyzer : AutoPropertyAssignmentAnalyzerBase<ThreadSafe> {
        public ThreadSafeAutoPropertyAssignmentAnalyzer([NotNull] IAnalyzerPrecondition<ThreadSafe> precondition) : base(precondition) {
        }


        protected override IHighlighting NewHighlighting(IAssignmentExpression assignment, string propertyName, string staticOrInstance) {
            return new AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType(assignment, propertyName, staticOrInstance);
        }
    }
}
