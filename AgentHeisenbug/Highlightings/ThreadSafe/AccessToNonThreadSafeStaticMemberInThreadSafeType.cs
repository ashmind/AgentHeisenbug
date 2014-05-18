using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class AccessToNonThreadSafeStaticMemberInThreadSafeType : IFixableByThreadSafeAttribute {
        [NotNull] public ITypeMember Member { get; set; }

        public AccessToNonThreadSafeStaticMemberInThreadSafeType([NotNull] IReferenceExpression reference, [NotNull] ITypeMember member, [NotNull] string memberKind)
            : this(reference, memberKind, member.ShortName)
        {
            Member = member;
        }

        IAttributesOwnerDeclaration IFixableByThreadSafeAttribute.GetTargetDeclaration() {
            return Member.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
        }
    }
}
