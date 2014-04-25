using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug {
    // similar to Argument, but for assumptions within classes
    // mostly to shut up NotNull checks where they do not seem reasonable
    public static class Assume {
        [ContractAnnotation("value:null=>halt")]
        public static void NotNull<T>(string name, T value) 
            where T : class
        {
            if (value == null)
                throw new Exception("Assumption failed: " + name + " is null.");
        }

        [ContractAnnotation("value:null=>halt")]
        public static void NotNull<T>(string name, T? value)
            where T : struct
        {
            if (value == null)
                throw new Exception("Assumption failed: " + name + " is null.");
        }
    }
}
