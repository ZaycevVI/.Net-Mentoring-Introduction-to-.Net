using Xml.Library.Constants;
using Xml.Library.Entity;

namespace Xml.Library.Resolver.Mapper
{
    public class BookMapper : LiteratureMapper<Book>
    {
        public new void Map(Book entity, string tag, string value)
        {
            base.Map(entity, tag, value);

            switch (tag)
            {
                case TagName.Author:
                    entity.Authors.Add(value);
                    break;
                case TagName.Isbn:
                    entity.Isbn = value;
                    break;
            }
        }
    }
}