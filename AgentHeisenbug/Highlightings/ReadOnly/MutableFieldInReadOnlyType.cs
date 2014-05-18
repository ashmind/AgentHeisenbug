using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ReadOnly {
    public partial class MutableFieldInReadOnlyType : IForMutableField {
        public IFieldDeclaration FieldDeclaration { get; private set; }

        public MutableFieldInReadOnlyType([NotNull] IFieldDeclaration fieldDeclaration) 
            : this(fieldDeclaration, fieldDeclaration.DeclaredName)
        {
            FieldDeclaration = fieldDeclaration;
        }
    }
}
