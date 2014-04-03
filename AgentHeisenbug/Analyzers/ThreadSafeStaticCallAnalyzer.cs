using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(CallToNotThreadSafeStaticMethodInThreadSafeType) })]
    public class ThreadSafeStaticCallAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;
        private readonly HeisenbugAnnotationCache annotationCache;

        public ThreadSafeStaticCallAnalyzer(AnalyzerPreconditions preconditions, HeisenbugAnnotationCache annotationCache) {
            this.preconditions = preconditions;
            this.annotationCache = annotationCache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var call = (IInvocationExpression)element;
            if (!this.preconditions.MustBeThreadSafe(call))
                return;

            var reference = call.InvocationExpressionReference;
            if (reference == null)
                return;

            var resolved = reference.Resolve();
            if (resolved.ResolveErrorType != ResolveErrorType.OK)
                return;

            var method = resolved.DeclaredElement as IMethod;
            if (method == null || !method.IsStatic)
                return;

            var safetyLevel = this.annotationCache.GetThreadSafety(method);
            if (!safetyLevel.Static)
                return;

            consumer.AddHighlighting(new CallToNotThreadSafeStaticMethodInThreadSafeType(
                call.InvokedExpression, "Method '{0}' is not declared to be thread-safe.", method.ShortName
            ));
        }
    }
}
