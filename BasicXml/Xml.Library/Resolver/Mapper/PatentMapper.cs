using System;
using System.Globalization;
using System.Xml;
using Xml.Library.Constants;
using Xml.Library.Entity;

namespace Xml.Library.Resolver.Mapper
{
    public class PatentMapper : Mapper<Patent>
    {
        public override void Map(Patent entity, string tag, string value)
        {
            base.Map(entity, tag, value);

            switch (tag)
            {
                case TagName.Inventor:
                    entity.Inventor = value;
                    break;
                case TagName.Country:
                    entity.Country = value;
                    break;
                case TagName.RegistrationNumber:
                    entity.RegistrationNumber = XmlConvert.ToInt32(value);
                    break;
                case TagName.RequestDate:
                    entity.RequestDate = string.IsNullOrEmpty(value) ?
                        null as DateTime? : DateTime.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case TagName.PublishDate:
                    entity.PublishDate = DateTime.Parse(value, CultureInfo.InvariantCulture);
                    break;
            }
        }
    }
}