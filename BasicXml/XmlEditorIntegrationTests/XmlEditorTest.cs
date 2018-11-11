using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Xml.Library.Constants;
using Xml.Library.Entity;
using Xml.Library.Entity.Base;
using Xml.Library.Stream;

namespace XmlEditorIntegrationTests
{
    [TestClass]
    public class XmlEditorTest
    {
        private const string NonExistentPath = "NonExistentXml.xml";
        private const string EmptyTemplatePath = "EmptyTemplate.xml";
        private const string BookXmlPath = "Book.xml";
        private const string PatentXmlPath = "Patent.xml";
        private const string NewspaperXmlPath = "Newspaper.xml";
        private const string NewspaperOptionalFieldXmlPath = "NewspaperOptionalField.xml";
        private const string XmlPath = "xmlFile.xml";
        private const string BookEntityPath = "BookEntity.xml";
        private const string NewspaperEntityPath = "NewspaperEntity.xml";
        private const string PatentEntityPath = "PatentEntity.xml";
        private const string LibraryEntityPath = "Library.xml";
        private const string OptionalFieldNumberReadPath = "OptionalFieldNumberRead.xml";
        private const string OptionalFieldNumberWritePath = "OptionalFieldNumberWrite.xml";
        private Book _book;
        private Patent _patent;
        private Newspaper _newspaper;

        [TestInitialize]
        public void TestInitialize()
        {
            _book = new Book
            {
                Name = "bookName",
                Authors = new List<string> { "author" },
                PublishDate = new DateTime(2018, 11, 09, 16, 42, 20),
                PublishingHouse = "aaa",
                Isbn = "bbb",
                PageAmount = 3,
                City = "Minsk",
                Note = "ffff"
            };

            _patent = new Patent
            {
                Name = "patentName",
                PublishDate = new DateTime(2018, 11, 09, 16, 53, 03),
                PageAmount = 1,
                Note = "note",
                Country = "Grodno",
                RegistrationNumber = 123,
                RequestDate = new DateTime(2018, 11, 09, 16, 53, 03),
                Inventor = "Newton"
            };

            _newspaper = new Newspaper
            {
                Name = "newspaperName",
                PublishDate = new DateTime(2018, 11, 09, 15, 46, 53),
                PageAmount = 3,
                Note = "nnote",
                City = "Brest",
                PublishingHouse = "ooo",
                Date = new DateTime(2018, 11, 09, 15, 46, 53),
                Issn = "issn",
                Number = 12
            };
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (File.Exists(NonExistentPath))
                File.Delete(NonExistentPath);

            if (File.Exists(BookXmlPath))
                File.Delete(BookXmlPath);

            if (File.Exists(PatentXmlPath))
                File.Delete(PatentXmlPath);

            if (File.Exists(NewspaperXmlPath))
                File.Delete(NewspaperXmlPath);

            if (File.Exists(XmlPath))
                File.Delete(XmlPath);

            if (File.Exists(NewspaperEntityPath))
                File.Delete(NewspaperEntityPath);

            if (File.Exists(BookEntityPath))
                File.Delete(BookEntityPath);

            if (File.Exists(PatentEntityPath))
                File.Delete(PatentEntityPath);

            if (File.Exists(LibraryEntityPath))
                File.Delete(LibraryEntityPath);

            if (File.Exists(NewspaperOptionalFieldXmlPath))
                File.Delete(NewspaperOptionalFieldXmlPath);

            if (File.Exists(OptionalFieldNumberReadPath))
                File.Delete(OptionalFieldNumberReadPath);

            if (File.Exists(OptionalFieldNumberWritePath))
                File.Delete(OptionalFieldNumberWritePath);
        }

