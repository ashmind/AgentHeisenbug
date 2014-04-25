using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ReadOnlyAttribute : Attribute {}
}

[ReadOnly] public class ReadOnly {}
           public class NonReadOnly {}

[ReadOnly]
public class Properties {
    public ReadOnly AutoGetSet { get; set; }
    public static ReadOnly StaticAutoGetSet { get; set; }

    public ReadOnly GetSet { get { return null; } set { } }
    public ReadOnly GetOnly { get { return null; } }

    public ReadOnly AutoPrivateSet1 { get; private set; }
    public static ReadOnly StaticAutoPrivateSet1 { get; private set; }

    public NonReadOnly AutoPrivateSet2 { get; private set; }
    public static NonReadOnly StaticAutoPrivateSet2 { get; private set; }

    static Properties() {
        new Properties().AutoPrivateSet1 = null;
        StaticAutoPrivateSet1 = null;
    }

    public Properties() {
        AutoPrivateSet1 = null;
        StaticAutoPrivateSet1 = value;
    }

    public void ChangeThis(Safe value) {
        AutoPrivateSet1 = value;
        StaticAutoPrivateSet1 = value;
    }

    public void ChangeOther(Properties properties, Safe value) {
        properties.AutoPrivateSet1 = value;
    }
}