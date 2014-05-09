using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Analyzers.Base;
using AgentHeisenbug.Annotations;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IClassLikeDeclaration) }, HighlightingTypes = new[] {
        typeof(ThreadSafeClassInheritedByNonThreadSafeType),
        typeof(ThreadSafeInterfaceImplementedByNonThreadSafeType)
    })]
    public class ThreadSafeInheritanceAnalyzer : InheritanceAnalyzerBase {
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public ThreadSafeInheritanceAnalyzer([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        protected override bool IsAnnotated(ITypeElement type) {
            return _annotationCache.GetFeaturesFromAnnotations(type).DeclaredThreadSafety != ThreadSafety.None;
        }

        protected override IHighlighting NewInterfaceImplementedByNonAnnotatedType(IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName) {
            return new ThreadSafeInterfaceImplementedByNonThreadSafeType(superTypeUsage, superTypeName, typeName);
        }

        protected override IHighlighting NewClassInheritedByNonAnnotatedType(IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName) {
            return new ThreadSafeClassInheritedByNonThreadSafeType(superTypeUsage, superTypeName, typeName);
        }
    }
}
