﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using AgentHeisenbug.Highlightings.AnnotationFixSupport;

namespace AgentHeisenbug.Highlightings.ReadOnly {
    public partial class AutoPropertyOfNonReadOnlyTypeInReadOnlyType : IFixableByAnnotation {
        [NotNull] public IType InvalidType { get; private set; }

        public AutoPropertyOfNonReadOnlyTypeInReadOnlyType([NotNull] IPropertyDeclaration propertyDeclaration, [NotNull] ITypeUsage invalidTypeUsage, [NotNull] IType invalidType)
            : this(invalidTypeUsage, propertyDeclaration.DeclaredName, invalidType.GetCSharpPresentableName())
        {
            InvalidType = invalidType;
        }
        
        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var declaration = InvalidType.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (declaration == null)
                yield break;

            yield return new AnnotationCandidate(declaration, AnnotationTypeNames.ReadOnly);
        }
    }
}