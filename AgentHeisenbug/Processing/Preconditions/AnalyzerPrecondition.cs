using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Processing.Preconditions {
    public class AnalyzerPrecondition<TFeature> : IAnalyzerPrecondition<TFeature>
        where TFeature : IFeatureMarker
    {
        [NotNull] protected HeisenbugFeatureProvider FeatureProvider { get; private set; }

        protected AnalyzerPrecondition([NotNull] HeisenbugFeatureProvider featureProvider) {
            FeatureProvider = featureProvider;
        }

        public virtual bool Applies([NotNull] ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return FeatureProvider.GetFeatures(parameter).Has<TFeature>();
        }

        public bool Applies([NotNull] ITreeNode node) {
            Argument.NotNull("node", node);
            var typeNode = (node as ITypeDeclaration) ?? node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null || typeNode.DeclaredElement == null)
                return false;
            
            return Applies(node, typeNode.DeclaredElement);
        }
        
        protected virtual bool Applies([NotNull] ITreeNode node, [NotNull] ITypeElement containingType) {
            return FeatureProvider.GetFeatures(containingType).Has<TFeature>();
        }
    }

    #region Workaround for R# not supporting open generics
    [PsiComponent]
    public class ReadOnlyAnalyzerPrecondition : AnalyzerPrecondition<ReadOnly> {
        public ReadOnlyAnalyzerPrecondition([NotNull] HeisenbugFeatureProvider featureProvider) : base(featureProvider) {}
    }

    [PsiComponent]
    public class InstanceThreadSafeAnalyzerPrecondition : AnalyzerPrecondition<InstanceThreadSafe> {
        public InstanceThreadSafeAnalyzerPrecondition([NotNull] HeisenbugFeatureProvider featureProvider) : base(featureProvider) {}
    }
    #endregion
}
