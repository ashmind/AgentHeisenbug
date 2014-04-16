using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ThreadSafeAttribute : Attribute {}
    public class ReadOnlyAttribute   : Attribute {}
    public class PureAttribute       : Attribute {}
}

[ThreadSafe] public class Safe     {}
[ReadOnly]   public class ReadOnly {}
             public class NonSafe  {}

[ThreadSafe]
public class Parameters {
    public void M(Safe s) {}
    public void M(ref Safe s) {}
    public void M(out Safe s) {}

    public void M(NonSafe s) {}
    [Pure] public void MPure(NonSafe s) {}

    public void M(ReadOnly s) {}
    public void M(ref ReadOnly s) {}
    public void M(out ReadOnly s) {}
}