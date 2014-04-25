using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Analyzers.Base;
using AgentHeisenbug.Highlightings;
using JetBrains.Annotations;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[] {
        typeof(AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType)
    })]
    public class ReadOnlyAutoPropertyAssignmentAnalyzer : AutoPropertyAssignmentAnalyzerBase {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;

        public ReadOnlyAutoPropertyAssignmentAnalyzer([NotNull] AnalyzerPreconditions preconditions) {
            _preconditions = preconditions;
        }

        protected override bool MustBeAnalyzed(IAssignmentExpression assignment) {
            return _preconditions.MustBeReadOnly(assignment);
        }

        protected override IHighlighting NewHighlighting(IAssignmentExpression assignment, string propertyName, string staticOrInstance) {
            return new AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType(assignment, propertyName, staticOrInstance);
        }
    }
}
