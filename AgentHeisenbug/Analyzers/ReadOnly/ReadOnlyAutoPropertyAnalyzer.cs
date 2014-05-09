using System.Linq;
using AgentHeisenbug.Annotations;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInReadOnlyType),
        typeof(AutoPropertyOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyAutoPropertyAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ReadOnlyAutoPropertyAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var property = (IPropertyDeclaration)element;
            if (!property.IsAuto || !_preconditions.MustBeReadOnly(property))
                return;

            var setter = property.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInReadOnlyType(setter.NameIdentifier.NotNull(), property.DeclaredName));

            TypeUsageTreeValidator.Validate(
                property.TypeUsage.NotNull(),
                property.Type.NotNull(),
                _preconditions.MustBeReadOnly,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsReadOnly,

                (type, usage) => consumer.AddHighlighting(new AutoPropertyOfNonReadOnlyTypeInReadOnlyType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, property.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}

