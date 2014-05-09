using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Annotations;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.Util;

namespace AgentHeisenbug.Processing {
    [PsiComponent]
    public class HeisenbugFeatureProvider : InvalidatingPsiCache {
        private static class FeatureInstances {
            [NotNull] public static readonly HeisenbugFeatures None = new HeisenbugFeatures(false, false, ThreadSafety.None);
            [NotNull] public static readonly HeisenbugFeatures TriviallySafe = new HeisenbugFeatures(true, true, ThreadSafety.All);
        }

        [NotNull] private readonly ConcurrentDictionary<IClrDeclaredElement, HeisenbugFeatures> _cache = new ConcurrentDictionary<IClrDeclaredElement, HeisenbugFeatures>();
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public HeisenbugFeatureProvider([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        [NotNull]
        public HeisenbugFeatures GetFeatures([NotNull] IType type) {
            var declaredType = type as IDeclaredType;
            if (declaredType == null) // arrays, pointers => all not threadsafe or readonly
                return FeatureInstances.None;

            var typeElement = declaredType.GetTypeElement();
            if (typeElement == null)
                return FeatureInstances.None;

            return GetFeatures(typeElement);
        }
        
        [NotNull]
        public HeisenbugFeatures GetFeatures([NotNull] IClrDeclaredElement member) {
            Argument.NotNull("member", member);
            return _cache.GetOrAdd(member, GetFeaturesUncached).NotNull();
        }

        [NotNull]
        private HeisenbugFeatures GetFeaturesUncached([NotNull] IClrDeclaredElement element) {
            var type = element as ITypeElement;
            if (type != null && IsTriviallySafe(type))
                return FeatureInstances.TriviallySafe;

            var features = _annotationCache.GetFeaturesFromAnnotations((IAttributesOwner)element);
            if (type != null) {
                if (IsTriviallyReadOnly(type))
                    features = features.WithReadOnly(true);

                return features;
            }

            var containingType = element.GetContainingType();
            if (containingType == null)
                return features;

            var parentFeatures = GetFeatures(containingType);
            return new HeisenbugFeatures(features, parentFeatures);
        }

        private bool IsTriviallySafe([NotNull] ITypeElement type) {
            return (type is IDelegate) || (type is IEnum);
        }

        private bool IsTriviallyReadOnly(ITypeElement type) {
            return type is IStruct;
        }

        protected override void InvalidateOnPhysicalChange() {
            base.InvalidateOnPhysicalChange();
            _cache.Clear();
        }
    }
}
