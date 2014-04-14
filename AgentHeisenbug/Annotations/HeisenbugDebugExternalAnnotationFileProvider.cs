using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Env;
using JetBrains.Metadata.Utils;
using JetBrains.ReSharper.Psi.Impl.Reflection2.ExternalAnnotations;
using JetBrains.Util;

namespace AgentHeisenbug.Annotations {
    // DEBUG ONLY

    [EnvironmentComponent(Sharing.Product)]
    public class HeisenbugDebugExternalAnnotationFileProvider : IExternalAnnotationsFileProvider {
        public IEnumerable<FileSystemPath> GetAnnotationsFiles(AssemblyNameInfo assemblyName = null, FileSystemPath assemblyLocation = null) {
            if (assemblyName == null)
                return Enumerable.Empty<FileSystemPath>();

            var root = FileSystemPath.Parse(@"d:\Development\VS 2012\AgentHeisenbug\#reflected\");
            var directoryForAssembly = root.Combine(assemblyName.Name);
            if (!directoryForAssembly.ExistsDirectory)
                return Enumerable.Empty<FileSystemPath>();

            return directoryForAssembly.GetDirectoryEntries("*.xml", true);
        }
    }
}
