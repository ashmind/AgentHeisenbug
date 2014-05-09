using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(CallToNonThreadSafeStaticMethodInThreadSafeType) })]
    public class ThreadSafeStaticCallAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;
        [NotNull] private readonly CodeAnnotationsCache _defaultAnnotationCache;

        public ThreadSafeStaticCallAnalyzer(
            [NotNull] AnalyzerPreconditions preconditions,
            [NotNull] HeisenbugFeatureProvider featureProvider,
            [NotNull] CodeAnnotationsCache defaultAnnotationCache
        ) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
            _defaultAnnotationCache = defaultAnnotationCache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var call = (IInvocationExpression)element;
            if (!_preconditions.MustBeThreadSafe(call))
                return;

            var reference = call.InvocationExpressionReference;
            if (reference == null)
                return;

            var resolved = reference.Resolve();
            var method = resolved.DeclaredElement as IMethod;
            if (method == null || !method.IsStatic)
                return;

            if (_featureProvider.GetFeatures(method).IsStaticAccessThreadSafe)
                return;

            if (_defaultAnnotationCache.IsPure(method))
                return;

            consumer.AddHighlighting(new CallToNonThreadSafeStaticMethodInThreadSafeType(call.InvokedExpression.NotNull(), method.ShortName));
        }
    }
}