using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Processing {
    [PsiComponent]
    public class AnalyzerPreconditions {
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public AnalyzerPreconditions([NotNull] HeisenbugFeatureProvider featureProvider) {
            _featureProvider = featureProvider;
        }

        public bool MustBeThreadSafe([NotNull] ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return _featureProvider.GetFeatures(parameter).DeclaredThreadSafety == ThreadSafety.Instance;
        }

        public bool MustBeThreadSafe([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);
            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            var safety = _featureProvider.GetFeatures(typeNode.DeclaredElement).DeclaredThreadSafety;
            var member = node.GetContainingNode<ICSharpTypeMemberDeclaration>();
            if (member == null)
                return safety == ThreadSafety.All;

            return member.IsStatic ? safety.Has(ThreadSafety.Static) : safety.Has(ThreadSafety.Instance);
        }

        public bool MustBeReadOnly([NotNull] ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return _featureProvider.GetFeatures(parameter).IsReadOnly;
        }

        public bool MustBeReadOnly([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);

            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;

            return _featureProvider.GetFeatures(typeNode.DeclaredElement).IsReadOnly;
        }
    }
}
