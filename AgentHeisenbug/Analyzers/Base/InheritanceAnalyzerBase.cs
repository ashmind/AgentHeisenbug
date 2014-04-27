using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Base {
    public abstract class InheritanceAnalyzerBase : IElementProblemAnalyzer {
        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var type = ((IClassLikeDeclaration)element);
            if (type.DeclaredElement == null || IsAnnotated(type.DeclaredElement))
                return;

            var superTypes = type.SuperTypes;
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

                consumer.AddHighlighting(Highlighting(type, superType, type.SuperTypeUsageNodes[index].NotNull()));
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