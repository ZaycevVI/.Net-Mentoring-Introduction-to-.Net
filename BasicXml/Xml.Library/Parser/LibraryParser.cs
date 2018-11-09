using System.Xml.Linq;
using Xml.Library.Converter;
using Xml.Library.Converter.Interface;
using Xml.Library.Entity;
using Xml.Library.Entity.Base;
using Xml.Library.Parser.Interface;

namespace Xml.Library.Parser
{
    public class LibraryParser : IEntityParser<EntityBase>
    {
        private readonly IConverter<Book> _bookConverter;
        private readonly IConverter<Newspaper> _newspaperConverter;
        private readonly IConverter<Patent> _patentConverter;

        public LibraryParser()
        {
            _bookConverter = new BookConverter();
            _newspaperConverter = new NewspaperConverter();
            _patentConverter = new PatentConverter();
        }

        public XElement Parse(EntityBase entity)
        {
            var type = entity.GetType();

            if (type == typeof(Book))
                return _bookConverter.ToXmlElement(entity as Book);

            return type == typeof(Newspaper) ? 
                _newspaperConverter.ToXmlElement(entity as Newspaper) 
                : _patentConverter.ToXmlElement(entity as Patent);
        }
    }
}
