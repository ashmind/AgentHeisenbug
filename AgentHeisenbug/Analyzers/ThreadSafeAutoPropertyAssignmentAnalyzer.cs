using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[] {
        typeof(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAssignmentAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;

        public ThreadSafeAutoPropertyAssignmentAnalyzer(AnalyzerPreconditions preconditions) {
            this.preconditions = preconditions;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var assignment = (IAssignmentExpression)element;
            if (!preconditions.MustBeThreadSafe(assignment))
                return;

            var assignmentContainingMethod = assignment.GetContainingTypeMemberDeclaration().DeclaredElement;
            if (assignmentContainingMethod is IConstructor)
                return;
            
            var assignmentContainingType = assignment.GetContainingTypeDeclaration().DeclaredElement;
            if (assignmentContainingType == null)
                return;

            var reference = assignment.Dest as IReferenceExpression;
            if (reference == null)
                return;

            var resolved = reference.Reference.Resolve();
            if (resolved.ResolveErrorType != ResolveErrorType.OK)
                return;

            var property = resolved.DeclaredElement as IProperty;
            if (property == null)
                return;

            var propertyContainingType = property.GetContainingType();
            if (propertyContainingType == null)
                return;

            if (!DeclaredElementEqualityComparer.TypeElementComparer.Equals(propertyContainingType, assignmentContainingType))
                return;

            if (!property.GetDeclarations().Cast<IPropertyDeclaration>().Any(d => d.IsAuto))
                return;

            consumer.AddHighlighting(new AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType(assignment, property.ShortName));
        }
    }
}
