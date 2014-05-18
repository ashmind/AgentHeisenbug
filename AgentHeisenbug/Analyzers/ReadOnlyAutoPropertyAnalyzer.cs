using System.Linq;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Highlightings.ReadOnly;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInReadOnlyType),
        typeof(AutoPropertyOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyAutoPropertyAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<ReadOnly> _precondition;
        [NotNull] private readonly ITypeUsageTreeValidator<ReadOnly> _typeUsageValidator;

        public ReadOnlyAutoPropertyAnalyzer([NotNull] IAnalyzerPrecondition<ReadOnly> precondition, [NotNull] ITypeUsageTreeValidator<ReadOnly> typeUsageValidator) {
            _precondition = precondition;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_precondition.Applies(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInReadOnlyType(element, setter));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new AutoPropertyOfNonReadOnlyTypeInReadOnlyType(element, invalid.Usage, invalid.Type));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}

