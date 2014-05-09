using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Processing {
    public class TypeUsageTreeValidator {
        [NotNull] private readonly Func<ITypeParameter, bool> _mustBeValid;
        [NotNull] private readonly Func<IType, bool> _isValid;
        [NotNull] private readonly Action<IType, ITypeUsage> _processInvalid;

        public TypeUsageTreeValidator(
            [NotNull] Func<ITypeParameter, bool> mustBeValid,
            [NotNull] Func<IType, bool> isValid,
            [NotNull] Action<IType, ITypeUsage> processInvalid
        ) {
            _mustBeValid = mustBeValid;
            _isValid = isValid;
            _processInvalid = processInvalid;
        }
        
        public static void Validate(
            [NotNull] ITypeUsage rootUsage,
            [NotNull] IType rootType,
            [NotNull] Func<ITypeParameter, bool> mustBeValid,
            [NotNull] Func<IType, bool> isValid,
            [NotNull] Action<IType, ITypeUsage> processInvalid
        ) {
            new TypeUsageTreeValidator(mustBeValid, isValid, processInvalid)
                .Validate(rootType, rootUsage);
        }

        public void Validate([NotNull] IType rootType, [NotNull] ITypeUsage rootUsage) {
            var parameters = ValidateAndGetParameters(rootType, rootUsage, null);
            if (parameters == null)
                return;

            ValidateSubTree(rootUsage, parameters);
        }

        [CanBeNull]
        private IList<ITypeParameter> ValidateAndGetParameters([NotNull] IType type, [NotNull] ITypeUsage usage, [CanBeNull] ITypeParameter parameter) {
            if (parameter != null && !_mustBeValid(parameter))
                return null;
            
            if (!_isValid(type)) {
                _processInvalid(type, usage);
                return null;
            }

            var declared = type as IDeclaredType;
            if (declared == null)
                return null;

            var element = declared.GetTypeElement();
            if (element == null)
                return null;

            return element.TypeParameters;
        }

        private void ValidateSubTree([NotNull] ITreeNode parent, [NotNull] IList<ITypeParameter> parameters) {
            var parametersQueue = (Queue<ITypeParameter>)null;
            foreach (var child in parent.Children()) {
                var usage = child as ITypeUsage;
                var subtreeParameters = parameters;
                if (usage != null) {
                    var childType = CSharpTypeFactory.CreateType(usage);
                    parametersQueue = parametersQueue ?? new Queue<ITypeParameter>(parameters);
                    var matchingParameter = parametersQueue.Dequeue();
                    subtreeParameters = ValidateAndGetParameters(childType, usage, matchingParameter);

                    if (subtreeParameters == null)
                        return;
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                ValidateSubTree(child, subtreeParameters);
            }
        }
    }
}
