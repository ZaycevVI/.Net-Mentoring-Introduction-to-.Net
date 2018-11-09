using System.Globalization;
using System.Xml.Linq;
using Xml.Library.Constants;
using Xml.Library.Converter.Interface;
using Xml.Library.Entity;

namespace Xml.Library.Converter
{
    public class NewspaperConverter : IConverter<Newspaper>
    {
        public XElement ToXmlElement(Newspaper newspaperConverter)
        {
            return new XElement(TagName.Newspaper,
                new XElement(TagName.Name, newspaperConverter.Name),
                new XElement(TagName.City, newspaperConverter.City),
                new XElement(TagName.PublishHouse, newspaperConverter.PublishingHouse),
                new XElement(TagName.PublishDate, newspaperConverter.PublishDate.ToString(CultureInfo.InvariantCulture)),
                new XElement(TagName.PageAmount, newspaperConverter.PageAmount),
                new XElement(TagName.Note, newspaperConverter.Note),
                new XElement(TagName.Number, newspaperConverter.Number),
                new XElement(TagName.Date, newspaperConverter.Date.ToString(CultureInfo.InvariantCulture)),
                new XElement(TagName.Issn, newspaperConverter.Issn));
        }
    }
}