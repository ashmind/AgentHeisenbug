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

[ReadOnly]
public class ReadOnlyGenericWithTwoArguments<[ReadOnly] T1, T2> {
}

public class NonReadOnly {}

[ReadOnly]
public class GenericFields {
    private readonly ReadOnlyGenericWithNonReadOnlyArgument<NonReadOnly> x;
    private readonly ReadOnlyGeneric<int> x;
    private readonly ReadOnlyGeneric<NonReadOnly> x;
    private readonly ReadOnlyGeneric<ReadOnlyGeneric<NonReadOnly>> x;
    private readonly ReadOnlyGenericWithTwoArguments<NonReadOnly, int> x;
}