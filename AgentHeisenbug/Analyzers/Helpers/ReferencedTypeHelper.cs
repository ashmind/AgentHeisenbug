using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using AgentHeisenbug.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Helpers {
    [PsiComponent]
    public class ReferencedTypeHelper {
        [NotNull] private static readonly HeisenbugAnnotations FalseAnnotations = new HeisenbugAnnotations(false, ThreadSafety.None);
        [NotNull] private readonly HeisenbugAnnotationCache _annotationCache;

        public ReferencedTypeHelper([NotNull] HeisenbugAnnotationCache annotationCache) {
            _annotationCache = annotationCache;
        }

        public bool IsReadOnly([NotNull] IType type) {
            Argument.NotNull("type", type);
            if (IsTriviallyImmutable(type))
                return true;

            return GetAnnotations(type).IsReadOnly;
        }

        public bool IsInstanceThreadSafeOrReadOnly([NotNull] IType type) {
            Argument.NotNull("type", type);
            if (IsTriviallyImmutable(type))
                return true;

            var annotations = GetAnnotations(type);
            return annotations.IsReadOnly
                || annotations.ThreadSafety.Has(ThreadSafety.Instance);
        }

        private bool IsTriviallyImmutable([NotNull] IType type) {
            return type.IsSimplePredefined()
                || type.IsValueType()
                || type.IsDelegateType();
        }
        
        [NotNull]
        private HeisenbugAnnotations GetAnnotations([NotNull] IType type) {
            var declaredType = type as IDeclaredType;
            if (declaredType == null) // arrays, pointers => all not threadsafe or readonly
                return FalseAnnotations;

            var typeElement = declaredType.GetTypeElement();
            if (typeElement == null)
                return FalseAnnotations;

            return _annotationCache.GetAnnotations(typeElement);
        }

        public void ValidateTypeUsageTree(
            [NotNull] ITypeUsage rootUsage,
            [NotNull] Func<IType, bool> isValid,
            [NotNull] Action<IType, ITypeUsage> processInvalid
        ) {
            ValidateTypeUsageSubTree(rootUsage, isValid, processInvalid);
        }

        private void ValidateTypeUsageSubTree(
            [NotNull] ITreeNode root,
            [NotNull] Func<IType, bool> isValid,
            [NotNull] Action<IType, ITypeUsage> processInvalid
        ) {
            var usage = root as ITypeUsage;
            if (usage != null) {
                var type = CSharpTypeFactory.CreateType(usage);
                if (!isValid(type)) {
                    processInvalid(type, usage);
                    return;
                }
            }

            foreach (var child in root.Children()) {
                // ReSharper disable once AssignNullToNotNullAttribute
                ValidateTypeUsageSubTree(child, isValid, processInvalid);
            }
        }
    }
}
