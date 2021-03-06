using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings.ThreadSafe;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInThreadSafeType),
        typeof(AutoPropertyOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration> {
        [NotNull] private readonly IAnalyzerPrecondition<ThreadSafe> _precondition;
        [NotNull] private readonly ITypeUsageTreeValidator<InstanceThreadSafe> _typeUsageValidator;

        public ThreadSafeAutoPropertyAnalyzer([NotNull] IAnalyzerPrecondition<ThreadSafe> precondition, [NotNull] ITypeUsageTreeValidator<InstanceThreadSafe> typeUsageValidator) {
            _precondition = precondition;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_precondition.Applies(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInThreadSafeType(element, setter));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(element, invalid.Usage, invalid.Type));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
