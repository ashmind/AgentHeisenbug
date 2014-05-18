using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ReadOnlyAttribute : Attribute {}
}

public class NonReadOnlyBase {}

public class NonReadOnlyClass_ImplementingNonReadOnly : NonReadOnlyBase {}
[ReadOnly] public class ReadOnlyClass_ImplementingNonReadOnly : NonReadOnlyBase {}