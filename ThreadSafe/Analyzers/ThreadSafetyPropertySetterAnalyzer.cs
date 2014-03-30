using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Highlightings;

namespace ThreadSafety.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAccessorDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyNotThreadSafe) })]
    public class ThreadSafetyPropertySetterAnalyzer : IElementProblemAnalyzer {
        private readonly ThreadSafetyAnalyzerHelper helper;

        public ThreadSafetyPropertySetterAnalyzer(ThreadSafetyAnalyzerHelper helper) {
            this.helper = helper;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var accessor = (IAccessorDeclaration)element;
            if (accessor.Kind != AccessorKind.SETTER || !this.helper.MustBeThreadSafe(accessor))
                return;

            var accessRights = accessor.GetAccessRights();
            if (accessRights == AccessRights.PRIVATE || accessRights == AccessRights.PROTECTED || accessRights == AccessRights.PROTECTED_AND_INTERNAL)
                return;

            var property = accessor.Parent as IPropertyDeclaration;
            if (property == null)
                return;

            consumer.AddHighlighting(new MutableFieldOrPropertyNotThreadSafe(
                accessor, "Property '{0}' in a [ThreadSafe] class should not have setter.", property.DeclaredName
            ));
        }
    }
}
