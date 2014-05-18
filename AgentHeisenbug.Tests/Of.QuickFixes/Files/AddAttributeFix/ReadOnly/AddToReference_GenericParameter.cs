using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly]
public class ReadOnlyGeneric<[ReadOnly] T> {}
public class NonReadOnlyClass {}

[ReadOnly]
public class ReadOnlyClass {
    public ReadOnlyGeneric<Non{caret}ReadOnlyClass> Property { get; private set; }
}