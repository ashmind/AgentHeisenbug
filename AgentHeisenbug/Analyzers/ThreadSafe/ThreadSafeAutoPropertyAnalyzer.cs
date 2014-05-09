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
    public class ThreadSafeAutoPropertyAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeAutoPropertyAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!element.IsAuto || !_preconditions.MustBeThreadSafe(element))
                return;

            var setter = element.GetSetter();
            if (setter != null && !setter.IsPrivate())
                consumer.AddHighlighting(new MutableAutoPropertyInThreadSafeType(setter.NameIdentifier.NotNull(), element.DeclaredName));

            TypeUsageTreeValidator.Validate(
                element.TypeUsage.NotNull(),
                element.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsInstanceAccessThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, element.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
