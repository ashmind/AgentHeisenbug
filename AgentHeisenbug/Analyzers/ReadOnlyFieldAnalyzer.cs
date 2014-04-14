﻿using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldInReadOnlyType) })]
    public class ReadOnlyFieldAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;
        private readonly ReferencedTypeHelper referenceHelper;

        public ReadOnlyFieldAnalyzer(AnalyzerPreconditions preconditions, ReferencedTypeHelper referenceHelper) {
            this.preconditions = preconditions;
            this.referenceHelper = referenceHelper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (!this.preconditions.MustBeReadOnly(field))
                return;

            if (!field.IsReadonly) {
                consumer.AddHighlighting(new MutableFieldInReadOnlyType(field, "Field '{0}' in a [ReadOnly] type should not be mutable.", field.DeclaredName));
                return;
            }

            if (!this.referenceHelper.IsReadOnly(field.Type)) {
                consumer.AddHighlighting(new FieldOfMutableTypeInReadOnlyType(
                    field.TypeUsage,
                    "Type '{0}' of field '{1}' in a [ReadOnly] type should not be mutable.",
                    field.Type.GetPresentableName(CSharpLanguage.Instance), field.DeclaredName)
                );
                return;
            }
        }
    }
}