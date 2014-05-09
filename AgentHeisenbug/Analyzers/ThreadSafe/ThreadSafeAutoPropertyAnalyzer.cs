using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInThreadSafeType),
        typeof(AutoPropertyOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeAutoPropertyAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var property = (IPropertyDeclaration)element;
            if (!property.IsAuto || !_preconditions.MustBeThreadSafe(property))
                return;

            var setter = property.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInThreadSafeType(setter.NameIdentifier.NotNull(), property.DeclaredName));

            TypeUsageTreeValidator.Validate(
                property.TypeUsage.NotNull(),
                property.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsInstanceAccessThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, property.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
