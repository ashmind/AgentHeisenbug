using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class TypeDescriptionIdEqualityComparer : IEqualityComparer<TypeDescription> {
        public static TypeDescriptionIdEqualityComparer Default { get; private set; }

        static TypeDescriptionIdEqualityComparer() {
            Default = new TypeDescriptionIdEqualityComparer();
        }

        public bool Equals(TypeDescription x, TypeDescription y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(TypeDescription obj) {
            return obj.Id.GetHashCode();
        }
    }
}
