using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.Common {
    public interface IForMutableField : IHighlighting {
        [NotNull] IFieldDeclaration FieldDeclaration { get; }
    }
}