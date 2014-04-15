using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] {
        typeof(MutableAutoPropertyInThreadSafeType),
        typeof(AutoPropertyOfNonThreadSafeTypeInThreadSafeType)
    })]
    public class ThreadSafeAutoPropertyAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;
        private readonly ReferencedTypeHelper referenceHelper;

        public ThreadSafeAutoPropertyAnalyzer(AnalyzerPreconditions preconditions, ReferencedTypeHelper referenceHelper) {
            this.preconditions = preconditions;
            this.referenceHelper = referenceHelper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var property = (IPropertyDeclaration)element;
            if (!property.IsAuto || !this.preconditions.MustBeThreadSafe(property))
                return;

            var setter = property.AccessorDeclarations.SingleOrDefault(a => a.Kind == AccessorKind.SETTER);
            if (setter == null)
                return;

            var accessRights = setter.GetAccessRights();
            if (accessRights != AccessRights.PRIVATE) {
                consumer.AddHighlighting(new MutableAutoPropertyInThreadSafeType(setter.NameIdentifier, property.DeclaredName));
                return;
            }

            if (!this.referenceHelper.GetThreadSafety(property.Type).Instance && !this.referenceHelper.IsReadOnly(property.Type)) {
                consumer.AddHighlighting(new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(
                    property.TypeUsage, property.DeclaredName, property.Type.GetPresentableName(CSharpLanguage.Instance)
                ));
            }
        }
    }
}
