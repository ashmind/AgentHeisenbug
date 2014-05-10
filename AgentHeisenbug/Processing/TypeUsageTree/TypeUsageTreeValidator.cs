using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Processing.TypeUsageTree {
    public abstract class TypeUsageTreeValidator {
        [NotNull]
        public IEnumerable<TypeUsagePair> GetAllInvalid([NotNull] IType rootType, [NotNull] ITypeUsage rootUsage) {
            var invalid = new List<TypeUsagePair>();
            ValidateRecursive(rootType, rootUsage, null, invalid);
            return invalid;
        }

        private void ValidateRecursive([NotNull] IType type, [NotNull] ITypeUsage typeUsage, ITypeParameter typeAsParameter, [NotNull] ICollection<TypeUsagePair> invalid) {
            if (typeAsParameter != null && !MustBeValid(typeAsParameter))
                return;

            if (!IsValid(type)) {
                invalid.Add(new TypeUsagePair(type, typeUsage));
                return;
            }

            var parameters = GetTypeParameters(type);
            if (parameters == null)
                return;

            var parametersQueue = new Queue<ITypeParameter>(parameters);
            foreach (var child in typeUsage.Children()) {
                // ReSharper disable once AssignNullToNotNullAttribute
                ValidateUnknownNode(child, parametersQueue, invalid);
            }
        }

        private void ValidateUnknownNode([NotNull] ITreeNode node, [NotNull] Queue<ITypeParameter> parametersQueue, [NotNull] ICollection<TypeUsagePair> invalid) {
            var usage = node as ITypeUsage;
            if (usage != null) {
                var type = CSharpTypeFactory.CreateType(usage);
                var matchingParameter = parametersQueue.Dequeue();
                ValidateRecursive(type, usage, matchingParameter, invalid);
            }
            else {
                foreach (var child in node.Children()) {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ValidateUnknownNode(child, parametersQueue, invalid);
                }
            }
        }

        [CanBeNull]
        private IList<ITypeParameter> GetTypeParameters(IType type) {
            var declared = type as IDeclaredType;
            if (declared == null)
                return null;

            var element = declared.GetTypeElement();
            if (element == null)
                return null;

            return element.TypeParameters;
        }

        protected abstract bool MustBeValid([NotNull] ITypeParameter parameter);
        protected abstract bool IsValid([NotNull] IType type);
    }
}