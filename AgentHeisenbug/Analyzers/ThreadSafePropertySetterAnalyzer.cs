using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAccessorDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyInThreadSafeType) })]
    public class ThreadSafePropertySetterAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerScopeRequirement requirement;

        public ThreadSafePropertySetterAnalyzer(AnalyzerScopeRequirement requirement) {
            this.requirement = requirement;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var accessor = (IAccessorDeclaration)element;
            if (accessor.Kind != AccessorKind.SETTER || !this.requirement.MustBeThreadSafe(accessor))
                return;

            var accessRights = accessor.GetAccessRights();
            if (accessRights == AccessRights.PRIVATE || accessRights == AccessRights.PROTECTED || accessRights == AccessRights.PROTECTED_AND_INTERNAL)
                return;

            var property = accessor.Parent as IPropertyDeclaration;
            if (property == null)
                return;

            consumer.AddHighlighting(new MutableFieldOrPropertyInThreadSafeType(
                accessor, "Property '{0}' in a [ThreadSafe] class should not have setter.", property.DeclaredName
            ));
        }
    }
}
