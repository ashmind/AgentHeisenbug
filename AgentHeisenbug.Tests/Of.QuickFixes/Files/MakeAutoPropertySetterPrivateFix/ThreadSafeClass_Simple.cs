using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe]
public class ThreadSafeClass {
    public int Property { get; s{caret}et; }
}