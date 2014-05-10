using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe] public interface IThreadSafe {}
[ThreadSafe] public class ThreadSafeBase {}
public interface INonThreadSafe {}
public class NonThreadSafeBase {}

public class NonThreadSafeClass_ImplementingThreadSafe : IThreadSafe {}
[ThreadSafe] public class ThreadSafeClass_ImplementingThreadSafe : IThreadSafe {}
[ThreadSafe] public class ThreadSafeClass_ImplementingNonThreadSafe : IThreadSafe {}

public class NonThreadSafeClass_InheritingThreadSafe : ThreadSafeBase {}
[ThreadSafe] public class ThreadSafeClass_InheritingThreadSafe : ThreadSafeBase {}
[ThreadSafe] public class ThreadSafeClass_InheritingNonThreadSafe : NonThreadSafeBase {}