using Xml.Library.Entity;
using Xml.Library.Entity.Base;
using Xml.Library.Resolver.Mapper;

namespace Xml.Library.Resolver
{
    public class PropertyResolver : IPropertyResolver<EntityBase>
    {
        private readonly BookMapper _bookMapper;
        private readonly PatentMapper _patentMapper;
        private readonly NewspaperMapper _newspaperMapper;

        public PropertyResolver()
        {
            _bookMapper = new BookMapper();
            _newspaperMapper = new NewspaperMapper();
            _patentMapper = new PatentMapper();
        }

        public void Map(EntityBase entity, string tag, string value)
        {
            if(entity.GetType() == typeof(Book))
                _bookMapper.Map(entity as Book, tag, value);

            else if(entity.GetType() == typeof(Patent))
                _patentMapper.Map(entity as Patent, tag, value);

            else
                _newspaperMapper.Map(entity as Newspaper, tag, value);
        }
    }
}