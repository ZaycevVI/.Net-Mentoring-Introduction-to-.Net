using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BasicSerialization.Entity
{
    [XmlRoot("catalog", Namespace = "http://library.by/catalog")]
    public class Catalog
    {
        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute("date")]
        public string DateString
        {
            get => Date.ToString("yyyy-MM-dd");
            set => Date = DateTime.Parse(value);
        }

        [XmlElement("book")]
        public List<Book> Books { get; set; }
    }
}
