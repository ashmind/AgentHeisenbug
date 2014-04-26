using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using AgentHeisenbug.Annotations;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Helpers {
    [PsiComponent]
    public class ReferencedTypeHelper {
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public ReferencedTypeHelper([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        private bool IsTriviallyImmutable([NotNull] IType type) {
            if (type.IsSimplePredefined() || type.IsValue() || type.IsDelegateType())
                return true;

            return false;
        }

        public bool IsReadOnly([NotNull] IType type) {
            Argument.NotNull("type", type);
            if (IsTriviallyImmutable(type))
                return true;

            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return false;

            var typeElement = scalarType.GetTypeElement();
            if (typeElement == null)
                return false;

            return _annotationCache.GetAnnotations(typeElement).IsReadOnly;
        }

        public bool IsInstanceThreadSafeOrReadOnly([NotNull] IType type) {
            return IsReadOnly(type)
                || GetThreadSafety(type).Has(ThreadSafety.Instance);
        }

        private ThreadSafety GetThreadSafety([NotNull] IType type) {
            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return ThreadSafety.None;

            var typeElement = scalarType.GetTypeElement();
            if (typeElement == null)
                return ThreadSafety.None;

            return _annotationCache.GetAnnotations(typeElement).ThreadSafety;
        }
    }
}
