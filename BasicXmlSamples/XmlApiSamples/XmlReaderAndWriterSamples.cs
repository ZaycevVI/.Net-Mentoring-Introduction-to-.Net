using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;

namespace XmlApiSamples
{
    [TestClass]
    public class XmlReaderAndWriterSamples
    {
        public void SimpleRead()
        {
            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile1));

            while (reader.Read())
            {
                Console.WriteLine("{0} {1} {2}", reader.NodeType, reader.Name,
                    reader.NodeType == XmlNodeType.Text ? '"' + reader.Value + '"' : "");
            }
        }

        [TestMethod]
        public void SimpleRead2()
        {
            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile1));

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.None:
                        break;
                    case XmlNodeType.Element:
                        Console.WriteLine("Element '{0}'", reader.Name);
                        break;
                    case XmlNodeType.Attribute:
                        Console.WriteLine("Attribute {0} = '{1}'", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine("Text '{0}'", reader.Value);
                        break;
                    case XmlNodeType.CDATA:
                        Console.WriteLine("CDATA '{0}'", reader.Value);
                        break;
                    case XmlNodeType.Comment:
                        Console.WriteLine("Comment '{0}'", reader.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        Console.WriteLine("Processing Instruction '{0}' with '{1}'", reader.Name, reader.Value);
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        Console.WriteLine("{0} with length '{1}'", reader.NodeType, reader.Value.Length);
                        break;

                    default:
                        Console.WriteLine("{0}", reader.NodeType);
                        break;
                }
            }
        }

        [TestMethod]
        public void ReadWhiteSpace()
        {
            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile2), 
                new XmlReaderSettings { IgnoreWhitespace = true });
            while (reader.ReadToFollowing("space"))
            {
                reader.Read();
                Console.WriteLine("{0} {1}", reader.NodeType, reader.Value.Length);
            }
        }

        [TestMethod]
        public void ReadStructure()
        {
            var ns = "http://epam.com/xml/CD";

            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile3));

            reader.ReadToFollowing("CD", ns);
            reader.ReadToDescendant("title", ns);

            Console.WriteLine(reader.ReadElementContentAsString());

            while (reader.ReadToNextSibling("song", ns))
            {
                reader.ReadToDescendant("title", ns);
                Console.WriteLine(reader.ReadElementContentAsString());

                reader.ReadToNextSibling("length", ns);
                reader.ReadToDescendant("minutes", ns);
                var min = reader.ReadElementContentAsInt();
                reader.ReadToNextSibling("seconds", ns);
                var sec = reader.ReadElementContentAsInt();
                Console.WriteLine("{0:00}:{1:00}", min, sec);

                reader.ReadEndElement(); // </length>

                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                        reader.Skip();
                }
            }
        }


        [TestMethod]
        public void WriteStructure()
        {
            var ns = "http://epam.com/xml/CD";

            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, 
                new XmlWriterSettings { Indent = true });

            xmlWriter.WriteStartElement("CD", ns);
            xmlWriter.WriteAttributeString("serial", "B6B41B");
            xmlWriter.WriteAttributeString("disc-length", "36:55");

            xmlWriter.WriteStartElement("song", ns);
            xmlWriter.WriteStartElement("length", ns);
            xmlWriter.WriteStartElement("minutes", ns);
            xmlWriter.WriteValue(3);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("seconds", ns);
            xmlWriter.WriteValue(33);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.Close();
            Console.WriteLine(stringWriter.ToString());
        }
    }
}
