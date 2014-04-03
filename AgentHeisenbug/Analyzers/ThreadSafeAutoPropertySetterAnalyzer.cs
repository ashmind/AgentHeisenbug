using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyInThreadSafeType) })]
    public class ThreadSafeAutoPropertySetterAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerPreconditions preconditions;

        public ThreadSafeAutoPropertySetterAnalyzer(AnalyzerPreconditions preconditions) {
            this.preconditions = preconditions;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var property = (IPropertyDeclaration)element;
            if (!property.IsAuto || !this.preconditions.MustBeThreadSafe(property))
                return;

            var setter = property.AccessorDeclarations.SingleOrDefault(a => a.Kind == AccessorKind.SETTER);
            if (setter == null)
                return;

            var accessRights = setter.GetAccessRights();
            if (accessRights == AccessRights.PRIVATE || accessRights == AccessRights.PROTECTED || accessRights == AccessRights.PROTECTED_AND_INTERNAL)
                return;

            consumer.AddHighlighting(new MutableFieldOrPropertyInThreadSafeType(
                setter.NameIdentifier, "Property '{0}' in a [ThreadSafe] class should not have setter.", property.DeclaredName
            ));
        }
    }
}
