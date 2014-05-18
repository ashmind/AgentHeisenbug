using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe]
public class ThreadSafeGeneric<[ThreadSafe] T> {}
public class NonThreadSafeClass {}

[ThreadSafe]
public class ThreadSafeClass {
    public ThreadSafeGeneric<Non{caret}ThreadSafeClass> Property { get; private set; }
}