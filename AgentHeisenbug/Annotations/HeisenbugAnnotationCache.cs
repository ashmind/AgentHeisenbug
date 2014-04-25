using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Impl.Reflection2.ExternalAnnotations;
using AgentHeisenbug.Annotations.Generated;

namespace AgentHeisenbug.Annotations {
    [PsiComponent]
    public class HeisenbugAnnotationCache : InvalidatingPsiCache {
        [NotNull] private static readonly string GeneratedAttributeNamespace = typeof(GeneratedThreadSafeAttribute).Namespace;
        [NotNull] private static readonly Func<ExternalAnnotationAttributeInstance, int, string> GetPositionalAttributeValue = CompileGetPositionalAttributeValue();

        [NotNull] private readonly CodeAnnotationsCache _annotationCache;
        [NotNull] private readonly ConcurrentDictionary<IAttributesOwner, ThreadSafety> _threadSafeCache = new ConcurrentDictionary<IAttributesOwner, ThreadSafety>();
        [NotNull] private readonly ConcurrentDictionary<IAttributesOwner, bool> _readOnlyCache = new ConcurrentDictionary<IAttributesOwner, bool>();

        public HeisenbugAnnotationCache([NotNull] CodeAnnotationsCache annotationCache) {
            _annotationCache = annotationCache;
        }

        public ThreadSafety GetThreadSafety([NotNull] IAttributesOwner member) {
            Argument.NotNull("member", member);
            return _threadSafeCache.GetOrAdd(member, m => GetValueUncached(
                // ReSharper disable AssignNullToNotNullAttribute
                m, "ThreadSafeAttribute",
                ThreadSafety.All,
                generated => (ThreadSafety)Enum.Parse(typeof(ThreadSafety), GetPositionalAttributeValue((ExternalAnnotationAttributeInstance)generated, 0)),
                GetThreadSafety
                // ReSharper restore AssignNullToNotNullAttribute
            ));
        }
        
        public bool IsReadOnly([NotNull] IAttributesOwner member) {
            Argument.NotNull("member", member);
            return _readOnlyCache.GetOrAdd(member, m => GetValueUncached(
                // ReSharper disable once AssignNullToNotNullAttribute
                m, "ReadOnlyAttribute", true, _ => true, IsReadOnly
            ));
        }

        private T GetValueUncached<T>([NotNull] IAttributesOwner member, string attributeShortName, T valueIfManual, [NotNull] Func<IAttributeInstance, T> valueIfGenerated, [NotNull] Func<IAttributesOwner, T> getValueCached) {
            var attributes = member.GetAttributeInstances(true);
            Assume.NotNull("attributes", attributes);

            var manual = attributes.Any(a => this._annotationCache.IsAnnotationAttribute(a, attributeShortName));
            if (manual)
                return valueIfManual;

            var generatedFullName = GeneratedAttributeNamespace + ".Generated" + attributeShortName;
            // ReSharper disable once PossibleNullReferenceException
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
            this._threadSafeCache.Clear();
            this._readOnlyCache.Clear();
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
