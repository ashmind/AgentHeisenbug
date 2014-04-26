using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Analyzers.Base;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[] {
        typeof(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAssignmentAnalyzer : AutoPropertyAssignmentAnalyzerBase {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;

        public ThreadSafeAutoPropertyAssignmentAnalyzer([NotNull] AnalyzerPreconditions preconditions) {
            _preconditions = preconditions;
        }

        protected override bool MustBeAnalyzed(IAssignmentExpression assignment) {
            return _preconditions.MustBeThreadSafe(assignment);
        }

        protected override IHighlighting NewHighlighting(IAssignmentExpression assignment, string propertyName, string staticOrInstance) {
            return new AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType(assignment, propertyName, staticOrInstance);
        }
    }
}
