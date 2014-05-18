using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Analyzers.Base;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Highlightings.ReadOnly;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[] {
        typeof(AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType)
    })]
    public class ReadOnlyAutoPropertyAssignmentAnalyzer : AutoPropertyAssignmentAnalyzerBase<ReadOnly> {
        public ReadOnlyAutoPropertyAssignmentAnalyzer([NotNull] IAnalyzerPrecondition<ReadOnly> precondition) : base(precondition) {
        }
        
        protected override IHighlighting NewHighlighting(IAssignmentExpression assignment, string propertyName, string staticOrInstance) {
            return new AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType(assignment, propertyName, staticOrInstance);
        }
    }
}
