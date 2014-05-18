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
    public static SafeClass SafeField;
    public static NonSafeClass NonSafeField;
    public static SafeClass SafeProperty { get; set; }
    public static NonSafeClass NonSafeProperty { get; set; }
    public static void SafeMethod() {}
}

public class NonSafeClass {
    public const string Constant = "C";

    public static SafeClass SafeField;
    public static NonSafeClass NonSafeField;
    public static SafeClass SafeProperty { get; set; }
    public static NonSafeClass NonSafeProperty { get; set; }

    [ThreadSafe] public static void SafeMethod() {}
    [Pure] public static void PureMethod() {}
    public static void NonSafeMethod() {}
}

public enum Enum {
    Value
}

[ThreadSafe]
public class StaticCalls {
    public void Call() {
        SafeClass.SafeMethod();

        SafeClass.SafeField = SafeClass.SafeField;
        SafeClass.NonSafeField = SafeClass.NonSafeField;
        SafeClass.SafeProperty = SafeClass.SafeProperty;
        SafeClass.NonSafeProperty = SafeClass.NonSafeProperty;

        NonSafeClass.Constant;

        NonSafeClass.SafeMethod();
        NonSafeClass.PureMethod();
        NonSafeClass.NonSafeMethod();

        NonSafeClass.SafeField = SafeClass.SafeField;
        NonSafeClass.NonSafeField = SafeClass.NonSafeField;
        NonSafeClass.SafeProperty = SafeClass.SafeProperty;
        NonSafeClass.NonSafeProperty = SafeClass.NonSafeProperty;

        LocalStaticMethod();
        Enum.Value;
    }

    private static void LocalStaticMethod() {
        
    }
}