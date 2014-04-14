using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class TypeHelpIdEqualityComparer : IEqualityComparer<TypeHelp> {
        public static TypeHelpIdEqualityComparer Default { get; private set; }

        static TypeHelpIdEqualityComparer() {
            Default = new TypeHelpIdEqualityComparer();
        }

        public bool Equals(TypeHelp x, TypeHelp y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(TypeHelp obj) {
            return obj.Id.GetHashCode();
        }
    }
}
