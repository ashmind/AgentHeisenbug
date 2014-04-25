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

        public bool MustBeThreadSafe([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);
            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            var safety = _annotationCache.GetThreadSafety(typeNode.DeclaredElement);
            var member = node.GetContainingNode<ICSharpTypeMemberDeclaration>();
            if (member == null)
                return safety == ThreadSafety.All;

            return member.IsStatic ? safety.Has(ThreadSafety.Static) : safety.Has(ThreadSafety.Instance);
        }

        public bool MustBeReadOnly([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);

            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            return this._annotationCache.IsReadOnly(typeNode.DeclaredElement);
        }
    }
}
