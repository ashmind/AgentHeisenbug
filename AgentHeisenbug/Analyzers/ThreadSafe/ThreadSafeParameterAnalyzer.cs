using System.Linq;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.TypeUsageTree;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IRegularParameterDeclaration) }, HighlightingTypes = new[] { typeof(ParameterOfNonThreadSafeTypeInThreadSafeMethod) })]
    public class ThreadSafeParameterAnalyzer : ElementProblemAnalyzer<IRegularParameterDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;
        [NotNull] private readonly ThreadSafeTypeUsageValidator _typeUsageValidator;

        public ThreadSafeParameterAnalyzer(
            [NotNull] AnalyzerPreconditions preconditions,
            [NotNull] HeisenbugFeatureProvider featureProvider,
            [NotNull] ThreadSafeTypeUsageValidator typeUsageValidator
        ) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IRegularParameterDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeThreadSafe(element))
                return;

            var method = element.GetContainingNode<IMethodDeclaration>();
            if (method == null || method.IsPrivate() || (method.DeclaredElement != null && _featureProvider.GetFeatures(method.DeclaredElement).IsPure))
                return;

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new ParameterOfNonThreadSafeTypeInThreadSafeMethod(
                    invalid.Usage, element.DeclaredName, invalid.Type.GetCSharpPresentableName()
                ));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
