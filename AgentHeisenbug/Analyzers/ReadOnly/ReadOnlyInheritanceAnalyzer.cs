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

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IClassLikeDeclaration) }, HighlightingTypes = new[] {
        typeof(ReadOnlyClassInheritedByNonReadOnlyType),
        typeof(ReadOnlyInterfaceImplementedByNonReadOnlyType)
    })]
    public class ReadOnlyInheritanceAnalyzer : InheritanceAnalyzerBase {
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public ReadOnlyInheritanceAnalyzer([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        protected override bool IsAnnotated(ITypeElement type) {
            return _annotationCache.GetAnnotations(type).IsReadOnly;
        }

        protected override IHighlighting NewInterfaceImplementedByNonAnnotatedType(IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName) {
            return new ReadOnlyInterfaceImplementedByNonReadOnlyType(superTypeUsage, superTypeName, typeName);
        }

        protected override IHighlighting NewClassInheritedByNonAnnotatedType(IDeclaredTypeUsage superTypeUsage, string superTypeName, string typeName) {
            return new ReadOnlyClassInheritedByNonReadOnlyType(superTypeUsage, superTypeName, typeName);
        }
    }
}
