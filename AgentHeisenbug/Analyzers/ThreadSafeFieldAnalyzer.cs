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
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyInThreadSafeType) })]
    public class ThreadSafeFieldAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;
        private readonly ReferencedTypeHelper referenceHelper;

        public ThreadSafeFieldAnalyzer(AnalyzerPreconditions preconditions, ReferencedTypeHelper referenceHelper) {
            this.preconditions = preconditions;
            this.referenceHelper = referenceHelper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var field = (IFieldDeclaration)element;
            if (!this.preconditions.MustBeThreadSafe(field))
                return;

            if (!field.IsReadonly) {
                consumer.AddHighlighting(new MutableFieldOrPropertyInThreadSafeType(field, "Field '{0}' in a [ThreadSafe] class should not be mutable.", field.DeclaredName));
                return;
            }

            if (!this.referenceHelper.GetThreadSafety(field.Type).Instance) {
                consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
                    field.TypeUsage,
                    "Type '{0}' of field '{1}' in a [ThreadSafe] type should be thread-safe.",
                    field.Type.GetPresentableName(CSharpLanguage.Instance), field.DeclaredName)
                );
            }
        }
    }
}
