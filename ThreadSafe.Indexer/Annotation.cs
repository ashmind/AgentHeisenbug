using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer {
    public class Annotation {
        public Annotation(string memberXmlId, string attributeConstructorXmlId, params string[] attributeArguments) {
            this.MemberXmlId = memberXmlId;
            this.AttributeConstructorXmlId = attributeConstructorXmlId;
            this.AttributeArguments = attributeArguments;
        }

        public string MemberXmlId { get; private set; }
        public string AttributeConstructorXmlId { get; private set; }
        public string[] AttributeArguments { get; private set; }
    }
}
