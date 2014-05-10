using System.Linq;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;

namespace AgentHeisenbug.Analyzers.Base {
    public abstract class AutoPropertyAssignmentAnalyzerBase<TFeature> : ElementProblemAnalyzer<IAssignmentExpression>
        where TFeature : IFeatureMarker
    {
        [NotNull] private readonly IAnalyzerPrecondition<TFeature> _precondition;

        protected AutoPropertyAssignmentAnalyzerBase([NotNull] IAnalyzerPrecondition<TFeature> precondition) {
            _precondition = precondition;
        }

        protected override void Run(IAssignmentExpression element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_precondition.Applies(element))
                return;

            var reference = element.Dest as IReferenceExpression;
            if (reference == null)
                return;

            var property = reference.Reference.Resolve().DeclaredElement as IProperty;
            if (property == null)
                return;

            var containingType = element.GetContainingTypeDeclaration();
            if (containingType == null || containingType.DeclaredElement == null || !containingType.DeclaredElement.Properties.Contains(property))
                return;

            if (!CSharpDeclaredElementUtil.IsAutoProperty(property))
                return;

            var containingMethod = element.GetContainingTypeMemberDeclaration();
            if (containingMethod != null && containingMethod.DeclaredElement is IConstructor && containingMethod.DeclaredElement.IsStatic == property.IsStatic)
                return;

            consumer.AddHighlighting(NewHighlighting(
                element, property.ShortName, property.IsStatic ? "static " : ""
            ));
        }

        protected abstract IHighlighting NewHighlighting([NotNull] IAssignmentExpression assignment, [NotNull] string propertyName, string staticOrInstance);
    }
}
