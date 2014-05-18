using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

public class NonThreadSafeClass {}

[ThreadSafe]
public class TestClass {
    public Non{caret}ThreadSafeClass Property { get; private set; }
}