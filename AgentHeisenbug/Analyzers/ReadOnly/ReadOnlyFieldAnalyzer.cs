using System.Linq;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.TypeUsageTree;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInReadOnlyType),
        typeof(FieldOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyFieldAnalyzer : ElementProblemAnalyzer<IFieldDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly ReadOnlyTypeUsageValidator _typeUsageValidator;

        public ReadOnlyFieldAnalyzer(
            [NotNull] AnalyzerPreconditions preconditions,
            [NotNull] ReadOnlyTypeUsageValidator typeUsageValidator
        ) {
            _preconditions = preconditions;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeReadOnly(element))
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
