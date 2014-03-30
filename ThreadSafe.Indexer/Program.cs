using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ThreadSafe.Indexer {
    public static class Program {
        public static void Main(params string[] args) {
            var inputDirectory = new DirectoryInfo(args[0]);
            var outputDirectory = new DirectoryInfo(args[1]);

            var types = new HelpReader().ReadDirectory(inputDirectory, f => f.Name.Contains("NET_FRAMEWORK_45"));
            //types = Buffer(types, 200);

            new AnnotationWriter().WriteAll(outputDirectory, types);
            
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
