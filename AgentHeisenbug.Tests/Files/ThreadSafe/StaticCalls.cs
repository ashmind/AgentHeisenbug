using System;
using JetBrains.Annotations;
using JetBrains.CommonControls.Validation;
using JetBrains.ReSharper.Daemon.CSharp.Errors;

namespace JetBrains.Annotations {
    public class ThreadSafeAttribute : Attribute {}
    public class ReadOnlyAttribute   : Attribute {}
    public class PureAttribute       : Attribute {}
}

[ThreadSafe]
public class SafeClass {
    public static void StaticMethod() {}
}

public class NonSafeClass {
    [ThreadSafe] public static void SafeMethod() {}
    [Pure] public static void PureMethod() {}
    public static void NonSafeMethod() {}
}


[ThreadSafe]
public class StaticCalls {
    public void Call() {
        SafeClass.StaticMethod();
        NonSafeClass.SafeMethod();
        NonSafeClass.PureMethod();
        NonSafeClass.NonSafeMethod();
        LocalStaticMethod();
    }

    private static void LocalStaticMethod() {
        
    }
}