using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer {
    public class AnnotationsByMember {
        public AnnotationsByMember(string memberXmlId, params Annotation[] annotations) {
            this.MemberXmlId = memberXmlId;
            this.Annotations = annotations;
        }

        public string MemberXmlId { get; private set; }
        public ICollection<Annotation> Annotations { get; private set; }
    }
}
