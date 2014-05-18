using System.Linq;
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
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInReadOnlyType),
        typeof(FieldOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyFieldAnalyzer : ElementProblemAnalyzer<IFieldDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<ReadOnly> _precondition;
        [NotNull] private readonly ITypeUsageTreeValidator<ReadOnly> _typeUsageValidator;

        public ReadOnlyFieldAnalyzer(
            [NotNull] IAnalyzerPrecondition<ReadOnly> precondition,
            [NotNull] ITypeUsageTreeValidator<ReadOnly> typeUsageValidator
        ) {
            _precondition = precondition;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_precondition.Applies(element))
                return;
            
            if (!element.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInReadOnlyType(element, element.DeclaredName));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new FieldOfNonReadOnlyTypeInReadOnlyType(
                    invalid.Usage, element.DeclaredName, invalid.Type.GetCSharpPresentableName()
                ));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
