using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class ThreadSafeInterfaceInNonThreadSafeType : IFixableByThreadSafeAttribute {
        [NotNull] public IClassLikeDeclaration TypeDeclaration { get; private set; }

        public ThreadSafeInterfaceInNonThreadSafeType([NotNull] IClassLikeDeclaration typeDeclaration, [NotNull] IDeclaredTypeUsage baseClassUsage, [NotNull] IDeclaredType baseClass)
            : this(baseClassUsage, baseClass.GetCSharpPresentableName(), typeDeclaration.DeclaredElement.NotNull().ShortName)
        {
            TypeDeclaration = typeDeclaration;
        }
        
        IAttributesOwnerDeclaration IFixableByThreadSafeAttribute.GetTargetDeclaration() {
            return TypeDeclaration;
        }
    }
}
