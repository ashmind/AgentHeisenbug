using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IRegularParameterDeclaration) }, HighlightingTypes = new[] { typeof(ParameterOfNonThreadSafeTypeInThreadSafeMethod) })]
    public class ThreadSafeParameterAnalyzer : ElementProblemAnalyzer<IRegularParameterDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;
        [NotNull] private readonly CodeAnnotationsCache _annotationsCache;

        public ThreadSafeParameterAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider, [NotNull] CodeAnnotationsCache annotationsCache) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
            _annotationsCache = annotationsCache;
        }

        protected override void Run(IRegularParameterDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeThreadSafe(element))
                return;

            var method = element.GetContainingNode<IMethodDeclaration>();
            if (method == null || method.IsPrivate() || _annotationsCache.IsPure(method.DeclaredElement))
                return;

            TypeUsageTreeValidator.Validate(
                element.TypeUsage.NotNull(),
                element.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsInstanceAccessThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new ParameterOfNonThreadSafeTypeInThreadSafeMethod(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, element.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
