using System;
using System.Collections.Generic;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class TypeDescription {
        public TypeDescription(string id, IReadOnlyCollection<string> assemblyNames, TypeThreadSafety threadSafety, string threadSafetyText) {
            this.Id = Argument.NotNullOrEmpty("id", id);
            this.AssemblyNames = Argument.NotNullOrEmpty("assemblyNames", assemblyNames);
            this.ThreadSafety = threadSafety;
            this.ThreadSafetyText = threadSafetyText;
        }

        public string Id                                 { get; private set; }
        public IReadOnlyCollection<string> AssemblyNames { get; private set; }
        public TypeThreadSafety ThreadSafety             { get; private set; }
        public string ThreadSafetyText                   { get; private set; }
    }
}