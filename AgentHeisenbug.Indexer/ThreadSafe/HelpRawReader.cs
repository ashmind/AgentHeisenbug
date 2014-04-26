using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using AshMind.Extensions;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class HelpRawReader {
        [NotNull]private static readonly XmlNamespaceManager NamespaceManager;
        [NotNull] private readonly ICollection<FileInfo> _files;
        
        static HelpRawReader() {
            NamespaceManager = new XmlNamespaceManager(new NameTable());
            NamespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            NamespaceManager.AddNamespace("mtps", "http://msdn2.microsoft.com/mtps");
        }

        public HelpRawReader([NotNull] ICollection<FileInfo> files) {
            _files = files;
        }

        [NotNull]
        public IEnumerable<TypeHelp> ReadFiles([NotNull] Action<double> reportProgress) {
            return _files.OnAfterEach((_, index) => reportProgress((double)index / _files.Count))
                             .SelectMany(ReadFile);
        }

        [NotNull]
        private IEnumerable<TypeHelp> ReadFile([NotNull] FileInfo file) {
            using (var stream = file.OpenRead())
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read)) {
                // ReSharper disable once AssignNullToNotNullAttribute
                // ReSharper disable PossibleNullReferenceException
                var results = archive.Entries.Where(e => e.Name.EndsWith(".htm"))
                                             .Select(ReadEntryText)
                                             .Select(ParseDescription)
                                             .Where(d => d != null);
                // ReSharper restore PossibleNullReferenceException

                foreach (var result in results) {
                    yield return result;
                }
            }
        }

        [NotNull]
        private string ReadEntryText([NotNull] ZipArchiveEntry entry) {
            using (var stream = entry.Open())
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        [CanBeNull]
        private TypeHelp ParseDescription([NotNull] string xmlString) {
            var xml = new XPathDocument(new StringReader(xmlString)).CreateNavigator();
            var id = (string)xml.Evaluate("string(//xhtml:meta[@name='Microsoft.Help.Id']/@content)", NamespaceManager);
            if (id == null || !id.StartsWith("T:"))
                return null;

            var threadSafetyText = GetThreadSafetyText(xml);
            return new TypeHelp(id, GetAssemblyNames(xml), GetThreadSafety(threadSafetyText), threadSafetyText);
        }

        [NotNull]
        private string[] GetAssemblyNames([NotNull] XPathNavigator xml) {
            var singleName = (string)xml.Evaluate("string(//xhtml:strong[.='Assembly:']/following-sibling::*[1])", NamespaceManager);
            if (!string.IsNullOrEmpty(singleName))
                return new[] {singleName};

            var parentResult = xml.Select("//xhtml:strong[.='Assemblies:']/parent::*", NamespaceManager);
            if (!parentResult.MoveNext())
                throw new Exception("Could not find assembly names.");

            // ReSharper disable once PossibleNullReferenceException
            var siblings = parentResult.Current.SelectChildren(XPathNodeType.Element);

            var assemblyNameSpans = siblings
                // ReSharper disable PossibleNullReferenceException
                .Cast<XPathNavigator>()
                .SkipWhile(x => x.LocalName != "strong" || x.Value != "Assemblies:")
                .Skip(1)
                .TakeWhile(x => x.LocalName == "br" || x.LocalName == "span")
                .Where(x => x.LocalName == "span");
                // ReSharper restore PossibleNullReferenceException

            // ReSharper disable once PossibleNullReferenceException
            return assemblyNameSpans.Select(x => x.Value).ToArray();
        }

        [CanBeNull]
        private string GetThreadSafetyText([NotNull] XPathNavigator xml) {
            var text = (string)xml.Evaluate("string(//mtps:CollapsibleArea[@Title='Thread Safety'])", NamespaceManager);
            if (text == null)
                return null;

            return Regex.Replace(text, @"\s+", " ");
        }

        private TypeThreadSafety GetThreadSafety(string text) {
            if (string.IsNullOrEmpty(text))
                return TypeThreadSafety.NotFound;
            
            if (Regex.IsMatch(text, @"except(?!ion)|only|all other|none of the other", RegexOptions.IgnoreCase))
                return TypeThreadSafety.NotParsed;

            if (Regex.IsMatch(text, @"instances of [^\.]+ are thread safe\.", RegexOptions.IgnoreCase))
                return TypeThreadSafety.Instance;

            if (Regex.IsMatch(text, @"(all( public( and protected)?)?|the) (members|methods)[^\.]+are thread[ \-]?safe|(type|class) is[^\.,]+thread[ \-]?safe\.", RegexOptions.IgnoreCase))
                return TypeThreadSafety.All;

            if (Regex.IsMatch(text, @"static[^,\.]* (members|methods) (of|on)[^,\.]* are( all)? thread[ \-]?safe.", RegexOptions.IgnoreCase))
                return TypeThreadSafety.Static;

            if (Regex.IsMatch(text, @"members are not thread[ \-]?safe|you must synchronize any write operations", RegexOptions.IgnoreCase))
                return TypeThreadSafety.NotSafe;

            return TypeThreadSafety.NotParsed;
        }
    }
}
