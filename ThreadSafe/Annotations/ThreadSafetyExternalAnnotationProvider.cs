using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AshMind.Extensions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using JetBrains.ReSharper.Psi;
using JetBrains.Util.dataStructures;
using ThreadSafetyTips;

namespace ThreadSafety.Annotations {
    [PsiComponent]
    public class ThreadSafetyExternalAnnotationProvider {
        private readonly JsonSerializer serializer = new JsonSerializer();
        private readonly ConcurrentDictionary<string, IDictionary<string, ThreadSafetyLevel>> cache = new ConcurrentDictionary<string, IDictionary<string, ThreadSafetyLevel>>();
        private readonly DirectoryInfo annotationsDirectory;

        public ThreadSafetyExternalAnnotationProvider() {
            this.annotationsDirectory =  new DirectoryInfo(@"d:\Development\VS 2012\ThreadSafe\#reflected\");
        }

        public ThreadSafetyLevel GetThreadSafetyLevel([NotNull] ITypeMember member) {
            var assemblyModule = member.Module as IAssemblyPsiModule;
            if (assemblyModule == null)
                return ThreadSafetyLevel.None;
            
            var assemblyName = assemblyModule.Assembly.AssemblyName.Name;
            var annotations = this.cache.GetOrAdd(assemblyName, GetExternalAnnotationsUncached);
            if (annotations == null)
                return ThreadSafetyLevel.None;

            return annotations.GetValueOrDefault(member.XMLDocId);
        }

        private IDictionary<string, ThreadSafetyLevel> GetExternalAnnotationsUncached([NotNull] string assemblyName) {
            var annotationFiles = annotationsDirectory.GetFiles(assemblyName + ".*.json");
            var dictionary = new Dictionary<string, ThreadSafetyLevel>();

            foreach (var file in annotationFiles.OrderBy(f => f.Name)) {
                using (var reader = file.OpenText())
                using (var jsonReader = new JsonTextReader(reader)) {
                    serializer.Populate(jsonReader, dictionary);
                }
            }

            return dictionary;
        }
    }
}
