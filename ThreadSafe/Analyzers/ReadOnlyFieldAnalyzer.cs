using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Highlightings;

namespace ThreadSafety.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldInReadOnlyType) })]
    public class ReadOnlyFieldAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerScopeRequirement requirement;

        public ReadOnlyFieldAnalyzer(AnalyzerScopeRequirement requirement) {
            this.requirement = requirement;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (field.IsReadonly || !this.requirement.MustBeReadOnly(field))
                return;

            consumer.AddHighlighting(new MutableFieldInReadOnlyType(
                field, "Field '{0}' in a [ReadOnly] type should not be mutable.", field.DeclaredName
            ));
        }
    }
}
