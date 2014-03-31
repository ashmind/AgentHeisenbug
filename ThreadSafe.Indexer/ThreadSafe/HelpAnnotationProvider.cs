using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AshMind.Extensions;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class HelpAnnotationProvider {
        private const string ThreadSafeConstructorXmlId = "M:GeneratedThreadSafeAttribute.#ctor";

        private readonly HelpRawReader reader;
        private const string NonParsedFileName = "NotParsed.Generated.txt";

        public HelpAnnotationProvider(HelpRawReader reader) {
            this.reader = reader;
        }

        public IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(IEnumerable<FileInfo> helpFiles, DirectoryInfo tempDirectory) {
            if (tempDirectory.Exists)
                tempDirectory.Delete(true);
            tempDirectory.Create();

            AggregateByAssembly(tempDirectory, reader.ReadFiles(helpFiles));

            foreach (var dumpFile in tempDirectory.EnumerateFiles("*.dump")) {
                yield return new AnnotationsByAssembly(Path.GetFileNameWithoutExtension(dumpFile.Name), GetAnnotations(dumpFile));
            }

            tempDirectory.Delete(true);
        }

        private ICollection<Annotation> GetAnnotations(FileInfo dumpFile) {
            var lines = File.ReadAllLines(dumpFile.FullName).Distinct();
            var annotations = new List<Annotation>();
            foreach (var line in lines) {
                var parts = line.Split('#');
                annotations.Add(new Annotation(parts[0], ThreadSafeConstructorXmlId, parts[1]));
            }

            return annotations;
        }

        public void AggregateByAssembly(DirectoryInfo tempDirectory, IEnumerable<TypeDescription> entries) {
            tempDirectory.Create();

            File.Delete(Path.Combine(tempDirectory.FullName, NonParsedFileName));
            var writers = new Dictionary<string, TextWriter>();
            try {
                foreach (var description in entries) {
                    WriteRaw(tempDirectory, description, writers);
                }
            }
            finally {
                foreach (var writer in writers.Values) {
                    writer.Dispose();
                }
            }
        }

        private void WriteRaw(DirectoryInfo directory, TypeDescription description, IDictionary<string, TextWriter> writers) {
            if (description.ThreadSafety == TypeThreadSafety.NotSafe || description.ThreadSafety == TypeThreadSafety.NotFound)
                return;

            if (description.ThreadSafety == TypeThreadSafety.NotParsed) {
                WriteNotParsed(directory, description);
                return;
            }

            foreach (var assemblyName in description.AssemblyNames) {
                var writer = writers.GetOrAdd(assemblyName, _ => new StreamWriter(Path.Combine(directory.FullName, assemblyName + ".dump"), true));
                writer.Write(description.Id);
                writer.Write("#");
                writer.WriteLine(description.ThreadSafety.ToString());
            }
        }

        private void WriteNotParsed(DirectoryInfo directory, TypeDescription description) {
            var filePath = Path.Combine(directory.FullName, NonParsedFileName);
            using (var writer = new StreamWriter(filePath, true)) {
                writer.WriteLine(description.Id);
                writer.WriteLine(description.ThreadSafetyText);
                writer.WriteLine();
            }
        }
    }
}
