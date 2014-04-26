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
        [NotNull] private static readonly Func<ExternalAnnotationAttributeInstance, int, string> GetPositionalAttributeValue = CompileGetPositionalAttributeValue();

        [NotNull] private readonly CodeAnnotationsCache _annotationCache;
        [NotNull] private readonly ConcurrentDictionary<IAttributesOwner, HeisenbugAnnotations> _heisenbugCache = new ConcurrentDictionary<IAttributesOwner, HeisenbugAnnotations>();

        public HeisenbugAnnotationCache([NotNull] CodeAnnotationsCache annotationCache) {
            _annotationCache = annotationCache;
        }

        [NotNull]
        public HeisenbugAnnotations GetAnnotations([NotNull] IAttributesOwner member) {
            Argument.NotNull("member", member);

            // ReSharper disable once AssignNullToNotNullAttribute
            return _heisenbugCache.GetOrAdd(member, GetAnnotationsUncached);
        }

        [NotNull]
        private HeisenbugAnnotations GetAnnotationsUncached([NotNull] IAttributesOwner member) {
            var attributes = member.GetAttributeInstances(true);
            Assume.NotNull(attributes, "attributes");

            return new HeisenbugAnnotations(
                IsReadOnlyUncached(member, attributes),
                GetThreadSafetyUncached(member, attributes)
            );
        }

        private bool IsReadOnlyUncached([NotNull] IClrDeclaredElement member, [NotNull] IList<IAttributeInstance> attributes) {
            return HasAnnotationAttribute(attributes, "ReadOnlyAttribute")
                // ReSharper disable once PossibleNullReferenceException
                || GetValueFromParentOrDefault(member, a => a.IsReadOnly);
        }

        private ThreadSafety GetThreadSafetyUncached([NotNull] IClrDeclaredElement member, [NotNull] IList<IAttributeInstance> attributes) {
            if (HasAnnotationAttribute(attributes, "ThreadSafeAttribute"))
                return ThreadSafety.All;

            return GetThreadSafetyFromGenerated(attributes)
                // ReSharper disable once PossibleNullReferenceException
                ?? GetValueFromParentOrDefault(member, a => a.ThreadSafety);
        }

        private bool HasAnnotationAttribute([NotNull] IEnumerable<IAttributeInstance> attributes, [NotNull] string name) {
            return attributes.Any(a => _annotationCache.IsAnnotationAttribute(a, name));
        }

        private ThreadSafety? GetThreadSafetyFromGenerated([NotNull] IEnumerable<IAttributeInstance> attributes) {
            var generatedFullName = typeof(GeneratedThreadSafeAttribute).FullName;
            // ReSharper disable once PossibleNullReferenceException
            var generated = attributes.SingleOrDefault(a => a.GetClrName().FullName == generatedFullName);
            if (generated == null)
                return null;

            var valueString = GetPositionalAttributeValue(((ExternalAnnotationAttributeInstance)generated), 0);
            // ReSharper disable once AssignNullToNotNullAttribute
            return (ThreadSafety)Enum.Parse(typeof(ThreadSafety), valueString);
        }

        private T GetValueFromParentOrDefault<T>([NotNull] IClrDeclaredElement member, [NotNull] Func<HeisenbugAnnotations, T> getValue) {
            if (member is ITypeElement)
                return default(T);

            var containing = member.GetContainingType();
            if (containing != null)
                return getValue(GetAnnotations(containing));

            return default(T);
        }

        protected override void InvalidateOnPhysicalChange() {
            base.InvalidateOnPhysicalChange();
            _heisenbugCache.Clear();
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
