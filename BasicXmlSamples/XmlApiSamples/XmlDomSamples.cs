using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;

namespace XmlApiSamples
{
    [TestClass]
    public class XmlDomSamples
    {
        [TestMethod]
        public void CreateXmlStructure()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement name = doc.CreateElement("name");
            name.InnerText = "Patrick Hines";

            XmlElement phone1 = doc.CreateElement("phone");
            phone1.SetAttribute("type", "home");
            phone1.InnerText = "206-555-0144";

            XmlElement phone2 = doc.CreateElement("phone");
            phone2.SetAttribute("type", "work");
            phone2.InnerText = "425-555-0145";

            XmlElement street1 = doc.CreateElement("street1");
            street1.InnerText = "123 Main St";

            XmlElement city = doc.CreateElement("city");
            city.InnerText = "Mercer Island";

            XmlElement state = doc.CreateElement("state");
            state.InnerText = "WA";

            XmlElement postal = doc.CreateElement("postal");
            postal.InnerText = "68042";

            XmlElement address = doc.CreateElement("address");
            address.AppendChild(street1);
            address.AppendChild(city);
            address.AppendChild(state);
            address.AppendChild(postal);

            XmlElement contact = doc.CreateElement("contact");
            contact.AppendChild(name);
            contact.AppendChild(phone1);
            contact.AppendChild(phone2);
            contact.AppendChild(address);

            XmlElement contacts = doc.CreateElement("contacts");
            contacts.AppendChild(contact);
            doc.AppendChild(contacts);

            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, 
                new XmlWriterSettings { Indent = true });

            doc.WriteTo(xmlWriter);
            xmlWriter.Close();

            Console.WriteLine(stringWriter);
        }

        [TestMethod]
        public void ReadAndSave()
        {
            var doc = new XmlDocument();
            
            // Direct document loading
            doc.Load(@"Contacts.xml");
            
            // Parse document or fragment
            doc.LoadXml("<root><a>Text</a></root>");

            // Read document fragment from XmlReader
            var reader = XmlReader.Create(
                new FileStream(@"Contacts.xml", FileMode.Open));
            reader.ReadToFollowing("contacts");
            var node = doc.ReadNode(reader);

            // Direct Save to file
            doc.Save(@"Contacts1.xml");

            // Save through XmlWriter
            var writer = XmlWriter.Create(
                new FileStream(@"Contacts2.xml", FileMode.Create),
                new XmlWriterSettings { Indent = true });
            doc.WriteTo(writer);
            writer.Close();
        }
    }
}
