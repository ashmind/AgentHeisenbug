using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInThreadSafeType),
        typeof(FieldOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeFieldAnalyzer : ElementProblemAnalyzer<IFieldDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeFieldAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeThreadSafe(element))
                return;
            
            if (!element.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInThreadSafeType(element, element.DeclaredName));

            TypeUsageTreeValidator.Validate(
                element.TypeUsage.NotNull(),
                element.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsInstanceAccessThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, element.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
