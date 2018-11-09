using Xml.Library.Entity.Base;

namespace Xml.Library.Resolver
{
    public interface IPropertyResolver<in T> where T : EntityBase
    {
        void Map(T entity, string tag, string value);
    }
}