using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class MutableFieldInThreadSafeType : IForMutableField {
        public IFieldDeclaration FieldDeclaration { get; private set; }

        public MutableFieldInThreadSafeType([NotNull] IFieldDeclaration fieldDeclaration) 
            : this(fieldDeclaration, fieldDeclaration.DeclaredName)
        {
            FieldDeclaration = fieldDeclaration;
        }
    }
}
