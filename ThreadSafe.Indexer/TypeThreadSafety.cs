using System;

namespace ThreadSafe.Indexer {
    public enum TypeThreadSafety {
        NotFound,
        NotParsed,
        NotSafe,

        Static,
        Instance,
        All
    }
}