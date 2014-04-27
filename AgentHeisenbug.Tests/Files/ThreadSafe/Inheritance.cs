using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [ThreadSafe]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe] public interface IThreadSafe {}
[ThreadSafe] public class ThreadSafeBase {}

public class NonReadOnlyImplementingClass : IThreadSafe {}
[ThreadSafe] public class ReadOnlyImplementingClass : IThreadSafe {}

public class NonThreadSafeSubClass : ThreadSafeBase {}
[ThreadSafe] public class ThreadSafeSubClass : ThreadSafeBase {}