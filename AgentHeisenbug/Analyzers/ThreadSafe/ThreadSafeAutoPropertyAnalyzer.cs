using System.Linq;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.TypeUsageTree;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Highlightings;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInThreadSafeType),
        typeof(AutoPropertyOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly ThreadSafeTypeUsageValidator _typeUsageValidator;

        public ThreadSafeAutoPropertyAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] ThreadSafeTypeUsageValidator typeUsageValidator) {
            _preconditions = preconditions;
            _typeUsageValidator = typeUsageValidator;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_preconditions.MustBeThreadSafe(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInThreadSafeType(setter.NameIdentifier.NotNull(), element.DeclaredName));

            foreach (var invalid in _typeUsageValidator.GetAllInvalid(element.Type.NotNull(), element.TypeUsage.NotNull())) {
                // ReSharper disable AssignNullToNotNullAttribute
                consumer.AddHighlighting(new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(
                    invalid.Usage, element.DeclaredName, invalid.Type.GetCSharpPresentableName()
                ));
                // ReSharper enable AssignNullToNotNullAttribute
            }
        }
    }
}
