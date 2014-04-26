using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class TypeHelp {
        public TypeHelp([NotNull] string id, [NotNull] IReadOnlyCollection<string> assemblyNames, TypeThreadSafety threadSafety, [CanBeNull] string threadSafetyText) {
            this.Id = Argument.NotNullOrEmpty("id", id);
            this.AssemblyNames = Argument.NotNullOrEmpty("assemblyNames", assemblyNames);
            this.ThreadSafety = threadSafety;
            this.ThreadSafetyText = threadSafetyText;
        }

        [NotNull] public string Id                                 { get; private set; }
        [NotNull] public IReadOnlyCollection<string> AssemblyNames { get; private set; }
        public TypeThreadSafety ThreadSafety                       { get; private set; }
        [CanBeNull] public string ThreadSafetyText                 { get; private set; }
    }
}