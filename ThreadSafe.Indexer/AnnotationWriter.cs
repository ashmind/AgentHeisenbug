using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AgentHeisenbug.Indexer {
    public class AnnotationWriter {
        public void WriteAll(DirectoryInfo directory, IEnumerable<AnnotationsByAssembly> annotations) {
            directory.Create();

            foreach (var assembly in annotations) {
                WriteAssembly(directory, assembly);
            }
        }

        private void WriteAssembly(DirectoryInfo directory, AnnotationsByAssembly assembly) {
            var subdirectory = new DirectoryInfo(Path.Combine(directory.FullName, assembly.AssemblyName));
            subdirectory.Create();

            var filePath = Path.Combine(subdirectory.FullName, "Annotations.xml");
            using (var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true })) {
                writer.WriteStartElement("assembly");
                writer.WriteAttributeString("name", assembly.AssemblyName);
                foreach (var annotation in assembly.Annotations.OrderBy(a => a.MemberXmlId)) {
                    WriteAnnotation(writer, annotation);
                }
                writer.WriteEndElement();
            }
        }

        private void WriteAnnotation(XmlWriter writer, Annotation annotation) {
            writer.WriteStartElement("member");
            writer.WriteAttributeString("name", annotation.MemberXmlId);
            writer.WriteStartElement("attribute");
            writer.WriteAttributeString("ctor", annotation.AttributeConstructorXmlId);
            foreach (var argument in annotation.AttributeArguments) {
                writer.WriteElementString("argument", argument);
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
