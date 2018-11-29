using System;
using System.Xml.Serialization;

namespace BasicSerialization.Entity
{
    public class Book
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("isbn")]
        public string Isbn { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        public Genre Genre { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlElement("publish_date")]
        public string PublishDateString
        {
            get => PublishDate.ToString("yyyy-MM-dd");
            set => PublishDate = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime PublishDate{ get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("registration_date")]
        public string RegistrationDateString
        {
            get => RegistrationDate.ToString("yyyy-MM-dd");
            set => RegistrationDate = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime RegistrationDate { get; set; }

        public bool ShouldSerializeIsbn()
        {
            return !string.IsNullOrEmpty(Isbn);
        }
    }
}