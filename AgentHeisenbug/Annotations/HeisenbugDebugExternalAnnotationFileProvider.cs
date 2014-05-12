using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using JetBrains.Metadata.Utils;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl.Reflection2.ExternalAnnotations;
using JetBrains.Util;

namespace AgentHeisenbug.Annotations {
#if DEBUG
    [PsiComponent]
    public class HeisenbugDebugExternalAnnotationFileProvider : IExternalAnnotationsFileProvider {
        [NotNull] private readonly FileSystemPath _path;

        public HeisenbugDebugExternalAnnotationFileProvider() {
            _path = FileSystemPath.Parse(Assembly.GetExecutingAssembly().Location)
                                  .Combine("../../../#annotations");
        }

        public IEnumerable<FileSystemPath> GetAnnotationsFiles(AssemblyNameInfo assemblyName = null, FileSystemPath assemblyLocation = null) {
            if (assemblyName == null)
                return Enumerable.Empty<FileSystemPath>();

            var directoryForAssembly = _path.Combine(assemblyName.Name);
            if (!directoryForAssembly.ExistsDirectory)
                return Enumerable.Empty<FileSystemPath>();

            return directoryForAssembly.GetDirectoryEntries("*.xml", true);
        }
    }
#endif
}
