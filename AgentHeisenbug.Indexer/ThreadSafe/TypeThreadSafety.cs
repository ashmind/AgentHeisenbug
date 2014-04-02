using System;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public enum TypeThreadSafety {
        NotFound,
        NotParsed,
        NotSafe,

        Static,
        Instance,
        All
    }
}