        [TestMethod]
        public void Write_When_File_Doesnt_Exist_Create_Empty_Template()
        {
            // Arrange
            var xmlEditor = new XmlEditor(NonExistentPath);

            // Act
            xmlEditor.Write(null);

            // Assert
            File.Exists(NonExistentPath).ShouldBeTrue();
            var doc = XDocument.Load(NonExistentPath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeFalse();
            doc.Root.Name.ShouldBe(TagName.Library);
        }

        [TestMethod]
        public void Write_When_File_Is_Empty_Template()
        {
            // Arrange
            var xmlEditor = new XmlEditor(EmptyTemplatePath);

            // Act
            xmlEditor.Write(null);

            // Assert
            File.Exists(EmptyTemplatePath).ShouldBeTrue();
            var doc = XDocument.Load(EmptyTemplatePath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeFalse();
            doc.Root.Name.ShouldBe(TagName.Library);
        }

        [TestMethod]
        public void Write_When_Book_Is_Added()
        {
            // Arrange
            var entities = new List<EntityBase>
            {
                _book
            };

            var xmlEditor = new XmlEditor(BookXmlPath);

            // Act
            xmlEditor.Write(entities);

            // Assert
            File.Exists(BookXmlPath).ShouldBeTrue();
            var doc = XDocument.Load(BookXmlPath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeTrue();
            CheckExpectedBookElement(doc.Root.Elements().First());
        }

        [TestMethod]
        public void Write_When_Patent_Is_Added()
        {
            // Arrange
            var entities = new List<EntityBase>
            {
                _patent
            };

            var xmlEditor = new XmlEditor(PatentXmlPath);

            // Act
            xmlEditor.Write(entities);

            // Assert
            File.Exists(PatentXmlPath).ShouldBeTrue();
            var doc = XDocument.Load(PatentXmlPath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeTrue();
            CheckExpectedPatentElement(doc.Root.Elements().First());
        }

        [TestMethod]
        public void Write_When_Newspaper_Is_Added()
        {
            // Arrange
            var entities = new List<EntityBase>
            {
                _newspaper
            };

            var xmlEditor = new XmlEditor(NewspaperXmlPath);

            // Act
            xmlEditor.Write(entities);

            // Assert
            File.Exists(NewspaperXmlPath).ShouldBeTrue();
            var doc = XDocument.Load(NewspaperXmlPath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeTrue();
            CheckExpectedNewspaperElement(doc.Root.Elements().First());
        }

        [TestMethod]
        public void Write_When_Different_Entities_Were_Added()
        {
            // Arrange
            var entities = new List<EntityBase>
            {
                _book, _newspaper, _patent
            };

            var xmlEditor = new XmlEditor(XmlPath);

            // Act
            xmlEditor.Write(entities);

            // Assert
            File.Exists(XmlPath).ShouldBeTrue();
            var doc = XDocument.Load(XmlPath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.Elements().Count().ShouldBe(3);
            CheckExpectedBookElement(doc.Root.Elements().First());
            CheckExpectedNewspaperElement(doc.Root.Elements().ElementAt(1));
            CheckExpectedPatentElement(doc.Root.Elements().Last());
        }

        [TestMethod]
        public void Read_Book_Parsed_As_Expected()
        {
            // Arrange
            var xmlEditor = new XmlEditor(BookEntityPath);

            // Act
            var result = xmlEditor.Read();

            // Assert
            result.Count.ShouldBe(1);
            CheckExpectedEntity(result.First() as Book);
        }

        [TestMethod]
        public void Read_Newspaper_Parsed_As_Expected()
        {
            // Arrange
            var xmlEditor = new XmlEditor(NewspaperEntityPath);

            // Act
            var result = xmlEditor.Read();

            // Assert
            result.Count.ShouldBe(1);
            CheckExpectedEntity(result.First() as Newspaper);
        }

        [TestMethod]
        public void Read_Patent_Parsed_As_Expected()
        {
            // Arrange
            var xmlEditor = new XmlEditor(PatentEntityPath);

            // Act
            var result = xmlEditor.Read();

            // Assert
            result.Count.ShouldBe(1);
            CheckExpectedEntity(result.First() as Patent);
        }

        [TestMethod]
        public void Read_Library_Parsed_As_Expected()
        {
            // Arrange
            var xmlEditor = new XmlEditor(LibraryEntityPath);

            // Act
            var result = xmlEditor.Read();

            // Assert
            result.Count.ShouldBe(3);
            CheckExpectedEntity(result.First() as Book);
            CheckExpectedEntity(result.ElementAt(1) as Newspaper);
            CheckExpectedEntity(result.Last() as Patent);
        }

        [TestMethod]
        public void Write_Optional_Field_Works_As_Expected()
        {
            // Arrange
            _newspaper.Number = null;
            var entities = new List<EntityBase>
            {
                _newspaper
            };

            var xmlEditor = new XmlEditor(OptionalFieldNumberWritePath);

            // Act
            xmlEditor.Write(entities);

            // Assert
            File.Exists(OptionalFieldNumberWritePath).ShouldBeTrue();
            var doc = XDocument.Load(OptionalFieldNumberWritePath);
            doc.Declaration.ShouldNotBeNull();
            doc.Root.HasElements.ShouldBeTrue();
            doc.Root.Elements(TagName.Number).FirstOrDefault().ShouldBeNull();
        }

        [TestMethod]
        public void Read_Optional_Field_Works_As_Expected()
        {
            // Arrange
            var xmlEditor = new XmlEditor(OptionalFieldNumberReadPath);

            // Act
            var result = xmlEditor.Read();

            // Assert
            result.Count.ShouldBe(1);
            var newspaper = result.First() as Newspaper;
            newspaper.Number.ShouldBeNull();
        }

        private void CheckExpectedEntity(Newspaper newspaper)
        {
            newspaper.Name.ShouldBe(_newspaper.Name);
            newspaper.Date.ShouldBe(_newspaper.Date);
            newspaper.PublishDate.ShouldBe(_newspaper.PublishDate);
            newspaper.PublishingHouse.ShouldBe(_newspaper.PublishingHouse);
            newspaper.Issn.ShouldBe(_newspaper.Issn);
            newspaper.Number.ShouldBe(_newspaper.Number);
            newspaper.City.ShouldBe(_newspaper.City);
            newspaper.Note.ShouldBe(_newspaper.Note);
            newspaper.PageAmount.ShouldBe(_newspaper.PageAmount);
        }

        private void CheckExpectedEntity(Book book)
        {
            book.Name.ShouldBe(_book.Name);
            book.Isbn.ShouldBe(_book.Isbn);
            book.Authors.Count.ShouldBe(_book.Authors.Count);
            book.Authors.First().ShouldBe(_book.Authors.First());
            book.PageAmount.ShouldBe(_book.PageAmount);
            book.PublishDate.ShouldBe(_book.PublishDate);
            book.PublishingHouse.ShouldBe(_book.PublishingHouse);
            book.City.ShouldBe(_book.City);
            book.Note.ShouldBe(_book.Note);
        }

        private void CheckExpectedEntity(Patent patent)
        {
            patent.Name.ShouldBe(_patent.Name);
            patent.Country.ShouldBe(_patent.Country);
            patent.PublishDate.ShouldBe(_patent.PublishDate);
            patent.RegistrationNumber.ShouldBe(_patent.RegistrationNumber);
            patent.RequestDate.ShouldBe(_patent.RequestDate);
            patent.Inventor.ShouldBe(_patent.Inventor);
            patent.Note.ShouldBe(_patent.Note);
            patent.PageAmount.ShouldBe(_patent.PageAmount);
        }

        private void CheckExpectedNewspaperElement(XElement newspaper)
        {
            newspaper.Name.ShouldBe(TagName.Newspaper);
            newspaper.Elements(TagName.Name).First().Value.ShouldBe(_newspaper.Name);
            newspaper.Elements(TagName.City).First().Value.ShouldBe(_newspaper.City);
            newspaper.Elements(TagName.PublishHouse).First().Value.ShouldBe(_newspaper.PublishingHouse);
            newspaper.Elements(TagName.PublishDate).First().Value.ShouldBe(_newspaper.PublishDate.ToString(CultureInfo.InvariantCulture));
            newspaper.Elements(TagName.PageAmount).First().Value.ShouldBe(_newspaper.PageAmount.ToString());
            newspaper.Elements(TagName.Note).First().Value.ShouldBe(_newspaper.Note);
            newspaper.Elements(TagName.Number).First().Value.ShouldBe(_newspaper.Number.ToString());
            newspaper.Elements(TagName.Date).First().Value.ShouldBe(_newspaper.Date.ToString(CultureInfo.InvariantCulture));
            newspaper.Elements(TagName.Issn).First().Value.ShouldBe(_newspaper.Issn);
        }

        private void CheckExpectedPatentElement(XElement patent)
        {
            patent.Name.ShouldBe(TagName.Patent);
            patent.Elements(TagName.Name).First().Value.ShouldBe(_patent.Name);
            patent.Elements(TagName.Inventor).First().Value.ShouldBe(_patent.Inventor);
            patent.Elements(TagName.Country).First().Value.ShouldBe(_patent.Country);
            patent.Elements(TagName.RegistrationNumber).First().Value.ShouldBe(_patent.RegistrationNumber.ToString());
            patent.Elements(TagName.RequestDate).First().Value.ShouldBe(_patent.RequestDate?.ToString(CultureInfo.InvariantCulture));
            patent.Elements(TagName.PublishDate).First().Value.ShouldBe(_patent.PublishDate.ToString(CultureInfo.InvariantCulture));
            patent.Elements(TagName.PageAmount).First().Value.ShouldBe(_patent.PageAmount.ToString());
            patent.Elements(TagName.Note).First().Value.ShouldBe(_patent.Note);
        }

        private void CheckExpectedBookElement(XElement book)
        {
            book.Name.ShouldBe(TagName.Book);
            book.Elements(TagName.Name).First().Value.ShouldBe(_book.Name);
            book.Elements(TagName.Authors).First().Elements(TagName.Author).Count().ShouldBe(1);
            book.Elements(TagName.Authors).First()
                .Elements(TagName.Author).First()
                .Value.ShouldBe(_book.Authors.First());
            book.Elements(TagName.City).First().Value.ShouldBe(_book.City);
            book.Elements(TagName.PublishHouse).First().Value.ShouldBe(_book.PublishingHouse);
            book.Elements(TagName.PublishDate).First().Value.ShouldBe(_book.PublishDate.ToString(CultureInfo.InvariantCulture));
            book.Elements(TagName.PageAmount).First().Value.ShouldBe(_book.PageAmount.ToString());
            book.Elements(TagName.Note).First().Value.ShouldBe(_book.Note);
            book.Elements(TagName.Isbn).First().Value.ShouldBe(_book.Isbn);
        }
    }
}

