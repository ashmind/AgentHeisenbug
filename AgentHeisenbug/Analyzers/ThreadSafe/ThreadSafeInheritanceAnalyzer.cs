using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Highlightings;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IClassLikeDeclaration) }, HighlightingTypes = new[] {
        typeof(NonThreadSafeBaseClassInThreadSafeClass),
        typeof(ThreadSafeBaseClassInNonThreadSafeClass),
        typeof(ThreadSafeInterfaceInNonThreadSafeType)
    })]
    public class ThreadSafeInheritanceAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeInheritanceAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IClassLikeDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (element.DeclaredElement == null)
                return;

            var superTypes = element.SuperTypes;
            if (superTypes == null)
                return;
            
            var mustBeThreadSafe = _preconditions.MustBeThreadSafe(element);
            var index = -1;
            foreach (var superType in superTypes) {
                index += 1;
                if (superType == null)
                    continue;
                
                var highlightning = Highlight(element, mustBeThreadSafe, superType, element.SuperTypeUsageNodes[index].NotNull(), consumer);
                if (highlightning == null)
                    continue;

                consumer.AddHighlighting(highlightning);
            }
        }

        private IHighlighting Highlight([NotNull] IClassLikeDeclaration element, bool mustBeThreadSafe, [NotNull] IDeclaredType superType, [NotNull] IDeclaredTypeUsage superTypeUsage, IHighlightingConsumer consumer) {
            var superTypeElement = superType.GetTypeElement();
            if (superTypeElement == null)
                return null;

            var superTypeThreadSafe = _featureProvider.GetFeatures(superTypeElement).DeclaredThreadSafety.Has(ThreadSafety.Instance);
            var typeName = element.DeclaredElement.NotNull().ShortName;
            if (mustBeThreadSafe) {
                if (!superTypeThreadSafe)
                    return new NonThreadSafeBaseClassInThreadSafeClass(superTypeUsage, superType.GetCSharpPresentableName(), typeName);

                return null;
            }

            if (ShouldNotConsiderAnnotatingFor(superTypeElement))
                return null;

            if (superType.IsInterfaceType())
                return new ThreadSafeInterfaceInNonThreadSafeType(superTypeUsage, superType.GetCSharpPresentableName(), typeName);

            return new ThreadSafeBaseClassInNonThreadSafeClass(superTypeUsage, superType.GetCSharpPresentableName(), typeName);
        }

        private bool ShouldNotConsiderAnnotatingFor([NotNull] ITypeElement superTypeElement) {
            return superTypeElement.IsAttribute();
        }
    }
}
