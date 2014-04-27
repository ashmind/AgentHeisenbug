using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly]
public class ReadOnlyGenericWithNonReadOnlyArgument<T> {
    private readonly T field;
    public T M(T argument) {}
}

[ReadOnly]
public class ReadOnlyGeneric<[ReadOnly] T> {
    private readonly T field;
}

public class NonReadOnly {}

[ReadOnly]
public class GenericFields {
    private readonly ReadOnlyGeneric<int> x;
    private readonly ReadOnlyGeneric<NonReadOnly> x;
    private readonly ReadOnlyGeneric<ReadOnlyGeneric<NonReadOnly>> x;
}