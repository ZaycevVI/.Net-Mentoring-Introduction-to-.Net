using System.Collections.Generic;
using Xml.Library.Entity.Base;

namespace Xml.Library.Stream.Interface
{
    public interface IStreamEditor
    {
        void Write(List<EntityBase> entities);
        List<EntityBase> Read();
    }
}