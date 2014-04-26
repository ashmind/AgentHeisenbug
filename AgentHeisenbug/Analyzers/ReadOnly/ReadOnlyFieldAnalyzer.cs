﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ReadOnly {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldInReadOnlyType) })]
    public class ReadOnlyFieldAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly ReferencedTypeHelper _referenceHelper;

        public ReadOnlyFieldAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] ReferencedTypeHelper referenceHelper) {
            _preconditions = preconditions;
            _referenceHelper = referenceHelper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (!_preconditions.MustBeReadOnly(field))
                return;

            if (!field.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInReadOnlyType(field, field.DeclaredName));

            if (!_referenceHelper.IsReadOnly(field.Type)) {
                consumer.AddHighlighting(new FieldOfNonReadOnlyTypeInReadOnlyType(
                    field.TypeUsage, field.DeclaredName, field.Type.GetCSharpPresentableName()
                ));
            }
        }
    }
}