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
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableFieldInThreadSafeType),
        typeof(FieldOfNonThreadSafeTypeInThreadSafeType)
    })]
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
                consumer.AddHighlighting(new MutableFieldInThreadSafeType(field, field.DeclaredName));
                return;
            }

            if (!this.referenceHelper.GetThreadSafety(field.Type).Instance && !this.referenceHelper.IsReadOnly(field.Type)) {
                consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
                    field.TypeUsage, field.DeclaredName, field.Type.GetPresentableName(CSharpLanguage.Instance)
                ));
            }
        }
    }
}
