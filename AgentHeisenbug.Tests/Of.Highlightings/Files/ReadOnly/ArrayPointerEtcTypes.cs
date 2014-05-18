using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ReadOnlyAttribute : Attribute {}
}

public class NonReadOnly {}

[ReadOnly]
public unsafe class ArrayPointerEtcTypes {
    private readonly int? x;
    private readonly int* x;
    private readonly int** x;
    private readonly int*[] x;
    private readonly int[] x;
    private readonly int[,] x;
    private readonly int[][] x;

    private readonly NonReadOnly[] x;
    private readonly NonReadOnly[,] x;
    private readonly NonReadOnly[][] x;
}