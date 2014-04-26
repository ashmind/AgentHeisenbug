using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug.Indexer {
    public class AnnotationsByMember {
        [NotNull] public string MemberXmlId { get; private set; }
        [NotNull] public ICollection<Annotation> Annotations { get; private set; }

        public AnnotationsByMember([NotNull] string memberXmlId, [NotNull] params Annotation[] annotations) {
            MemberXmlId = memberXmlId;
            Annotations = annotations;
        }
    }
}
