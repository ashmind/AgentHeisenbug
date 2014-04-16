using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using AgentHeisenbug.Annotations;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Helpers {
    [PsiComponent]
    public class ReferencedTypeHelper {
        private readonly HeisenbugAnnotationCache annotationCache;

        public ReferencedTypeHelper(HeisenbugAnnotationCache annotationCache) {
            this.annotationCache = annotationCache;
        }

        private bool IsImmutable(IType type) {
            if (type.IsDelegate() || type.IsSimplePredefined() || type.IsValue())
                return true;

            return false;
        }

        public bool IsReadOnlyOrImmutable(IType type) {
            if (IsImmutable(type))
                return true;

            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return false;

            var typeElement = scalarType.GetTypeElement();
            if (typeElement == null)
                return false;

            return this.annotationCache.IsReadOnly(typeElement);
        }

        public ThreadSafety GetThreadSafety(IType type) {
            if (IsImmutable(type))
                return ThreadSafety.All;

            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return ThreadSafety.None;

            var typeElement = scalarType.GetTypeElement();
            if (typeElement == null)
                return ThreadSafety.None;

            return this.annotationCache.GetThreadSafety(typeElement);
        }

        public bool IsInstanceThreadSafeOrReadOnlyOrImmutable(IType type) {
            return IsReadOnlyOrImmutable(type)
                || GetThreadSafety(type).Has(ThreadSafety.Instance);
        }
    }
}
