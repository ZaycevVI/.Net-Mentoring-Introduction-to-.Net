using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XmlApiSamples
{
    [TestClass]
    public class StreamedAndDOM
    {
        [TestMethod]
        public void XmlReaderAndXmlDocumentSample()
        {
            var ns = "http://epam.com/xml/CD";
            var doc = new XmlDocument();

            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile3));
            reader.ReadToFollowing("CD", ns);
            reader.ReadStartElement();

            while (reader.ReadToNextSibling("song", ns))
            {
                var nestedReader = reader.ReadSubtree();
                var songElement = doc.ReadNode(nestedReader) as XmlElement;

                var titleNode = songElement.ChildNodes.OfType<XmlElement>().First(n => n.Name == "title");

                var min = XmlConvert.ToInt32(songElement.GetElementsByTagName("minutes", ns)[0].InnerText);
                var sec = XmlConvert.ToInt32(songElement.GetElementsByTagName("seconds", ns)[0].InnerText);

                Console.WriteLine("{0} = {1:00}:{2:00}", titleNode.InnerText, min, sec);
            }
        }


        [TestMethod]
        public void XmlReaderAndxLinq()
        {
            var nsString = "http://epam.com/xml/CD";
            var ns = XNamespace.Get(nsString);

            var reader = XmlReader.Create(new StringReader(Properties.Resources.XMLFile3));
            reader.ReadToFollowing("CD", nsString);
            reader.ReadStartElement();

            while (reader.ReadToNextSibling("song", nsString))
            {
                var song = XElement.ReadFrom(reader) as XElement;
                var title = song.Element(ns + "title").Value;
                var min = XmlConvert.ToInt32(song.Element(ns + "length").Element(ns + "minutes").Value);
                var sec = XmlConvert.ToInt32(song.Element(ns + "length").Element(ns + "seconds").Value);

                Console.WriteLine("{0} = {1:00}:{2:00}", title, min, sec);
            }
        }
    }
}
