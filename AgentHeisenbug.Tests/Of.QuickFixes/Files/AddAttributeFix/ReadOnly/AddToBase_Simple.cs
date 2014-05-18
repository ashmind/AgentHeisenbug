using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ReadOnlyAttribute : Attribute {}
}

public class NonReadOnlyBase {}

[ReadOnly]
public class ReadOnlyClass_InheritingNonReadOnly : Non{caret}ReadOnlyBase {}