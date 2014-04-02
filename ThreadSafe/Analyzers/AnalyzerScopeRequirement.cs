﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ThreadSafety.Annotations;
using ThreadSafetyTips;

namespace ThreadSafety.Analyzers {
    [PsiComponent]
    public class AnalyzerScopeRequirement {
        private readonly HeisenbugAnnotationCache annotationCache;

        public AnalyzerScopeRequirement(HeisenbugAnnotationCache annotationCache) {
            this.annotationCache = annotationCache;
        }

        public bool MustBeThreadSafe(ICSharpTreeNode node) {
            var typeNode = node.GetContainingTypeDeclaration();
            if (typeNode == null)
                return false;

            return this.annotationCache.GetThreadSafetyLevel(typeNode.DeclaredElement) != ThreadSafetyLevel.None;
        }

        public bool MustBeReadOnly(ICSharpTreeNode node) {
            var typeNode = node.GetContainingTypeDeclaration();
            if (typeNode == null)
                return false;

            return this.annotationCache.IsReadOnly(typeNode.DeclaredElement);
        }
    }
}