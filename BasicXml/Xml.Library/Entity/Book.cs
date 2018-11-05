using System;
using System.Collections;
using System.Collections.Generic;

namespace Xml.Library.Entity
{
    public class Book : Literature
    {
        public List<string> Authors { get; set; } = new List<string>();
        public string Isbn { get; set; }
    }
}