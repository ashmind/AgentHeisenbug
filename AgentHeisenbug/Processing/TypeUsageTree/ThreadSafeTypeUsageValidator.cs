using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AgentHeisenbug.Processing.TypeUsageTree {
    [PsiComponent]
    public class ThreadSafeTypeUsageValidator : TypeUsageTreeValidator {
        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeTypeUsageValidator([NotNull] AnalyzerPreconditions preconditions, [NotNull] HeisenbugFeatureProvider featureProvider) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override bool MustBeValid(ITypeParameter parameter) {
            return _preconditions.MustBeThreadSafe(parameter);
        }

        protected override bool IsValid(IType type) {
            return _featureProvider.GetFeatures(type).IsInstanceAccessThreadSafeOrReadOnly;
        }
    }
}
