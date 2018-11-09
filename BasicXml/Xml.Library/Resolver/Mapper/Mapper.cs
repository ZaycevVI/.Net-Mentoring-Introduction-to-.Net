using System;
using Xml.Library.Constants;
using Xml.Library.Entity.Base;

namespace Xml.Library.Resolver.Mapper
{
    public abstract class Mapper<T> : IMapper<T> where T : EntityBase
    {
        public virtual void Map(T entity, string tag, string value)
        {
            switch (tag)
            {
                case TagName.Name:
                    entity.Name = value;
                    break;
                case TagName.PageAmount:
                    entity.PageAmount = Convert.ToInt32(value);
                    break;
                case TagName.Note:
                    entity.Note = value;
                    break;
            }
        }
    }
}
