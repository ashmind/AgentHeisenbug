﻿using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ThreadSafeAttribute : Attribute {}
}

public class NonThreadSafeClass {
    public static void Method() {}
}

[ThreadSafe]
public class TestClass {
    public void SafeMethod() {
        NonThreadSafeClass.Meth{caret}od();
    }
}