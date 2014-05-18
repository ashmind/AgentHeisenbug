using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class AccessToNonThreadSafeStaticMemberInThreadSafeType : IFixableByAnnotation {
        [NotNull] public ITypeMember Member { get; private set; }

        public AccessToNonThreadSafeStaticMemberInThreadSafeType([NotNull] IReferenceExpression reference, [NotNull] ITypeMember member, [NotNull] string memberKind)
            : this(reference, memberKind, member.ShortName)
        {
            Member = member;
        }
        
        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var declaration = Member.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (declaration == null)
                yield break;

            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ThreadSafe);
            var containingType = declaration.GetContainingTypeDeclaration();
            if (containingType != null)
                yield return new AnnotationCandidate(containingType, AnnotationTypeNames.ThreadSafe);
        }
    }
}
