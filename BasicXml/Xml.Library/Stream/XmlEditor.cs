using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Xml.Library.Constants;
using Xml.Library.Entity.Base;
using Xml.Library.Factory;
using Xml.Library.Helper;
using Xml.Library.Parser;
using Xml.Library.Parser.Interface;
using Xml.Library.Resolver;

namespace Xml.Library.Stream
{
    public class XmlEditor
    {
        private readonly string _path;
        private readonly IEntityParser<EntityBase> _parser;
        private readonly IPropertyResolver<EntityBase> _propertyResolver;
        private readonly List<string> _baseTags;
        private readonly List<string> _collectionTag;

        public XmlEditor(string path, IEntityParser<EntityBase> parser, IPropertyResolver<EntityBase> propertyResolver)
        {
            _parser = parser;
            _propertyResolver = propertyResolver;
            _path = path;
            _baseTags = new List<string> { TagName.Book, TagName.Newspaper, TagName.Patent };
            _collectionTag = new List<string> { TagName.Authors };
        }

        public XmlEditor(string path)
        {
            _path = path;
            _baseTags = new List<string> { TagName.Book, TagName.Newspaper, TagName.Patent };
            _propertyResolver = new PropertyResolver();
            _parser = new LibraryParser();
            _collectionTag = new List<string> { TagName.Authors };

            if (!File.Exists(path))
                CreateDocumentTemplate();
        }

        public void Write(List<EntityBase> entities)
        {
            if (!File.Exists(_path))
                CreateDocumentTemplate();

            if (entities == null) return;

            var xDocument = XDocument.Load(_path);
            var root = xDocument.Element(TagName.Library);

            if (root.LastNode == null)
                root.Add(Parse(entities));

            else
                root.LastNode.AddAfterSelf(Parse(entities));

            xDocument.Save(_path);
        }

        public List<EntityBase> Read()
        {
            var entites = new List<EntityBase>();

            if (!File.Exists(_path))
                return entites;

            var settings = GetSettings();
            
            using (var xmlReader = XmlReader.Create(
                new FileStream(_path, FileMode.OpenOrCreate), settings))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element) continue;

                    if (IsBaseTag(xmlReader.Name))
                    {
                        var baseTag = xmlReader.Name;
                        entites.Add(EntityFactory.CreateEntity(baseTag));
                        continue;
                    }

                    var innerTag = xmlReader.Name;

                    if (!entites.Any() || IsCollectionTag(innerTag)) continue;

                    xmlReader.Read();
                    _propertyResolver.Map(entites.Last(), innerTag, xmlReader.Value);
                }
            }

            return entites;
        }

        private IEnumerable<XElement> Parse(IEnumerable<EntityBase> entities)
        {
            return entities.Select(entity => _parser.Parse(entity));
        }

        private void CreateDocumentTemplate()
        {
            XmlHelper.CreateNewTemplate(_path, TagName.Library, TagName.Date);
        }

        private XmlReaderSettings GetSettings()
        {
            return new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
        }

        private bool IsBaseTag(string tag)
        {
            return _baseTags.Contains(tag);
        }

        private bool IsCollectionTag(string tag)
        {
            return _collectionTag.Contains(tag);
        }
    }
}
