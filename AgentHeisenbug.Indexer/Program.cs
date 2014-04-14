using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AgentHeisenbug.Indexer.ReadOnly;
using AgentHeisenbug.Indexer.ThreadSafe;

namespace AgentHeisenbug.Indexer {
    public static class Program {
        public static void Main(params string[] args) {
            Console.Title = Assembly.GetExecutingAssembly().GetName().Name;
            FluentConsole.White.Line("Started");
            
            var outputDirectory = new DirectoryInfo(args[0]);
            var msdnDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["indexer:MsdnPath"]);
            var frameworkDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["indexer:FrameworkPath"]);
            var assemblyFilter = ConfigurationManager.AppSettings["indexer:AssemblyFilter"];

            var helpParsingFailures = new List<TypeHelp>();
            var providers = new IAnnotationProvider[] {
                new ReadOnlyAnnotationProvider(frameworkDirectory),
                new HelpAnnotationProvider(new HelpRawReader(msdnDirectory.GetFiles("*NET_FRAMEWORK_45*.mshc")), helpParsingFailures.Add)
            };

            using (ConsoleMultiProgressReporter.Start(providers)) {
                var annotations = GetAllAnnotations(providers, assemblyFilter);
                new AnnotationWriter().WriteAll(outputDirectory, annotations);
            }

            WriteHelpParsingFailures(outputDirectory, helpParsingFailures);
        }

        private static IEnumerable<AnnotationsByAssembly> GetAllAnnotations(IEnumerable<IAnnotationProvider> providers, string assemblyFilter) {
            var annotations = providers.AsParallel()
                                       .SelectMany(p => p.GetAnnotationsByAssembly(
                                           n => Regex.IsMatch(n, assemblyFilter),
                                           progress => ConsoleMultiProgressReporter.Progress(p, progress)
                                       ))
                                       .GroupBy(a => a.AssemblyName)
                                       .Select(MergeByAssembly);

            return annotations.AsSequential();
        }

        private static AnnotationsByAssembly MergeByAssembly(IGrouping<string, AnnotationsByAssembly> group) {
            return new AnnotationsByAssembly(group.Key, group.SelectMany(g => g.Annotations).GroupBy(a => a.MemberXmlId).Select(MergeByMember).ToArray());
        }

        private static AnnotationsByMember MergeByMember(IGrouping<string, AnnotationsByMember> group) {
            return new AnnotationsByMember(group.Key, group.SelectMany(g => g.Annotations).ToArray());
        }

        private static void WriteHelpParsingFailures(DirectoryInfo outputDirectory, IEnumerable<TypeHelp> failures) {
            var filePath = Path.Combine(outputDirectory.FullName, "HelpParsingFailures.txt");
            using (var writer = new StreamWriter(filePath)) {
                foreach (var failure in failures.OrderBy(f => f.Id)) {
                    writer.WriteLine(failure.Id);
                    writer.WriteLine(failure.ThreadSafetyText);
                    writer.WriteLine();
                }
            }
        }
    }
}
