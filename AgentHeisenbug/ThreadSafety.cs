using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug {
    public struct ThreadSafety {
        public static class Values {
            public static readonly ThreadSafety All = new ThreadSafety(true, true);
            public static readonly ThreadSafety None = new ThreadSafety();
        }

        private static readonly IDictionary<string, ThreadSafety> ParseMap = new Dictionary<string, ThreadSafety> {
            { "None",     new ThreadSafety(false, false) },
            { "Static",   new ThreadSafety(true,  false) },
            { "Instance", new ThreadSafety(false, true) },
            { "All",      new ThreadSafety(true,  true) },
        };

        public ThreadSafety(bool @static, bool instance) : this() {
            this.Static   = @static;
            this.Instance = instance;
        }

        public bool Static   { get; private set; }
        public bool Instance { get; private set; }

        public static ThreadSafety Parse([NotNull] string value) {
            ThreadSafety parsed;
            if (!ParseMap.TryGetValue(value, out parsed))
                throw new FormatException("Unknown ThreadSafety value '" + value + "'.");

            return parsed;
        }
    }
}
