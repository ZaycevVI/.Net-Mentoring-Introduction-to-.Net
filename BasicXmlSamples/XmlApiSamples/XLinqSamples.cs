using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace XmlApiSamples
{
    [TestClass]
    public class XLinqSamples
    {
        [TestMethod]
        public void CreateXmlStructure()
        {
            var contacts =
                new XElement("contacts",
                    new XElement("contact",
                        new XElement("name", "Patrick Hines"),
                        new XElement("phone", "206-555-0144",
                            new XAttribute("type", "home")),
                        new XElement("phone", "425-555-0145",
                            new XAttribute("type", "work")),
                        new XElement("address",
                            new XElement("street1", "123 Main St"),
                            new XElement("city", "Mercer Island"),
                            new XElement("state", "WA"),
                            new XElement("postal", "68042"))
                        )
                );

            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter,
                new XmlWriterSettings { Indent = true });

            contacts.WriteTo(xmlWriter);
            xmlWriter.Close();

            Console.WriteLine(stringWriter);

        }

        [TestMethod]
        public void ReadAndSave()
        {
            // Read full document
            var doc = XElement.Load(@"Contacts.xml");

            // Parse document fragment from string 
            doc = XElement.Parse("<root><a>Text</a></root>");

            // Read subtree from XmlReader
            var reader = XmlReader.Create(new FileStream(@"Contacts.xml", FileMode.Open));
            reader.ReadToFollowing("contacts");
            var node = XDocument.ReadFrom(reader);
            
            // Save to file directly
            doc.Save(@"Contacts1.xml");

            // Save through XmlWriter
            var writer = XmlWriter.Create(
                new FileStream(@"Contacts2.xml", FileMode.Create), new XmlWriterSettings { Indent = true });
            doc.WriteTo(writer);
            writer.Close();

        }
    }
}
