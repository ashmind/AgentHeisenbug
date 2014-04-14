using System;
using System.Collections.Generic;

namespace AgentHeisenbug.Annotations.Generated {
    public sealed class GeneratedThreadSafeAttribute : Attribute {
        public string Safety { get; set; }

        public GeneratedThreadSafeAttribute(string safety) {
            this.Safety = safety;
        }
    }
}
