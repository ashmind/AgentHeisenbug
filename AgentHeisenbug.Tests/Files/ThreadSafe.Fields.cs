using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ThreadSafeAttribute : Attribute {}
    public class ReadOnlyAttribute   : Attribute {}
}

[ThreadSafe] public class Safe     {}
[ReadOnly]   public class ReadOnly {}
             public class NonSafe  {}

[ThreadSafe]
public class Fields {
    private Safe mutable1;
    
    private readonly Safe readonly1;
    private readonly ReadOnly readonly2;
    private readonly NonSafe readonly3;
}