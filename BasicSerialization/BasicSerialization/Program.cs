using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using BasicSerialization.Entity;

namespace BasicSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var formatter = new XmlSerializer(typeof(Catalog));
            Catalog catalog;


            using (var fs = new FileStream("books.xml", FileMode.Open))
            {
                catalog = formatter.Deserialize(fs) as Catalog;
                Display(catalog);
            }

            using (var fs = new FileStream("booksSerialized.xml", FileMode.CreateNew))
            {
                formatter.Serialize(fs, catalog);
            }
        }

        private static void Display(Catalog catalog)
        {
            Console.WriteLine("Catalog Date: {0}", catalog.Date);

            foreach (var book in catalog.Books)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine("Book");
                Console.WriteLine("Id: {0}", book.Id);
                Console.WriteLine("Isbn: {0}", book.Isbn);
                Console.WriteLine("Author: {0}", book.Author);
                Console.WriteLine("Title: {0}", book.Title);
                Console.WriteLine("Genre: {0}", book.Genre);
                Console.WriteLine("Publisher: {0}", book.Publisher);
                Console.WriteLine("PublishDate: {0}", book.PublishDate);
                Console.WriteLine("Description: {0}", book.Description);
                Console.WriteLine("RegistrationDate: {0}", book.RegistrationDate);
                Console.WriteLine("===============================================");
            }
        }
    }
}
