using System.Linq;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Annotations;
using ThreadSafety.Highlightings;
using ThreadSafetyTips;

namespace ThreadSafety.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IAccessorDeclaration) }, HighlightingTypes = new[] { typeof(ExposingNotThreadSafeType) })]
    public class ThreadSafetyTypeExposureAnalyzer : IElementProblemAnalyzer {
        private readonly AnalyzerScopeRequirement requirement;
        private readonly HeisenbugAnnotationCache annotationCache;

        public ThreadSafetyTypeExposureAnalyzer(AnalyzerScopeRequirement requirement, HeisenbugAnnotationCache annotationCache) {
            this.requirement = requirement;
            this.annotationCache = annotationCache;
        }

        public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            var accessor = (IAccessorDeclaration)element;
            if (accessor.Kind != AccessorKind.GETTER || !this.requirement.MustBeThreadSafe(accessor))
                return;

            var property = accessor.Parent as IPropertyDeclaration;
            if (property == null)
                return;
            
            var accessRights = accessor.GetAccessRights(); // private getters will be analyzed based on usage
            if (accessRights == AccessRights.PRIVATE || accessRights == AccessRights.PROTECTED || accessRights == AccessRights.PROTECTED_AND_INTERNAL)
                return;

            var propertyType = property.DeclaredElement.ReturnType.GetScalarType();
            if (propertyType == null)
                return;

            var propertyTypeElement = propertyType.GetTypeElement();
            if (propertyTypeElement == null)
                return;

            var safetyLevel = this.annotationCache.GetThreadSafetyLevel(propertyTypeElement);
            if (safetyLevel == ThreadSafetyLevel.Instance || safetyLevel == ThreadSafetyLevel.All)
                return;
            
            consumer.AddHighlighting(new ExposingNotThreadSafeType(
                property.TypeUsage, "Type '{0}' is not thread-safe and so should not be exposed by [ThreadSafe] type.", propertyType.GetPresentableName(CSharpLanguage.Instance)
            ));
        }
    }
}
