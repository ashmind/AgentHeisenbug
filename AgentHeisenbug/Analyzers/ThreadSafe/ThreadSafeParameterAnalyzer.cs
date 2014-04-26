using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IRegularParameterDeclaration) }, HighlightingTypes = new[] { typeof(ParameterOfNonThreadSafeTypeInThreadSafeMethod) })]
    public class ThreadSafeParameterAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly ReferencedTypeHelper _referenceHelper;
        [NotNull] private readonly CodeAnnotationsCache _annotationsCache;

        public ThreadSafeParameterAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] ReferencedTypeHelper referenceHelper, [NotNull] CodeAnnotationsCache annotationsCache) {
            _preconditions = preconditions;
            _referenceHelper = referenceHelper;
            _annotationsCache = annotationsCache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var parameter = (IRegularParameterDeclaration)element;
            if (!_preconditions.MustBeThreadSafe(parameter))
                return;

            var method = parameter.GetContainingNode<IMethodDeclaration>();
            if (method == null || this._annotationsCache.IsPure(method.DeclaredElement))
                return;

            Assume.NotNullWorkaround(parameter.Type != null, "parameter.Type");
            Assume.NotNullWorkaround(parameter.TypeUsage != null, "parameter.TypeUsage");
            if (!_referenceHelper.IsInstanceThreadSafeOrReadOnly(parameter.Type)) {
                consumer.AddHighlighting(new ParameterOfNonThreadSafeTypeInThreadSafeMethod(
                    parameter.TypeUsage, parameter.DeclaredName, parameter.Type.GetCSharpPresentableName()
                ));
            }
        }
    }
}
