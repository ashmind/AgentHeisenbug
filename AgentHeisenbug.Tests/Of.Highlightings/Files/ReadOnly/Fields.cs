﻿using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly] public class ReadOnly {}
           public class NonReadOnly {}

[ReadOnly]
public class Fields {
    private ReadOnly mutable;
    private static ReadOnly staticMutable;

    private readonly ReadOnly readonly1;
    private readonly static ReadOnly staticReadOnly1;
    
    private readonly NonReadOnly readonly2;
    private readonly static NonReadOnly staticReadOnly2;
}