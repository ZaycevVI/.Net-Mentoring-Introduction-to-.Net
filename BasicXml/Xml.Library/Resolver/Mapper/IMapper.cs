using Xml.Library.Entity.Base;

namespace Xml.Library.Resolver.Mapper
{
    public interface IMapper<T>
        where T : EntityBase
    {
        void Map(T entity, string tag, string value);
    }
}