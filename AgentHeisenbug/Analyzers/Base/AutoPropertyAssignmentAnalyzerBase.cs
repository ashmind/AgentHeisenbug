using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Analyzers.Base {
    public abstract class AutoPropertyAssignmentAnalyzerBase : IElementProblemAnalyzer {
        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var assignment = (IAssignmentExpression)element;
            if (!MustBeAnalyzed(assignment))
                return;

            var reference = assignment.Dest as IReferenceExpression;
            if (reference == null)
                return;

            var property = reference.Reference.Resolve().DeclaredElement as IProperty;
            if (property == null)
                return;

            var containingType = assignment.GetContainingTypeDeclaration();
            if (containingType == null || containingType.DeclaredElement == null || !containingType.DeclaredElement.Properties.Contains(property))
                return;

            if (!CSharpDeclaredElementUtil.IsAutoProperty(property))
                return;

            var containingMethod = assignment.GetContainingTypeMemberDeclaration();
            if (containingMethod != null && containingMethod.DeclaredElement is IConstructor && containingMethod.DeclaredElement.IsStatic == property.IsStatic)
                return;

            consumer.AddHighlighting(NewHighlighting(
                assignment, property.ShortName, property.IsStatic ? "static " : ""
            ));
        }

        protected abstract bool MustBeAnalyzed([NotNull] IAssignmentExpression assignment);
        protected abstract IHighlighting NewHighlighting([NotNull] IAssignmentExpression assignment, [NotNull] string propertyName, string staticOrInstance);
    }
}
