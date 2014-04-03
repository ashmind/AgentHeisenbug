using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug {
    public struct ThreadSafety {
        public static class Values {
            public static readonly ThreadSafety All = new ThreadSafety(true, true);
            public static readonly ThreadSafety None = new ThreadSafety();
        }

        public ThreadSafety(bool @static, bool instance) : this() {
            this.Static   = @static;
            this.Instance = instance;
        }

        public bool Static   { get; private set; }
        public bool Instance { get; private set; }
    }
}
