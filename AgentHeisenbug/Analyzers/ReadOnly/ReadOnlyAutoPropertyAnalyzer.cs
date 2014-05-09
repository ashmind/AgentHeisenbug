using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInReadOnlyType),
        typeof(AutoPropertyOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyAutoPropertyAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ReadOnlyAutoPropertyAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_preconditions.MustBeReadOnly(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInReadOnlyType(setter.NameIdentifier.NotNull(), element.DeclaredName));

            TypeUsageTreeValidator.Validate(
                element.TypeUsage.NotNull(),
                element.Type.NotNull(),
                _preconditions.MustBeReadOnly,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsReadOnly,

                (type, usage) => consumer.AddHighlighting(new AutoPropertyOfNonReadOnlyTypeInReadOnlyType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, element.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}

