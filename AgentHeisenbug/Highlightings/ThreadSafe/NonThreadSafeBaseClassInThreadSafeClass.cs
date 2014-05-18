using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class NonThreadSafeBaseClassInThreadSafeClass : IFixableByThreadSafeAttribute {
        [NotNull] public IDeclaredType BaseClass { get; private set; }

        public NonThreadSafeBaseClassInThreadSafeClass([NotNull] ITypeDeclaration classDeclaration, [NotNull] IDeclaredTypeUsage baseClassUsage, [NotNull] IDeclaredType baseClass)
            : this(baseClassUsage, baseClass.GetCSharpPresentableName(), classDeclaration.DeclaredElement.NotNull().ShortName)
        {
            BaseClass = baseClass;
        }
        
        IAttributesOwnerDeclaration IFixableByThreadSafeAttribute.GetTargetDeclaration() {
            var typeElement = BaseClass.GetTypeElement();
            if (typeElement == null)
                return null;

            return typeElement.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
        }
    }
}
