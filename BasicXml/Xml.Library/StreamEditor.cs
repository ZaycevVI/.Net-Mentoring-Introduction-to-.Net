using System.Collections.Generic;
using System.IO;
using System.Xml;
using Xml.Library.Entity;

namespace Xml.Library
{
    public abstract class StreamEditor
    {
        protected readonly Stream Stream;

        protected StreamEditor(Stream stream)
        {
            Stream = stream;
        }

        public abstract void Write(List<EntityBase> entities);

        public abstract List<EntityBase> Read();
    }
}