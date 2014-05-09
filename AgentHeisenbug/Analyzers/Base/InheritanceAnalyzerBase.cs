using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Base {
    public abstract class InheritanceAnalyzerBase : ElementProblemAnalyzer<IClassLikeDeclaration> {
        protected override void Run(IClassLikeDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (element.DeclaredElement == null || IsAnnotated(element.DeclaredElement))
                return;

            var superTypes = element.SuperTypes;
            if (superTypes == null)
                return;

            var index = -1;
            foreach (var superType in superTypes) {
                index += 1;
                if (superType == null)
                    continue;

                var superTypeElement = superType.GetTypeElement();
                if (superTypeElement == null)
                    continue;

                if (!IsAnnotated(superTypeElement))
                    continue;

                consumer.AddHighlighting(Highlighting(element, superType, element.SuperTypeUsageNodes[index].NotNull()));
            }
        }

        protected abstract bool IsAnnotated([NotNull] ITypeElement type);

        private IHighlighting Highlighting([NotNull] IClassLikeDeclaration type, [NotNull] IType superType, [NotNull] IDeclaredTypeUsage superTypeUsage) {
            // ReSharper disable once PossibleNullReferenceException
            var typeName = type.DeclaredElement.ShortName;
            if (superType.IsInterfaceType())
                return NewInterfaceImplementedByNonAnnotatedType(superTypeUsage, superType.GetCSharpPresentableName(), typeName);

            return NewClassInheritedByNonAnnotatedType(superTypeUsage, superType.GetCSharpPresentableName(), typeName);
        }

        protected abstract IHighlighting NewInterfaceImplementedByNonAnnotatedType([NotNull] IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName);
        protected abstract IHighlighting NewClassInheritedByNonAnnotatedType([NotNull] IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName);
    }
}