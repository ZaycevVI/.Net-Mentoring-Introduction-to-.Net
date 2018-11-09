using System.Xml.Linq;
using Xml.Library.Entity.Base;

namespace Xml.Library.Parser.Interface
{
    public interface IEntityParser<in T> where T : EntityBase
    {
        XElement Parse(T entity);
    }
}