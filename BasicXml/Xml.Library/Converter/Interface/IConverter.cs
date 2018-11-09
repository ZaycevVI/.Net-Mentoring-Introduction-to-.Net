using System.Xml.Linq;
using Xml.Library.Entity.Base;

namespace Xml.Library.Converter.Interface
{
    public interface IConverter<in T> where T : EntityBase
    {
        XElement ToXmlElement(T entity) ;
    }
}