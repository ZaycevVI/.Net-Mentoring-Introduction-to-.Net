using System;
using Xml.Library.Entity.Base;

namespace Xml.Library.Entity
{
    public class Newspaper : Literature
    {
        /// <summary>
        /// Optional field
        /// </summary>
        public uint? Number { get; set; }
        public DateTime Date { get; set; }
        public string Issn { get; set; }
    }
}