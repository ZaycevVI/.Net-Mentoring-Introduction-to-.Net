using System;

namespace Xml.Library.Entity.Base
{
    public abstract class Literature : EntityBase
    {
        public string City { get; set; }
        public string PublishingHouse { get; set; }
        public DateTime PublishDate { get; set; }
    }
}