using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly]
public class BclTypes {
    private readonly int x;
    private readonly int? x;
    private readonly decimal x;
    private readonly decimal? x;
    private readonly string x;
    private readonly DateTime x;
    private readonly Action x;
    private readonly IntPtr x;
    private readonly Uri x;

    private readonly IReadOnlyCollection<T> x;
    private readonly  ReadOnlyCollection<T> x;

    private readonly IReadOnlyList<T> x;

    private readonly IReadOnlyDictionary<TKey, TValue> x;
    private readonly  ReadOnlyDictionary<TKey, TValue> x;
}