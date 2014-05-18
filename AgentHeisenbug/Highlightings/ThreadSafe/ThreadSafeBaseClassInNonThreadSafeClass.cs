using System;
using System.Collections.Generic;
using System.Linq;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    public partial class ThreadSafeBaseClassInNonThreadSafeClass : IFixableByAnnotation {
        [NotNull] public IClassLikeDeclaration ClassDeclaration { get; private set; }

        public ThreadSafeBaseClassInNonThreadSafeClass([NotNull] IClassLikeDeclaration classDeclaration, [NotNull] IDeclaredTypeUsage baseClassUsage, [NotNull] IDeclaredType baseClass)
            : this(baseClassUsage, baseClass.GetCSharpPresentableName(), classDeclaration.DeclaredElement.NotNull().ShortName) 
        {
            ClassDeclaration = classDeclaration;
        }
        
        IEnumerable<AnnotationCandidate> IFixableByAnnotation.GetCandidates() {
            yield return new AnnotationCandidate(ClassDeclaration, AnnotationTypeNames.ThreadSafe);
        }
    }
}
