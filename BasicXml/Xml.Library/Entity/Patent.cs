using System;

namespace Xml.Library.Entity
{
    public class Patent : EntityBase
    {
        public string Inventor { get; set; }
        public string Country { get; set; }
        public int RegistrationNumber { get; set; }

        /// <summary>
        /// Optional field
        /// </summary>
        public DateTime? RequestDate { get; set; }
        public DateTime PublishDate { get; set; }
    }
}