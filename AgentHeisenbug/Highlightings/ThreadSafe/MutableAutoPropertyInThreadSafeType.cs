using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class MutableAutoPropertyInThreadSafeType : IForMutableAutoProperty {
        public IAccessorDeclaration SetterDeclaration { get; private set; }

        public MutableAutoPropertyInThreadSafeType([NotNull] IPropertyDeclaration propertyDeclaration, [NotNull] IAccessorDeclaration setterDeclaration) 
            : this(setterDeclaration.NameIdentifier.NotNull(), propertyDeclaration.DeclaredName)
        {
            SetterDeclaration = setterDeclaration;
        }
    }
}
