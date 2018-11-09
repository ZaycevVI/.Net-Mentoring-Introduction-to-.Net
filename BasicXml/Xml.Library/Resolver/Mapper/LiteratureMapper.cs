using System;
using System.Globalization;
using Xml.Library.Constants;
using Xml.Library.Entity.Base;

namespace Xml.Library.Resolver.Mapper
{
    public abstract class LiteratureMapper<T> : Mapper<T> where T : Literature
    {
        public override void Map(T entity, string tag, string value)
        {
            base.Map(entity, tag, value);

            switch (tag)
            {
                case TagName.City:
                    entity.City = value;
                    break;
                case TagName.PublishHouse:
                    entity.PublishingHouse = value;
                    break;
                case TagName.PublishDate:
                    entity.PublishDate = DateTime.Parse(value, CultureInfo.InvariantCulture);
                    break;
            }
        }
    }
}