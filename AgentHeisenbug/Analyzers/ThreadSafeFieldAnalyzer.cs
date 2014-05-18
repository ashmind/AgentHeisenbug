using System.Linq;
using AgentHeisenbug.Highlightings;
using AgentHeisenbug.Highlightings.ThreadSafe;
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
        typeof(MutableFieldInThreadSafeType),
        typeof(FieldOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeFieldAnalyzer : ElementProblemAnalyzer<IFieldDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<ThreadSafe> _precondition;
        [NotNull] private readonly ITypeUsageTreeValidator<InstanceThreadSafe> _typeUsageValidator;

        public ThreadSafeFieldAnalyzer(
            [NotNull] IAnalyzerPrecondition<ThreadSafe> precondition,
            [NotNull] ITypeUsageTreeValidator<InstanceThreadSafe> typeUsageValidator
        ) {
            _precondition = precondition;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_precondition.Applies(element))
                return;
            
            if (!element.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInThreadSafeType(element, element.DeclaredName));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(element, invalid.Usage, invalid.Type));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
