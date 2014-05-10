using System.Collections.Generic;
using AgentHeisenbug.Processing.FeatureTypes;
using AgentHeisenbug.Processing.TypeUsageTree;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Processing {
    public interface ITypeUsageTreeValidator<TFeature>
        where TFeature : IFeatureMarker
    {
        [NotNull] IEnumerable<TypeUsagePair> GetAllInvalid([NotNull] IType rootType, [NotNull] ITypeUsage rootUsage);
    }
}