using System.Collections.Generic;
using Xml.Library.Entity.Base;

namespace Xml.Library.Entity
{
    public class Book : Literature
    {
        public List<string> Authors { get; set; } = new List<string>();
        public string Isbn { get; set; }
    }
}