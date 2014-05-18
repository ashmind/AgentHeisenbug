using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.Common {
    public interface IForMutableAutoProperty : IHighlighting {
        [NotNull] IAccessorDeclaration SetterDeclaration { get; }
    }
}