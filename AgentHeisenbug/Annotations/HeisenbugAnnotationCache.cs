using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.Util.dataStructures;

namespace AgentHeisenbug.Annotations {
    [PsiComponent]
    public class HeisenbugAnnotationCache : InvalidatingPsiCache {
        private readonly CodeAnnotationsCache annotationCache;
        private readonly ConcurrentDictionary<IAttributesOwner, ThreadSafetyLevel> threadSafeCache = new ConcurrentDictionary<IAttributesOwner, ThreadSafetyLevel>();
        private readonly ConcurrentDictionary<IAttributesOwner, bool> readOnlyCache = new ConcurrentDictionary<IAttributesOwner, bool>();

        public HeisenbugAnnotationCache(CodeAnnotationsCache annotationCache) {
            this.annotationCache = annotationCache;
        }

        public ThreadSafetyLevel GetThreadSafetyLevel(IAttributesOwner member) {
            return this.threadSafeCache.GetOrAdd(member, m => GetValueUncached(
                m, "ThreadSafeAttribute",
                ThreadSafetyLevel.All,
                generated => (ThreadSafetyLevel)Enum.Parse(typeof(ThreadSafetyLevel), (string)generated.PositionParameter(0).ConstantValue.Value),
                GetThreadSafetyLevel
            ));
        }
        
        public bool IsReadOnly(IAttributesOwner member) {
            return this.readOnlyCache.GetOrAdd(member, m => GetValueUncached(
                m, "ReadOnlyAttribute", true, _ => true, IsReadOnly
            ));
        }

        private T GetValueUncached<T>(IAttributesOwner member, string attributeShortName, T valueIfManual, Func<IAttributeInstance, T> valueIfGenerated, Func<IAttributesOwner, T> getValueCached) {
            var attributes = member.GetAttributeInstances(true);
            var manual = attributes.Any(a => this.annotationCache.IsAnnotationAttribute(a, attributeShortName));
            if (manual)
                return valueIfManual;

            var generated = attributes.SingleOrDefault(a => this.annotationCache.IsAnnotationAttribute(a, "Generated" + attributeShortName));
            if (generated != null)
                return valueIfGenerated(generated);

            if (member is ITypeElement)
                return default(T);

            var containing = member.GetContainingType();
            if (containing != null)
                return getValueCached(containing);

            return default(T);
        }

        protected override void InvalidateOnPhysicalChange() {
            base.InvalidateOnPhysicalChange();
            this.threadSafeCache.Clear();
            this.readOnlyCache.Clear();
        }
    }
}
