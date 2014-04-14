using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AgentHeisenbug.Annotations.Generated;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Impl.Reflection2.ExternalAnnotations;

namespace AgentHeisenbug.Annotations {
    [PsiComponent]
    public class HeisenbugAnnotationCache : InvalidatingPsiCache {
        private static readonly string GeneratedAttributeNamespace = typeof(GeneratedThreadSafeAttribute).Namespace;
        private static readonly Func<ExternalAnnotationAttributeInstance, int, string> GetPositionalAttributeValue = CompileGetPositionalAttributeValue();

        private readonly CodeAnnotationsCache annotationCache;
        private readonly ConcurrentDictionary<IAttributesOwner, ThreadSafety> threadSafeCache = new ConcurrentDictionary<IAttributesOwner, ThreadSafety>();
        private readonly ConcurrentDictionary<IAttributesOwner, bool> readOnlyCache = new ConcurrentDictionary<IAttributesOwner, bool>();

        public HeisenbugAnnotationCache(CodeAnnotationsCache annotationCache) {
            this.annotationCache = annotationCache;
        }

        public ThreadSafety GetThreadSafety(IAttributesOwner member) {
            return this.threadSafeCache.GetOrAdd(member, m => GetValueUncached(
                m, "ThreadSafeAttribute",
                ThreadSafety.Values.All,
                generated => ThreadSafety.Parse(GetPositionalAttributeValue((ExternalAnnotationAttributeInstance)generated, 0)),
                GetThreadSafety
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

            var generatedFullName = GeneratedAttributeNamespace + ".Generated" + attributeShortName;
            var generated = attributes.SingleOrDefault(a => a.GetClrName().FullName == generatedFullName);
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
        
        private static Func<ExternalAnnotationAttributeInstance, int, string> CompileGetPositionalAttributeValue() {
            var attribute = Expression.Parameter(typeof(ExternalAnnotationAttributeInstance), "attribute");
            var index = Expression.Parameter(typeof(int), "index");

            var arguments = Expression.Field(attribute, "myPositionalArguments");
            var argument = Expression.Call(arguments, "get_Item", null, index);
            var value = Expression.Convert(Expression.Property(argument, "Value"), typeof(string));

            return Expression.Lambda<Func<ExternalAnnotationAttributeInstance, int, string>>(value, attribute, index)
                             .Compile();
        }
    }
}
