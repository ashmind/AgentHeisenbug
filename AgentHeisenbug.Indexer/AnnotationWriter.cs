﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AgentHeisenbug.Indexer {
    public class AnnotationWriter {
        public void WriteAll(DirectoryInfo directory, IEnumerable<AnnotationsByAssembly> annotations) {
            directory.Create();

            foreach (var assembly in annotations) {
                if (assembly.Annotations.Count == 0)
                    continue;

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

        private void WriteAnnotation(XmlWriter writer, AnnotationsByMember member) {
            writer.WriteStartElement("member");
            writer.WriteAttributeString("name", member.MemberXmlId);
            foreach (var attribute in member.Annotations.OrderBy(a => a.AttributeConstructorXmlId)) {
                writer.WriteStartElement("attribute");
                writer.WriteAttributeString("ctor", attribute.AttributeConstructorXmlId);
                foreach (var argument in attribute.AttributeArguments) {
                    writer.WriteElementString("argument", argument);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}