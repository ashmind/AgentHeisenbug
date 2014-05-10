using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Processing.Preconditions {
    [PsiComponent]
    public class ThreadSafeAnalyzerPrecondition : AnalyzerPrecondition<ThreadSafe> {
        public ThreadSafeAnalyzerPrecondition([NotNull] HeisenbugFeatureProvider featureProvider) : base(featureProvider) {
        }

        public override bool Applies(ITypeParameter parameter) {
            Argument.NotNull("parameter", parameter);
            return FeatureProvider.GetFeatures(parameter).DeclaredThreadSafety == ThreadSafety.Instance;
        }

        protected override bool Applies(ITreeNode node, ITypeElement containingType) {
            var safety = FeatureProvider.GetFeatures(containingType).DeclaredThreadSafety;
            var member = node.GetContainingNode<ICSharpTypeMemberDeclaration>();
            if (member == null)
                return safety == ThreadSafety.All;

            return member.IsStatic ? safety.Has(ThreadSafety.Static) : safety.Has(ThreadSafety.Instance);
        }
    }
}
