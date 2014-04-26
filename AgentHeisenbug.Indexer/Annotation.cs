using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug.Indexer {
    public class Annotation {
        [NotNull] public string AttributeConstructorXmlId { get; private set; }
        [NotNull] public string[] AttributeArguments { get; private set; }

        public Annotation([NotNull] string attributeConstructorXmlId, [NotNull] params string[] attributeArguments) {
            AttributeConstructorXmlId = attributeConstructorXmlId;
            AttributeArguments = attributeArguments;
        }
    }
}
