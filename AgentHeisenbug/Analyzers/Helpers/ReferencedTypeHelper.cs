using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using AgentHeisenbug.Annotations;

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

        public bool IsReadOnly(IType type) {
            if (IsImmutable(type))
                return true;

            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return false;

            return this.annotationCache.IsReadOnly(scalarType.GetTypeElement());
        }

        public ThreadSafety GetThreadSafety(IType type) {
            if (IsImmutable(type))
                return ThreadSafety.Values.All;

            var scalarType = type.GetScalarType();
            if (scalarType == null)
                return ThreadSafety.Values.None;

            return this.annotationCache.GetThreadSafety(scalarType.GetTypeElement());
        }
    }
}
