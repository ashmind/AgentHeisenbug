using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Annotations;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Helpers {
    [PsiComponent]
    public class AnalyzerPreconditions {
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public AnalyzerPreconditions([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        public bool MustBeThreadSafe([NotNull] ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return _annotationCache.GetAnnotations(parameter).ThreadSafety == ThreadSafety.Instance;
        }

        public bool MustBeThreadSafe([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);
            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            var safety = _annotationCache.GetAnnotations(typeNode.DeclaredElement).ThreadSafety;
            var member = node.GetContainingNode<ICSharpTypeMemberDeclaration>();
            if (member == null)
                return safety == ThreadSafety.All;

            return member.IsStatic ? safety.Has(ThreadSafety.Static) : safety.Has(ThreadSafety.Instance);
        }

        public bool MustBeReadOnly([NotNull] ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return _annotationCache.GetAnnotations(parameter).IsReadOnly;
        }

        public bool MustBeReadOnly([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);

            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            return _annotationCache.GetAnnotations(typeNode.DeclaredElement).IsReadOnly;
        }
    }
}
