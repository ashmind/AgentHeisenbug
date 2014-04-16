using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IParameterDeclaration) }, HighlightingTypes = new[] { typeof(ParameterOfNonThreadSafeTypeInThreadSafeMethod) })]
    public class ThreadSafeParameterAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;
        private readonly ReferencedTypeHelper referenceHelper;
        private readonly CodeAnnotationsCache annotationsCache;

        public ThreadSafeParameterAnalyzer(AnalyzerPreconditions preconditions, ReferencedTypeHelper referenceHelper, CodeAnnotationsCache annotationsCache) {
            this.preconditions = preconditions;
            this.referenceHelper = referenceHelper;
            this.annotationsCache = annotationsCache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var parameter = (IRegularParameterDeclaration)element;
            if (!this.preconditions.MustBeThreadSafe(parameter))
                return;

            var method = parameter.GetContainingNode<IMethodDeclaration>();
            if (method == null || this.annotationsCache.IsPure(method.DeclaredElement))
                return;

            if (!this.referenceHelper.IsInstanceThreadSafeOrReadOnlyOrImmutable(parameter.Type)) {
                consumer.AddHighlighting(new ParameterOfNonThreadSafeTypeInThreadSafeMethod(
                    parameter.TypeUsage, parameter.DeclaredName, parameter.Type.GetPresentableName(CSharpLanguage.Instance)
                ));
            }
        }
    }
}
