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
    public class ThreadSafeFieldAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeFieldAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (!_preconditions.MustBeThreadSafe(field))
                return;
            
            if (!field.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInThreadSafeType(field, field.DeclaredName));

            TypeUsageTreeValidator.Validate(
                field.TypeUsage.NotNull(),
                field.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsInstanceAccessThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, field.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
