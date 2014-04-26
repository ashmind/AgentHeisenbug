using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Annotations {
    public class HeisenbugAnnotations {
        public bool IsReadOnly { get; private set; }
        public ThreadSafety ThreadSafety { get; private set; }

        public HeisenbugAnnotations(bool isReadOnly, ThreadSafety threadSafety) {
            IsReadOnly = isReadOnly;
            ThreadSafety = threadSafety;
        }
    }
}
