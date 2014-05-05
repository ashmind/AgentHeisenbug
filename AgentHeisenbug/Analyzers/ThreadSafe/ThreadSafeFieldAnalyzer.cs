using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using AgentHeisenbug.Analyzers.Helpers;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInThreadSafeType),
        typeof(FieldOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeFieldAnalyzer : IElementProblemAnalyzer {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly ReferencedTypeHelper _referenceHelper;

        public ThreadSafeFieldAnalyzer([NotNull] AnalyzerPreconditions preconditions, [NotNull] ReferencedTypeHelper referenceHelper) {
            _preconditions = preconditions;
            _referenceHelper = referenceHelper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (!_preconditions.MustBeThreadSafe(field))
                return;
            
            if (!field.IsReadonly)
                consumer.AddHighlighting(new MutableFieldInThreadSafeType(field, field.DeclaredName));

            _referenceHelper.ValidateTypeUsageTree(
                field.TypeUsage.NotNull(),
                field.Type.NotNull(),
                _preconditions.MustBeThreadSafe,
                _referenceHelper.IsInstanceThreadSafeOrReadOnly,

                (type, usage) => consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
                    // ReSharper disable AssignNullToNotNullAttribute
                    usage, field.DeclaredName, type.GetCSharpPresentableName()
                    // ReSharper enable AssignNullToNotNullAttribute
                ))
            );
        }
    }
}
