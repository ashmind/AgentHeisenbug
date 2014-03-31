using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgentHeisenbug.Indexer.ThreadSafe;

namespace AgentHeisenbug.Indexer {
    public static class Program {
        public static void Main(params string[] args) {
            var inputDirectory = new DirectoryInfo(args[0]);
            var outputDirectory = new DirectoryInfo(args[1]);
            var tempDirectory = new DirectoryInfo(Path.Combine(outputDirectory.FullName, "#temp"));

            var annotations = new HelpAnnotationProvider(new HelpRawReader())
                                    .GetAnnotationsByAssembly(inputDirectory.EnumerateFiles("*NET_FRAMEWORK_45*.mshc"), tempDirectory);
            //types = Buffer(types, 200);

            new AnnotationWriter().WriteAll(outputDirectory, annotations);
            
            Console.WriteLine("Completed");
            Console.ReadKey();
        }

        private static IEnumerable<T> Buffer<T>(IEnumerable<T> enumerable, int count) {
            var index = 0;
            var list = new List<T>();
            using (var enumerator = enumerable.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    list.Add(enumerator.Current);
                    index += 1;

                    while (index < count && enumerator.MoveNext()) {
                        list.Add(enumerator.Current);
                        index += 1;
                    }
                    
                    foreach (var item in list) {
                        yield return item;
                    }
                    index = 0;
                }
            }
        }
    }
}
