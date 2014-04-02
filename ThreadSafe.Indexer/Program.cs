using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AgentHeisenbug.Indexer.ReadOnly;
using AgentHeisenbug.Indexer.ThreadSafe;

namespace AgentHeisenbug.Indexer {
    public static class Program {
        public static void Main(params string[] args) {
            var outputDirectory = new DirectoryInfo(args[0]);
            var msdnDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["indexer:MsdnPath"]);
            var frameworkDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["indexer:FrameworkPath"]);
            var assemblyFilter = ConfigurationManager.AppSettings["indexer:AssemblyFilter"];

            var providers = new IAnnotationProvider[] {
                new ReadOnlyAnnotationProvider(frameworkDirectory),
                new HelpAnnotationProvider(new HelpRawReader(msdnDirectory.EnumerateFiles("*NET_FRAMEWORK_45*.mshc")))
            };

            var annotations = providers.AsParallel()
                                       .SelectMany(p => p.GetAnnotationsByAssembly(n => Regex.IsMatch(n, assemblyFilter)))
                                       .GroupBy(a => a.AssemblyName)
                                       .Select(MergeByAssembly);
            //types = Buffer(types, 200);

            new AnnotationWriter().WriteAll(outputDirectory, annotations.AsSequential());
        }

        private static AnnotationsByAssembly MergeByAssembly(IGrouping<string, AnnotationsByAssembly> group) {
            return new AnnotationsByAssembly(group.Key, group.SelectMany(g => g.Annotations).GroupBy(a => a.MemberXmlId).Select(MergeByMember).ToArray());
        }

        private static AnnotationsByMember MergeByMember(IGrouping<string, AnnotationsByMember> group) {
            return new AnnotationsByMember(group.Key, group.SelectMany(g => g.Annotations).ToArray());
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
