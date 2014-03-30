using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Annotations;
using ThreadSafety.Highlightings;
using ThreadSafetyTips;

namespace ThreadSafety.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(CallToStaticMethodNotThreadSafe) })]
    public class ThreadSafetyStaticCallAnalyzer : IElementProblemAnalyzer {
        private readonly ThreadSafetyAnalyzerHelper helper;
        private readonly ThreadSafetyAnnotationCache cache;

        public ThreadSafetyStaticCallAnalyzer(ThreadSafetyAnalyzerHelper helper, ThreadSafetyAnnotationCache cache) {
            this.helper = helper;
            this.cache = cache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var call = (IInvocationExpression)element;
            if (!this.helper.MustBeThreadSafe(call))
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

            var safetyLevel = this.cache.GetThreadSafetyLevel(method);
            if (safetyLevel == ThreadSafetyLevel.Static || safetyLevel == ThreadSafetyLevel.All)
                return;
            
            consumer.AddHighlighting(new CallToStaticMethodNotThreadSafe(
                call.InvokedExpression, "Method '{0}' is not declared to be thread-safe.", method.ShortName
            ));
        }
    }
}
