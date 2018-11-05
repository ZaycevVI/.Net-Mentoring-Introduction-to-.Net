using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Xml.Library.Entity;

namespace Xml.Library
{
    public class XmlEditor : StreamEditor, IDisposable
    {
        private readonly string _path;
        private const string LibraryTag = "library";
        private const string NameTag = "name";
        private const string AuthorTag = "author";
        private const string CityTag = "city";
        private const string PublishHouseTag = "publishHouse";
        private const string PublishDateTag = "publishDate";
        private const string PageAmountTag = "pageAmount";
        private const string NoteTag = "note";
        private const string IsbnTag = "isbn";
        private const string BookTag = "book";
        private const string NumberTag = "number";
        private const string DateTag = "date";
        private const string IssnTag = "issn";
        private const string NewspaperTag = "newspaper";
        private const string PatentTag = "patent";
        private const string InventorTag = "inventor";
        private const string CountryTag = "country";
        private const string RegistrationNumberTag = "registrationNumber";
        private const string RequestDateTag = "requestDate";


        public XmlEditor(string path)
            : this(new FileStream(path, FileMode.Create))
        {
        }

        private XmlEditor(FileStream stream)
            : base(stream)
        {
            _path = stream.Name;

            if (!File.Exists(stream.Name))
                CreateNewTemplate();
        }

        public override void Write(List<EntityBase> entities)
        {
            if (!File.Exists(_path))
                CreateNewTemplate();

            var xDocument = XDocument.Load(_path);
            var root = xDocument.Element(LibraryTag);
            root.LastNode.AddAfterSelf(Parse(entities));
        }

        public override List<EntityBase> Read()
        {
            var entites = new List<EntityBase>();

            using (var xmlReader = XmlReader.Create(Stream))
            {
            }

            return entites;
        }

        public void Dispose()
        {
            Stream.Dispose();
        }

        private void CreateNewTemplate()
        {
            var doc = new XmlDocument();
            var parrentTag = doc.CreateElement(LibraryTag);
            parrentTag.SetAttribute(DateTag, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            using (var xmlWriter = XmlWriter.Create(_path))
            {
                doc.WriteTo(xmlWriter);
            }
        }

        private List<XElement> Parse(List<EntityBase> entities)
        {
            var entitiesList = new List<XElement>();

            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case Book book:
                        entitiesList.Add(Parse(book));
                        break;
                    case Newspaper newspaper:
                        entitiesList.Add(Parse(newspaper));
                        break;
                    case Patent patent:
                        entitiesList.Add(Parse(patent));
                        break;
                }
            }

            return entitiesList;
        }

        private XElement Parse(Book book)
        {
            var authors = book.Authors.Select(author => new XElement("author", author));

            return new XElement(BookTag,
                new XElement(NameTag, book.Name),
                new XElement(AuthorTag, authors),
                new XElement(CityTag, book.City),
                new XElement(PublishHouseTag, book.PublishingHouse),
                new XElement(PublishDateTag, book.PublishDate.ToString(CultureInfo.InvariantCulture)),
                new XElement(PageAmountTag, book.PageAmount.ToString()),
                new XElement(NoteTag, book.Note),
                new XElement(IsbnTag, book.Isbn));
        }

        private XElement Parse(Newspaper newspaper)
        {
            return new XElement(NewspaperTag,
                new XElement(NameTag, newspaper.Name),
                new XElement(CityTag, newspaper.City),
                new XElement(PublishHouseTag, newspaper.PublishingHouse),
                new XElement(PublishDateTag, newspaper.PublishDate),
                new XElement(PageAmountTag, newspaper.PageAmount),
                new XElement(NoteTag, newspaper.Note),
                new XElement(NumberTag, newspaper.Number),
                new XElement(DateTag, newspaper.Date),
                new XElement(IssnTag, newspaper.Issn));
        }

        private XElement Parse(Patent patent)
        {
            return new XElement(PatentTag, 
                new XElement(NameTag, patent.Name), 
                new XElement(InventorTag, patent.Inventor), 
                new XElement(CountryTag, patent.Country), 
                new XElement(RegistrationNumberTag, patent.RegistrationNumber), 
                new XElement(RequestDateTag, patent.RequestDate), 
                new XElement(PublishDateTag, patent.PublishDate), 
                new XElement(PageAmountTag, patent.PageAmount), 
                new XElement(NoteTag, patent.Note));
        }
    }
}
