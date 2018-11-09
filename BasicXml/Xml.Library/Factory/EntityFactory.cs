using Xml.Library.Constants;
using Xml.Library.Entity;
using Xml.Library.Entity.Base;

namespace Xml.Library.Factory
{
    public class EntityFactory
    {
        public static EntityBase CreateEntity(string tagName)
        {
            EntityBase entity = null;

            switch (tagName)
            {
                case TagName.Newspaper:
                    entity = new Newspaper();
                    break;
                case TagName.Book:
                    entity = new Book();
                    break;
                case TagName.Patent:
                    entity = new Patent();
                    break;
            }

            return entity;
        }
    }
}
