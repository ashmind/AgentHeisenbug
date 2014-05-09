using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IReferenceExpression) }, HighlightingTypes = new[] { typeof(AssignmentToNonThreadSafeStaticMemberInThreadSafeType) })]
    public class ThreadSafeStaticReferenceAnalyzer : ElementProblemAnalyzer<IReferenceExpression> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeStaticReferenceAnalyzer(
            [NotNull] AnalyzerPreconditions preconditions,
            [NotNull] HeisenbugFeatureProvider featureProvider
        ) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IReferenceExpression element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeThreadSafe(element))
                return;
            
            var resolved = element.Reference.Resolve();
            var member = resolved.DeclaredElement as ITypeMember;
            if (member == null)
                return;

            var isProperty = member is IProperty;
            if (!isProperty && !(member is IField))
                return;

            if (!member.IsStatic || member.IsConstant())
                return;

            if (_featureProvider.GetFeatures(member).IsStaticAccessThreadSafe)
                return;

            var propertyOrField = isProperty ? "property" : "field";
            consumer.AddHighlighting(new AssignmentToNonThreadSafeStaticMemberInThreadSafeType(element.NotNull(), propertyOrField, member.ShortName));
        }
    }
}