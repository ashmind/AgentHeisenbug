using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentHeisenbug.Processing.TypeUsageTree {
    public struct TypeUsagePair {
        public TypeUsagePair(IType type, ITypeUsage usage) : this() {
            Type = type;
            Usage = usage;
        }

        public ITypeUsage Usage { get; private set; }
        public IType Type { get; private set; }
    }
}
