using System;
using System.Globalization;
using System.Xml;
using Xml.Library.Constants;
using Xml.Library.Entity;

namespace Xml.Library.Resolver.Mapper
{
    public class NewspaperMapper : LiteratureMapper<Newspaper>
    {
        public new void Map(Newspaper entity, string tag, string value)
        {
            base.Map(entity, tag, value);

            switch (tag)
            {
                case TagName.Number:
                    entity.Number = Convert.ToUInt32(value);
                    break;
                case TagName.Date:
                    entity.Date = DateTime.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case TagName.Issn:
                    entity.Issn = value;
                    break;
            }
        }
    }
}