using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings {
    public interface IFixableByThreadSafeAttribute : IHighlighting {
        [CanBeNull]
        IAttributesOwnerDeclaration GetTargetDeclaration();
    }
}
