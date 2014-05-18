using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

public class NonThreadSafeBase {}

[ThreadSafe]
public class ThreadSafeClass_InheritingNonThreadSafe : Non{caret}ThreadSafeBase {}