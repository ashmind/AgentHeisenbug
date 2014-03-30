using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ThreadSafety.Annotations;
using ThreadSafetyTips;

namespace ThreadSafety.Analyzers {
    [PsiComponent]
    public class ThreadSafetyAnalyzerHelper {
        private readonly ThreadSafetyAnnotationCache cache;

        public ThreadSafetyAnalyzerHelper(ThreadSafetyAnnotationCache cache) {
            this.cache = cache;
        }

        public bool MustBeThreadSafe(ICSharpTreeNode node) {
            var typeNode = node.GetContainingTypeDeclaration();
            if (typeNode == null)
                return false;

            return this.cache.GetThreadSafetyLevel(typeNode.DeclaredElement) != ThreadSafetyLevel.None;
        }
    }
}
