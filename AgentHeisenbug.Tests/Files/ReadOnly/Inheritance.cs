using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly] public interface IReadOnly {}
[ReadOnly] public class ReadOnlyBase {}

public class NonReadOnlyImplementingClass : IReadOnly {}
[ReadOnly] public class ReadOnlyImplementingClass : IReadOnly {}

public class NonReadOnlySubClass : ReadOnlyBase {}
[ReadOnly] public class ReadOnlySubClass : ReadOnlyBase {}