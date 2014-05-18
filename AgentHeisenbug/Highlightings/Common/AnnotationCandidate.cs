using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Highlightings.Common {
    public struct AnnotationCandidate {
        [NotNull] public IAttributesOwnerDeclaration TargetDeclaration { get; private set; }
        [NotNull] public string AnnotationTypeName { get; private set; }

        public AnnotationCandidate([NotNull] IAttributesOwnerDeclaration targetDeclaration, [NotNull] string annotationTypeName) : this() {
            TargetDeclaration = targetDeclaration;
            AnnotationTypeName = annotationTypeName;
        }
    }
}
