﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Highlightings.AnnotationFixSupport;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class FieldOfNonThreadSafeTypeInThreadSafeType : IFixableByAnnotation {
        [NotNull] public IType InvalidType { get; private set; }

        public FieldOfNonThreadSafeTypeInThreadSafeType([NotNull] IFieldDeclaration fieldDeclaration, [NotNull] ITypeUsage invalidTypeUsage, [NotNull] IType invalidType)
            : this(invalidTypeUsage, fieldDeclaration.DeclaredName, invalidType.GetCSharpPresentableName())
        {
            InvalidType = invalidType;
        }

        IEnumerable<AnnotationFixSupport.AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var declaration = InvalidType.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (declaration == null)
                yield break;

            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ReadOnly);
            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ThreadSafe);
        }
    }
}
