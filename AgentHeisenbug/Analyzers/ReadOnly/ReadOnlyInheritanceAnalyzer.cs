using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Highlightings;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IClassLikeDeclaration) }, HighlightingTypes = new[] {
        typeof(NonReadOnlyBaseClassInReadOnlyClass)
    })]
    public class ReadOnlyInheritanceAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ReadOnlyInheritanceAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IClassLikeDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeReadOnly(element))
                return;

            var superTypes = element.SuperTypes;
            if (superTypes == null)
                return;
            
            var index = -1;
            foreach (var superType in superTypes) {
                index += 1;
                if (superType == null)
                    continue;

                if (_featureProvider.GetFeatures(superType).IsReadOnly)
                    continue;
                
                consumer.AddHighlighting(
                    new NonReadOnlyBaseClassInReadOnlyClass(element.SuperTypeUsageNodes[index].NotNull(), superType.GetCSharpPresentableName(), element.DeclaredName)
                );
            }
        }
    }
}
