using System;
using JetBrains.Annotations;

namespace JetBrains.Annotations {
    public class ThreadSafeAttribute : Attribute {}
    public class ReadOnlyAttribute   : Attribute {}
}

[ThreadSafe] public class Safe     {}
[ReadOnly]   public class ReadOnly {}
             public class NonSafe  {}

[ThreadSafe]
public class Properties {
    public Safe AutoGetSet1 { get; set; }

    public Safe GetSet1 { get { return null; } set { } }

    public NonSafe GetOnly1 { get { return null; } }

    public Safe AutoPrivateSet1 { get; private set; }
    public ReadOnly AutoPrivateSet2 { get; private set; }
    public NonSafe AutoPrivateSet3 { get; private set; }

    public static Safe StaticAutoGetSet1 { get; set; }
    public static Safe StaticAutoPrivateSet1 { get; private set; }
    public static NonSafe StaticAutoPrivateSet2 { get; private set; }

    static Properties() {
        StaticAutoPrivateSet1 = null;
        new Properties().AutoPrivateSet1 = null;
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