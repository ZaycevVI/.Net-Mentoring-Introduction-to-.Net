using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace IntroductionToNoSql
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Найдите книги с количеством экземпляров больше единицы");
            Console.WriteLine("Покажите в результате только название книги.");
            var names = GetBooksWithCountGreaterOne().Select(b => b.Name);

            foreach (var name in names)
                Console.WriteLine("[Name]: {0}", name);

            PrintSeparator();

            Console.WriteLine("Отсортируйте книги по названию.");
            var books = GetBooksWithCountGreaterOne().OrderBy(b => b.Name);

            PrintBooks(books);

            Console.WriteLine("Ограничьте количество возвращаемых книг тремя");
            var books1 = GetBooksWithCountGreaterOne().Take(3);
            PrintBooks(books1);

            Console.WriteLine("Подсчитайте количество книг с кол-вом экземпляров > 1.");
            Console.WriteLine(GetBooksWithCountGreaterOne().Count());
            PrintSeparator();

            Console.WriteLine("Найдите книгу с макимальным/минимальным количеством (count");
            Console.WriteLine("Max Count: " + BookDatabase.Books.FirstOrDefault(b => b.Count == BookDatabase.Books.Max(b1 => b1.Count)));
            Console.WriteLine("Min Count: " + BookDatabase.Books.FirstOrDefault(b => b.Count == BookDatabase.Books.Min(b1 => b1.Count)));
            PrintSeparator();

            Console.WriteLine("Найдите список авторов (каждый автор должен быть в списке один раз)");
            var distinctAuthors = BookDatabase.Books.Where(b => !string.IsNullOrEmpty(b.Author)).Select(b => b.Author).Distinct();

            foreach (var author in distinctAuthors)
                Console.WriteLine(author);

            PrintSeparator();

            Console.WriteLine("Выберите книги без авторов");
            var booksWithoutAuthors = BookDatabase.Books.Where(b => string.IsNullOrEmpty(b.Author));
            PrintBooks(booksWithoutAuthors);

            Console.WriteLine("Увеличьте количество экземпляров каждой книги на единицу.");

            foreach (var book in BookDatabase.Books)
                book.Count += 1;

            PrintBooks();
            Console.WriteLine("Добавьте дополнительный жанр “favority” всем книгам с жанром “fantasy” (последующие запуски запроса не должны дублировать жанр “favority”).");

            AddGenre("favority");
            AddGenre("favority");
            AddGenre("favority");
            AddGenre("favority");

            PrintBooks();

            Console.WriteLine("Удалите книги с количеством экземпляров меньше трех");
            DeleteWithCountLess(3);

            foreach (var book in BookDatabase.Books)
                Console.WriteLine(book);
            Console.WriteLine("Удалите все книги.");
            BookDatabase.Books.Clear();
            PrintBooks();
        }

        static IEnumerable<Book> GetBooksWithCountGreaterOne()
        {
            return BookDatabase.Books.Where(b => b.Count > 1);
        }

        static void PrintBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine(book);

            PrintSeparator();
        }

        static void PrintBooks()
        {
            PrintBooks(BookDatabase.Books);
        }

        static void AddGenre(string genre)
        {
            var genres = BookDatabase.Books
                .Where(b => b.Genre.All(g => g != genre && g == "fantasy"))
                .Select(b => b.Genre);

            foreach (var g in genres)
                g.Add(genre);
        }

        static void DeleteWithCountLess(int count)
        {
            BookDatabase.Books.RemoveAll(b => b.Count < count);
        }

        static void PrintSeparator()
        {
            Console.WriteLine("==================================");
        }
    }
}
