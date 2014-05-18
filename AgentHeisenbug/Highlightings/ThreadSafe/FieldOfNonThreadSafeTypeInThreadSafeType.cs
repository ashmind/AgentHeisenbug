using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class FieldOfNonThreadSafeTypeInThreadSafeType : IFixableByThreadSafeAttribute {
        [NotNull] public IType InvalidType { get; set; }

        public FieldOfNonThreadSafeTypeInThreadSafeType([NotNull] IFieldDeclaration fieldDeclaration, [NotNull] ITypeUsage invalidTypeUsage, [NotNull] IType invalidType)
            : this(invalidTypeUsage, fieldDeclaration.DeclaredName, invalidType.GetCSharpPresentableName())
        {
            InvalidType = invalidType;
        }

        IAttributesOwnerDeclaration IFixableByThreadSafeAttribute.GetTargetDeclaration() {
            return InvalidType.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
        }
    }
}
