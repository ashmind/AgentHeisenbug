using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Highlightings;

namespace ThreadSafety.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyNotThreadSafe) })]
    public class ThreadSafeFieldAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerScopeRequirement requirement;

        public ThreadSafeFieldAnalyzer(AnalyzerScopeRequirement requirement) {
            this.requirement = requirement;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (field.IsReadonly || !this.requirement.MustBeThreadSafe(field))
                return;

            consumer.AddHighlighting(new MutableFieldOrPropertyNotThreadSafe(
                field, "Field '{0}' in a [ThreadSafe] class should not be mutable.", field.DeclaredName
            ));
        }
    }
}
