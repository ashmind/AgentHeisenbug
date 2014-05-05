﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ThreadSafeAttribute : Attribute {}
}

[ThreadSafe]
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

    public void StaticMethods() {
        Regex.Match("", "");
        Uri.EscapeDataString("");
        decimal.Add(0, 0);
    }
}