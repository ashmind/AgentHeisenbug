using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class AutoPropertyOfNonThreadSafeTypeInThreadSafeType : IFixableByAnnotation {
        [NotNull] public IType InvalidType { get; private set; }

        public AutoPropertyOfNonThreadSafeTypeInThreadSafeType([NotNull] IPropertyDeclaration propertyDeclaration, [NotNull] ITypeUsage invalidTypeUsage, [NotNull] IType invalidType)
            : this(invalidTypeUsage, propertyDeclaration.DeclaredName, invalidType.GetCSharpPresentableName())
        {
            InvalidType = invalidType;
        }
        
        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var declaration = InvalidType.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (declaration == null)
                yield break;

            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ReadOnly);
            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ThreadSafe);
        }
    }
}
