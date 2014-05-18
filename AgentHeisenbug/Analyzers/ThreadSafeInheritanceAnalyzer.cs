using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using AgentHeisenbug.Highlightings.ThreadSafe;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IClassLikeDeclaration) }, HighlightingTypes = new[] {
        typeof(NonThreadSafeBaseClassInThreadSafeClass),
        typeof(ThreadSafeBaseClassInNonThreadSafeClass),
        typeof(ThreadSafeInterfaceInNonThreadSafeType)
    })]
    public class ThreadSafeInheritanceAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<ThreadSafe> _precondition;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeInheritanceAnalyzer([NotNull] IAnalyzerPrecondition<ThreadSafe> precondition, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _precondition = precondition;
            _featureProvider = featureProvider;
        }

        protected override void Run(IClassLikeDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (element.DeclaredElement == null)
                return;

            var superTypes = element.SuperTypes;
            if (superTypes == null)
                return;
            
            var mustBeThreadSafe = _precondition.Applies(element);
            var index = -1;
            foreach (var superType in superTypes) {
                index += 1;
                if (superType == null)
                    continue;
                
                var highlightning = Highlight(element, mustBeThreadSafe, superType, element.SuperTypeUsageNodes[index].NotNull());
                if (highlightning == null)
                    continue;

                consumer.AddHighlighting(highlightning);
            }
        }

        private IHighlighting Highlight([NotNull] IClassLikeDeclaration element, bool mustBeThreadSafe, [NotNull] IDeclaredType superType, [NotNull] IDeclaredTypeUsage superTypeUsage) {
            var superTypeElement = superType.GetTypeElement();
            if (superTypeElement == null)
                return null;

            var superTypeThreadSafe = _featureProvider.GetFeatures(superTypeElement).DeclaredThreadSafety.Has(ThreadSafety.Instance);
            if (superTypeThreadSafe == mustBeThreadSafe)
                return null;

            if (!superTypeThreadSafe) {
                if (superType.IsInterfaceType())
                    return null;
                
                return new NonThreadSafeBaseClassInThreadSafeClass(element, superTypeUsage, superType);
            }

            if (ShouldNotConsiderAnnotatingFor(superTypeElement))
                return null;

            if (superType.IsInterfaceType())
                return new ThreadSafeInterfaceInNonThreadSafeType(element, superTypeUsage, superType);

            return new ThreadSafeBaseClassInNonThreadSafeClass(element, superTypeUsage, superType);
        }

        private bool ShouldNotConsiderAnnotatingFor([NotNull] ITypeElement superTypeElement) {
            return superTypeElement.IsAttribute();
        }
    }
}
