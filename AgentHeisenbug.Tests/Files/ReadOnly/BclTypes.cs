using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
    private readonly Regex x;

    private readonly IReadOnlyCollection<int> x;
    private readonly ReadOnlyCollection<int> x;

    private readonly IReadOnlyList<int> x;

    private readonly IReadOnlyDictionary<int, int> x;
    private readonly ReadOnlyDictionary<int, int> x;
}