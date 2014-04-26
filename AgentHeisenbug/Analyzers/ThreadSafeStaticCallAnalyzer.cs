using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Annotations;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(CallToNonThreadSafeStaticMethodInThreadSafeType) })]
    public class ThreadSafeStaticCallAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public ThreadSafeStaticCallAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugAnnotationCache annotationCache) {
            _preconditions = preconditions;
            _annotationCache = annotationCache;
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

            var safetyLevel = _annotationCache.GetAnnotations(method).ThreadSafety;
            if (safetyLevel.Has(ThreadSafety.Static))
                return;

            Assume.NotNullWorkaround(call.InvokedExpression != null, "call.InvokedExpression");
            consumer.AddHighlighting(new CallToNonThreadSafeStaticMethodInThreadSafeType(call.InvokedExpression, method.ShortName));
        }
    }
}