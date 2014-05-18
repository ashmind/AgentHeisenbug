using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings {
    public partial class ThreadSafeBaseClassInNonThreadSafeClass : IFixableByThreadSafeAttribute {
        [NotNull] public IClassLikeDeclaration ClassDeclaration { get; private set; }

        public ThreadSafeBaseClassInNonThreadSafeClass([NotNull] IClassLikeDeclaration classDeclaration, [NotNull] IDeclaredTypeUsage baseClassUsage, [NotNull] IDeclaredType baseClass)
            : this(baseClassUsage, baseClass.GetCSharpPresentableName(), classDeclaration.DeclaredElement.NotNull().ShortName) 
        {
            ClassDeclaration = classDeclaration;
        }
        
        IAttributesOwnerDeclaration IFixableByThreadSafeAttribute.GetTargetDeclaration() {
            return ClassDeclaration;
        }
    }
}
