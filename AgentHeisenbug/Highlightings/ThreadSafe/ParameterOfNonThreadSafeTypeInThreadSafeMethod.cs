using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings.AnnotationFixSupport;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class ParameterOfNonThreadSafeTypeInThreadSafeMethod : IFixableByAnnotation {
        [NotNull] public IMethodDeclaration MethodDeclaration { get; private set; }
        [NotNull] public IType InvalidType { get; private set; }

        public ParameterOfNonThreadSafeTypeInThreadSafeMethod(
            [NotNull] IParameterDeclaration parameterDeclaration,
            [NotNull] IMethodDeclaration methodDeclaration,
            [NotNull] ITypeUsage invalidTypeUsage,
            [NotNull] IType invalidType
        ) : this(invalidTypeUsage, parameterDeclaration.DeclaredName, invalidType.GetCSharpPresentableName()) {
            MethodDeclaration = methodDeclaration;
            InvalidType = invalidType;
        }

        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var declaration = InvalidType.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (declaration == null)
                yield break;

            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ReadOnly);
            yield return new AnnotationCandidate(MethodDeclaration, AnnotationTypeNames.Pure);
            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ThreadSafe);
        }
    }
}
