using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace AgentHeisenbug.Indexer.ThreadSafe {
    public class HelpRawReader {
        private static readonly XmlNamespaceManager namespaceManager;

        static HelpRawReader() {
            namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            namespaceManager.AddNamespace("mtps",  "http://msdn2.microsoft.com/mtps");
        }

        public IEnumerable<TypeDescription> ReadFiles(IEnumerable<FileInfo> files) {
            return files.SelectMany(ReadFile);
        }

        private IEnumerable<TypeDescription> ReadFile(FileInfo file) {
            using (var archive = new ZipArchive(file.OpenRead(), ZipArchiveMode.Read)) {
                var results = archive.Entries.Where(e => e.Name.EndsWith(".htm"))
                                             .Select(ReadEntryText)
                                             .AsParallel()
                                             .Select(ParseDescription)
                                             .Where(d => d != null)
                                             .AsSequential();

                foreach (var result in results) {
                    yield return result;
                }
            }
        }

        private string ReadEntryText(ZipArchiveEntry entry) {
            using (var stream = entry.Open())
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        private TypeDescription ParseDescription(string xmlString) {
            var xml = new XPathDocument(new StringReader(xmlString)).CreateNavigator();
            var id = (string)xml.Evaluate("string(//xhtml:meta[@name='Microsoft.Help.Id']/@content)", namespaceManager);
            if (id == null || !id.StartsWith("T:"))
                return null;

            var threadSafetyText = GetThreadSafetyText(xml);
            return new TypeDescription(id, GetAssemblyNames(xml), GetThreadSafety(threadSafetyText), threadSafetyText);
        }

        private string[] GetAssemblyNames(XPathNavigator xml) {
            var singleName = (string)xml.Evaluate("string(//xhtml:strong[.='Assembly:']/following-sibling::*[1])", namespaceManager);
            if (!string.IsNullOrEmpty(singleName))
                return new[] {singleName};

            var parentResult = xml.Select("//xhtml:strong[.='Assemblies:']/parent::*", namespaceManager);
            if (!parentResult.MoveNext())
                return null;

            var siblings = parentResult.Current.SelectChildren(XPathNodeType.Element);
            var s1 = siblings.Cast<XPathNavigator>().SkipWhile(x => x.LocalName != "strong" || x.Value != "Assemblies:");
            var s2 = s1.Skip(1);
            var s3 = s2.TakeWhile(x => x.LocalName == "br" || x.LocalName == "span");
            var s4 = s3.Where(x => x.LocalName == "span");
            return s4.Select(x => x.Value).ToArray();
        }

        private string GetThreadSafetyText(XPathNavigator xml) {
            var text = (string)xml.Evaluate("string(//mtps:CollapsibleArea[@Title='Thread Safety'])", namespaceManager);
            if (text == null)
                return null;

            return Regex.Replace(text, @"\s+", " ");
        }

        private TypeThreadSafety GetThreadSafety(string text) {
            if (string.IsNullOrEmpty(text))
                return TypeThreadSafety.NotFound;
            
            if (Regex.IsMatch(text, @"except|only|all other|none of the other", RegexOptions.IgnoreCase))
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
