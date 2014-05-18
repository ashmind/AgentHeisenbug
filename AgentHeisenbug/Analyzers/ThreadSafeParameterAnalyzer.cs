using System.Linq;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Highlightings.ThreadSafe;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IRegularParameterDeclaration) }, HighlightingTypes = new[] { typeof(ParameterOfNonThreadSafeTypeInThreadSafeMethod) })]
    public class ThreadSafeParameterAnalyzer : ElementProblemAnalyzer<IRegularParameterDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<InstanceThreadSafe> _precondition;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;
        [NotNull] private readonly ITypeUsageTreeValidator<InstanceThreadSafe> _typeUsageValidator;

        public ThreadSafeParameterAnalyzer(
            [NotNull] IAnalyzerPrecondition<ThreadSafe> precondition,
            [NotNull] HeisenbugFeatureProvider featureProvider,
            [NotNull] ITypeUsageTreeValidator<InstanceThreadSafe> typeUsageValidator
        ) {
            _precondition = precondition;
            _featureProvider = featureProvider;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IRegularParameterDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_precondition.Applies(element))
                return;

            var method = element.GetContainingNode<IMethodDeclaration>();
            if (method == null || method.IsPrivate() || (method.DeclaredElement != null && _featureProvider.GetFeatures(method.DeclaredElement).IsPure))
                return;

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new ParameterOfNonThreadSafeTypeInThreadSafeMethod(element, method, invalid.Usage, invalid.Type));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
