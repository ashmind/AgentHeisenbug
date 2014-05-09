using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug {
    [Flags]
    public enum ThreadSafety {
        None,
        Instance = 1,
        Static = 2,
        All = 3
    }
}
