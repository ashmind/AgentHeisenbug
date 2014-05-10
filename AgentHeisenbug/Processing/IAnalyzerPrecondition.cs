using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Processing {
    // ReSharper disable once UnusedTypeParameter
    public interface IAnalyzerPrecondition<in TFeature>
        where TFeature : IFeatureMarker 
    {
        bool Applies(ITypeParameter parameter);
        bool Applies(ITreeNode element);
    }
}
