using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Xml.Library.Constants;
using Xml.Library.Converter.Interface;
using Xml.Library.Entity;

namespace Xml.Library.Converter
{
    public class BookConverter : IConverter<Book>
    {
        public XElement ToXmlElement(Book entity)
        {
            var authors = entity.Authors.Select(author => new XElement(TagName.Author, author));

            return new XElement(TagName.Book,
                new XElement(TagName.Name, entity.Name),
                new XElement(TagName.Authors, authors),
                new XElement(TagName.City, entity.City),
                new XElement(TagName.PublishHouse, entity.PublishingHouse),
                new XElement(TagName.PublishDate, entity.PublishDate.ToString(CultureInfo.InvariantCulture)),
                new XElement(TagName.PageAmount, entity.PageAmount.ToString()),
                new XElement(TagName.Note, entity.Note),
                new XElement(TagName.Isbn, entity.Isbn));
        }
    }
}