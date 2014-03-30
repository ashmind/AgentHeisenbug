using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.Util.dataStructures;
using ThreadSafetyTips;

namespace ThreadSafety.Annotations {
    [PsiComponent]
    public class ThreadSafetyAnnotationCache : InvalidatingPsiCache {
        private readonly ConcurrentDictionary<IAttributesOwner, ThreadSafetyLevel> cache = new ConcurrentDictionary<IAttributesOwner, ThreadSafetyLevel>();
        private readonly ThreadSafetyExternalAnnotationProvider externalProvider;

        public ThreadSafetyAnnotationCache(ThreadSafetyExternalAnnotationProvider externalProvider) {
            this.externalProvider = externalProvider;
        }

        public ThreadSafetyLevel GetThreadSafetyLevel(IAttributesOwner member) {
            return this.cache.GetOrAdd(member, GetThreadSafetyLevelUncached);
        }

        private ThreadSafetyLevel GetThreadSafetyLevelUncached(IAttributesOwner member) {
            var annotated = member.GetAttributeInstances(true).Any(a => a.AttributeType.GetClrName().ShortName == "ThreadSafeAttribute");
            if (annotated)
                return ThreadSafetyLevel.All;

            var typeMember = member as ITypeMember;
            if (typeMember != null) {
                var external = this.externalProvider.GetThreadSafetyLevel(typeMember);
                if (external != ThreadSafetyLevel.None)
                    return external;
            }

            var containing = member.GetContainingType();
            if (containing != null)
                return GetThreadSafetyLevel(containing);

            return ThreadSafetyLevel.None;
        }

        protected override void InvalidateOnPhysicalChange() {
            base.InvalidateOnPhysicalChange();
            this.cache.Clear();
        }
    }
}
