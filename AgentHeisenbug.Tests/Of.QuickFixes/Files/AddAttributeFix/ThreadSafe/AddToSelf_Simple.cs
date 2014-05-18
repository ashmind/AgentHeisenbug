using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe]
public class ThreadSafeBase {}

public class NonThreadSafeClass_InheritingThreadSafe : Thread{caret}SafeBase {}