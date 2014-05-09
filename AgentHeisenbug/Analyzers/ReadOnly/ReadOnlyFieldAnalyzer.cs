using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInReadOnlyType),
        typeof(FieldOfNonReadOnlyTypeInReadOnlyType)
    })]
    public class ReadOnlyFieldAnalyzer : ElementProblemAnalyzer<IFieldDeclaration> {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ReadOnlyFieldAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeReadOnly(element))
                return;

            if (!element.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInReadOnlyType(element, element.DeclaredName));
            
            TypeUsageTreeValidator.Validate(
                element.TypeUsage.NotNull(),
                element.Type.NotNull(),
                _preconditions.MustBeReadOnly,
                // ReSharper disable once AssignNullToNotNullAttribute
                t => _featureProvider.GetFeatures(t).IsReadOnly,

                (type, usage) => consumer.AddHighlighting(new FieldOfNonReadOnlyTypeInReadOnlyType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, element.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
