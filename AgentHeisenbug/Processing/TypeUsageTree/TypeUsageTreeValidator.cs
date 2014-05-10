using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Processing.TypeUsageTree {
    public class TypeUsageTreeValidator<TFeature> : ITypeUsageTreeValidator<TFeature>
        where TFeature : IFeatureMarker
    {
        [NotNull] private readonly IAnalyzerPrecondition<TFeature> _precondition;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public TypeUsageTreeValidator([NotNull] IAnalyzerPrecondition<TFeature> precondition, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _precondition = precondition;
            _featureProvider = featureProvider;
        }

        public IEnumerable<TypeUsagePair> GetAllInvalid(IType rootType, ITypeUsage rootUsage) {
            var invalid = new List<TypeUsagePair>();
            ValidateRecursive(rootType, rootUsage, null, invalid);
            return invalid;
        }

        private void ValidateRecursive([NotNull] IType type, [NotNull] ITypeUsage typeUsage, ITypeParameter typeAsParameter, [NotNull] ICollection<TypeUsagePair> invalid) {
            if (typeAsParameter != null && !_precondition.Applies(typeAsParameter))
                return;

            var features = _featureProvider.GetFeatures(type);
            if (!features.Has<TFeature>()) {
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
    }

    #region Workaround for R# not supporting open generics
    [PsiComponent]
    public class ReadOnlyTypeUsageTreeValidator : TypeUsageTreeValidator<ReadOnly> {
        public ReadOnlyTypeUsageTreeValidator([NotNull] IAnalyzerPrecondition<ReadOnly> precondition, [NotNull] HeisenbugFeatureProvider featureProvider) : base(precondition, featureProvider) {}
    }

    [PsiComponent]
    public class InstanceThreadSafeTypeUsageTreeValidator : TypeUsageTreeValidator<InstanceThreadSafe> {
        public InstanceThreadSafeTypeUsageTreeValidator([NotNull] IAnalyzerPrecondition<InstanceThreadSafe> precondition, [NotNull] HeisenbugFeatureProvider featureProvider) : base(precondition, featureProvider) {}
    }
    #endregion
}