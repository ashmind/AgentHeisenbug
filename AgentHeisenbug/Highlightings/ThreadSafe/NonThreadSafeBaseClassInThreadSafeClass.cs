using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class NonThreadSafeBaseClassInThreadSafeClass : IFixableByAnnotation {
        [NotNull] public IDeclaredType BaseClass { get; private set; }

        public NonThreadSafeBaseClassInThreadSafeClass([NotNull] IClassLikeDeclaration classDeclaration, [NotNull] IDeclaredTypeUsage baseClassUsage, [NotNull] IDeclaredType baseClass)
            : this(baseClassUsage, baseClass.GetCSharpPresentableName(), classDeclaration.DeclaredElement.NotNull().ShortName)
        {
            BaseClass = baseClass;
        }
        
        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            var typeDeclaration = BaseClass.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
            if (typeDeclaration == null)
                yield break;

            yield return new AnnotationCandidate(typeDeclaration, AnnotationTypeNames.ThreadSafe);
        }
    }
}
