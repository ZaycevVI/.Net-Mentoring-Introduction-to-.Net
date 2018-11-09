using System.Globalization;
using System.Xml.Linq;
using Xml.Library.Constants;
using Xml.Library.Converter.Interface;
using Xml.Library.Entity;

namespace Xml.Library.Converter
{
    public class PatentConverter : IConverter<Patent>
    {
        public XElement ToXmlElement(Patent patent)
        {
            return new XElement(TagName.Patent,
                new XElement(TagName.Name, patent.Name),
                new XElement(TagName.Inventor, patent.Inventor),
                new XElement(TagName.Country, patent.Country),
                new XElement(TagName.RegistrationNumber, patent.RegistrationNumber),
                new XElement(TagName.RequestDate, patent.RequestDate?.ToString(CultureInfo.InvariantCulture)),
                new XElement(TagName.PublishDate, patent.PublishDate.ToString(CultureInfo.InvariantCulture)),
                new XElement(TagName.PageAmount, patent.PageAmount),
                new XElement(TagName.Note, patent.Note));
        }
    }
}