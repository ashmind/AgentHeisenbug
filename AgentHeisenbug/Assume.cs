using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AgentHeisenbug {
    // similar to Argument, but for assumptions within classes
    // mostly to shut up NotNull checks where they do not seem reasonable
    public static class Assume {
        [NotNull]
        [ContractAnnotation("value:null=>halt")]
        public static T NotNull<T>(T value, string name)
            where T : class
        {
            if (value == null)
                throw new Exception("Assumption failed: " + name + " is null.");

            return value;
        }

        [ContractAnnotation("value:null=>halt")]
        public static T NotNull<T>(T? value, string name)
            where T : struct
        {
            if (value == null)
                throw new Exception("Assumption failed: " + name + " is null.");

            return value.Value;
        }

        // http://youtrack.jetbrains.com/issue/RSRP-413607
        [ContractAnnotation("condition:false=>halt")]
        public static void NotNullWorkaround(bool condition, string name) {
            if (!condition)
                throw new Exception("Assumption failed: " + name + " is null.");
        }
    }
}
