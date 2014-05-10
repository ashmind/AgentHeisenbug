using System.Linq;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using AgentHeisenbug.Processing.TypeUsageTree;
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
        [NotNull] private readonly TypeUsageTreeValidator<ReadOnly> _typeUsageValidator;

        public ReadOnlyAutoPropertyAnalyzer([NotNull] IAnalyzerPrecondition<ReadOnly> precondition, [NotNull] TypeUsageTreeValidator<ReadOnly> typeUsageValidator) {
            _precondition = precondition;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_precondition.Applies(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInReadOnlyType(setter.NameIdentifier.NotNull(), element.DeclaredName));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new AutoPropertyOfNonReadOnlyTypeInReadOnlyType(
                    invalid.Usage, element.DeclaredName, invalid.Type.GetCSharpPresentableName()
                ));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}

