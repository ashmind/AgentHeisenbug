using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreadSafetyTips {
    public enum ThreadSafetyLevel {
        None,
        Static,
        Instance,
        All
    }
}
