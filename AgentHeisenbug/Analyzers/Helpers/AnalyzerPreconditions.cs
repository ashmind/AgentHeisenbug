﻿using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Analyzers.Helpers {
    [PsiComponent]
    public class AnalyzerPreconditions {
        private readonly HeisenbugAnnotationCache annotationCache;

        public AnalyzerPreconditions(HeisenbugAnnotationCache annotationCache) {
            this.annotationCache = annotationCache;
        }

        public bool MustBeThreadSafe(ITreeNode node) {
            var typeNode = node.GetContainingNode<ITypeDeclaration>();
            if (typeNode == null)
                return false;

            var safety = this.annotationCache.GetThreadSafety(typeNode.DeclaredElement);
            var member = node.GetContainingNode<ICSharpTypeMemberDeclaration>();
            if (member == null)
                return safety.Instance && safety.Static;

            return member.IsStatic ? safety.Static : safety.Instance;
        }

        public bool MustBeReadOnly(ICSharpTreeNode node) {
            var typeNode = node.GetContainingTypeDeclaration();
            if (typeNode == null)
                return false;

            return this.annotationCache.IsReadOnly(typeNode.DeclaredElement);
        }
    }
}
