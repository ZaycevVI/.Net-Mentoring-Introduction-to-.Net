using System;
using System.Globalization;
using System.Xml;
using Xml.Library.Constants;

namespace Xml.Library.Helper
{
    public static class XmlHelper
    {
        public static void CreateNewTemplate(string path, string rootTag, string rootAttribute)
        {
            var settings = new XmlWriterSettings {  Indent = true };

            using (var xmlWriter = XmlWriter.Create(path, settings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(TagName.Library);
                xmlWriter.WriteAttributeString(TagName.Date,
                    DateTime.Now.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();
            }
        }
    }
}