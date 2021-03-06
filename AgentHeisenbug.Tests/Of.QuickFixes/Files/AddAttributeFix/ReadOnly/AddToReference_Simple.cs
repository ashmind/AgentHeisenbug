﻿using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ReadOnlyAttribute : Attribute {}
}

public class NonReadOnlyClass {}

[ReadOnly]
public class TestClass {
    public Non{caret}ReadOnlyClass Property { get; private set; }
}