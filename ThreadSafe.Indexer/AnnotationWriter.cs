using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThreadSafe.Indexer {
    public class AnnotationWriter {
        private const string NonParsedFileName = "NotParsed.Generated.txt";

        public void WriteAll(DirectoryInfo directory, IEnumerable<TypeDescription> descriptions) {
            directory.Create();

            File.Delete(Path.Combine(directory.FullName, NonParsedFileName));
            foreach (var description in descriptions) {
                WriteRaw(directory, description);
            }

            foreach (var file in directory.GetFiles("*.dump")) {
                Process(file);
            }
        }

        private void WriteRaw(DirectoryInfo directory, TypeDescription description) {
            if (description.ThreadSafety == TypeThreadSafety.NotSafe || description.ThreadSafety == TypeThreadSafety.NotFound)
                return;

            if (description.ThreadSafety == TypeThreadSafety.NotParsed) {
                WriteNotParsed(directory, description);
                return;
            }

            foreach (var assemblyName in description.AssemblyNames) {
                var filePath = Path.Combine(directory.FullName, assemblyName + ".Generated.dump");
                using (var writer = new StreamWriter(filePath, true)) {
                    writer.Write(description.Id);
                    writer.Write("#");
                    writer.WriteLine(description.ThreadSafety.ToString());
                }
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

        private void Process(FileInfo dumpFile) {
            var lines = File.ReadAllLines(dumpFile.FullName).Distinct().OrderBy(l => l);
            var separator = new[] { '#' };

            var resultFilePath = dumpFile.FullName.Replace(".dump", ".json");
            using (var writer = new StreamWriter(resultFilePath)) {
                writer.WriteLine("{");

                var index = 0;
                foreach (var line in lines) {
                    var parts = line.Split(separator, 2);
                    if (index > 0)
                        writer.WriteLine(",");
                    writer.Write("  \"");
                    writer.Write(parts[0]);
                    writer.Write("\": \"");
                    writer.Write(parts[1]);
                    writer.Write("\"");

                    index += 1;
                }

                writer.WriteLine();
                writer.WriteLine("}");
            }

            dumpFile.Delete();
        }
    }
}
