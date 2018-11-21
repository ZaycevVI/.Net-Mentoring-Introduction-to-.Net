using System;
using System.Collections.Generic;
using System.Linq;
using IntroductionToNoSql.Entity;
using IntroductionToNoSql.Service;
using MongoDB.Driver;

namespace IntroductionToNoSql
{
    class Program
    {
        static void Main(string[] args)
        {
            var libraryService = new LibraryService();

            // Добавьте следующие книги (название, автор, количество экземпляров, жанр, год издания)
            libraryService.Insert(Books);

            Console.WriteLine("Найдите книги с количеством экземпляров больше единицы");
            Console.WriteLine("Покажите в результате только название книги.");
            var bookNames = libraryService
                .BooksCollection
                .Find(b => b.Count > 1)
                .Project(book => book.Name)
                .ToList();

            foreach (var name in bookNames)
                Console.WriteLine(name);

            PrintSeparator();

            Console.WriteLine("Отсортируйте книги по названию.");
            var books = libraryService
                .Books
                .Sort(new JsonSortDefinition<Book>("{Name: 1}"))
                .ToList();

            PrintBooks(books);

            Console.WriteLine("Ограничьте количество возвращаемых книг тремя");
            books = libraryService
                .Books
                .Limit(3)
                .ToList();

            PrintBooks(books);

            Console.WriteLine("Подсчитайте количество книг с кол-вом экземпляров > 1.");

            var count = libraryService
                .BooksCollection
                .Find(b => b.Count > 1)
                .CountDocuments();

            Console.WriteLine(count);
            PrintSeparator();

            Console.WriteLine("Найдите книгу с макимальным/минимальным количеством (count");

            var bookMinCount = libraryService
                .Books
                .Sort(new JsonSortDefinition<Book>("{Count: 1}"))
                .Limit(1)
                .FirstOrDefault();

            var bookMaxCount = libraryService
                .Books
                .Sort(new JsonSortDefinition<Book>("{Count: -1}"))
                .Limit(1)
                .FirstOrDefault();

            Console.WriteLine(bookMaxCount);
            Console.WriteLine(bookMinCount);
            PrintSeparator();

            Console.WriteLine("Найдите список авторов (каждый автор должен быть в списке один раз)");

            var distinctAuthors = libraryService
                .BooksCollection
                .Distinct(book => book.Author, "{Author: {$ne: null}}")
                .ToList();

            foreach (var author in distinctAuthors)
                Console.WriteLine(author);

            PrintSeparator();

            Console.WriteLine("Выберите книги без авторов");

            var booksWithoutAuthor = libraryService
                .BooksCollection
                .Find("{Author: {$eq: null}}")
                .ToList();

            PrintBooks(booksWithoutAuthor);

            Console.WriteLine("Увеличьте количество экземпляров каждой книги на единицу.");

            libraryService
                .BooksCollection
                .UpdateMany("{}", new JsonUpdateDefinition<Book>("{$inc:{Count:1}}"));

            PrintBooks(libraryService.Books.ToList());

            Console.WriteLine(
                "Добавьте дополнительный жанр “favority” всем книгам с жанром “fantasy” (последующие запуски запроса не должны дублировать жанр “favority”).");

            libraryService
                .BooksCollection
                .UpdateMany(b => b.Genre.Contains("fantasy"), new JsonUpdateDefinition<Book>("{$addToSet:{Genre:\"favority\"}}"));

            libraryService
                .BooksCollection
                .UpdateMany(b => b.Genre.Contains("fantasy"), new JsonUpdateDefinition<Book>("{$addToSet:{Genre:\"favority\"}}"));

            libraryService
                .BooksCollection
                .UpdateMany(b => b.Genre.Contains("fantasy"), new JsonUpdateDefinition<Book>("{$addToSet:{Genre:\"favority\"}}"));

            PrintBooks(libraryService.Books.ToList());

            Console.WriteLine("Удалите книги с количеством экземпляров меньше трех");
            libraryService
                .BooksCollection
                .DeleteMany(book => book.Count < 3);

            
            PrintBooks(libraryService.Books.ToList());

            Console.WriteLine("Удалите все книги.");
            libraryService
                .BooksCollection
                .DeleteMany("{}");

            PrintBooks(libraryService.Books.ToList());
        }

        static void PrintBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine(book);

            PrintSeparator();
        }

        static void DeleteWithCountLess(int count)
        {
            BookDatabase.Books.RemoveAll(b => b.Count < count);
        }

        static void PrintSeparator()
        {
            Console.WriteLine("==================================");
        }

        private static Book[] Books => new Book[]
        {
            new Book
            {
                Name = "Hobbit",
                Author = "Tolkien",
                Count = 5,
                Genre = new List<string> {"fantasy"},
                Year = 2014
            },
            new Book
            {
                Name = "Lord of the rings",
                Author = "Tolkien",
                Count = 3,
                Genre = new List<string> {"fantasy"},
                Year = 2015
            },
            new Book
            {
                Name = "Kolobok",
                Count = 10,
                Genre = new List<string> {"kids"},
                Year = 2000
            },
            new Book
            {
                Name = "Repka",
                Count = 11,
                Genre = new List<string> {"kids"},
                Year = 2000
            },
            new Book
            {
                Name = "Dyadya Stiopa",
                Author = "Mihalkov",
                Count = 1,
                Genre = new List<string> {"kids"},
                Year = 2001
            }
        };
    }
}
