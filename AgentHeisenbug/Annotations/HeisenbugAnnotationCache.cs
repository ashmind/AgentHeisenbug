using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Impl.Reflection2.ExternalAnnotations;
using JetBrains.Util;
using AgentHeisenbug.Annotations.Generated;

namespace AgentHeisenbug.Annotations {
    [PsiComponent]
    public class HeisenbugAnnotationCache : InvalidatingPsiCache {
        [NotNull] private static readonly Func<ExternalAnnotationAttributeInstance, int, string> GetPositionalAttributeValue = CompileGetPositionalAttributeValue();

        [NotNull] private readonly CodeAnnotationsCache _annotationCache;
        [NotNull] private readonly ConcurrentDictionary<IAttributesOwner, HeisenbugFeatures> _heisenbugCache = new ConcurrentDictionary<IAttributesOwner, HeisenbugFeatures>();

        public HeisenbugAnnotationCache([NotNull] CodeAnnotationsCache annotationCache) {
            _annotationCache = annotationCache;
        }

        [NotNull]
        public HeisenbugFeatures GetFeaturesFromAnnotations([NotNull] IAttributesOwner element) {
            Argument.NotNull("element", element);
            return _heisenbugCache.GetOrAdd(element, GetFeaturesFromAnnotationsUncached).NotNull();
        }

        [NotNull]
        private HeisenbugFeatures GetFeaturesFromAnnotationsUncached([NotNull] IAttributesOwner element) {
            var attributes = element.ReliablyGetAttributeInstances(false).NotNull();
            var member = element as ITypeMember;
            return new HeisenbugFeatures(
                HasAnnotationAttribute(attributes, "ReadOnlyAttribute"),
                member != null && _annotationCache.IsPure(member),
                GetThreadSafetyUncached(attributes)
            );
        }

        private ThreadSafety GetThreadSafetyUncached([NotNull] IList<IAttributeInstance> attributes) {
            if (HasAnnotationAttribute(attributes, "ThreadSafeAttribute"))
                return ThreadSafety.All;

            return GetThreadSafetyFromGenerated(attributes) ?? ThreadSafety.None;
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
            return (ThreadSafety)Enum.Parse(typeof(ThreadSafety), valueString.NotNull());
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
