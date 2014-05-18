using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ReadOnly {
    public partial class MutableAutoPropertyInReadOnlyType : IForMutableAutoProperty {
        public IAccessorDeclaration SetterDeclaration { get; private set; }

        public MutableAutoPropertyInReadOnlyType([NotNull] IPropertyDeclaration propertyDeclaration, [NotNull] IAccessorDeclaration setterDeclaration) 
            : this(setterDeclaration.NameIdentifier.NotNull(), propertyDeclaration.DeclaredName)
        {
            SetterDeclaration = setterDeclaration;
        }
    }
}
