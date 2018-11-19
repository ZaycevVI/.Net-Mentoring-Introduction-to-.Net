using System.Collections.Generic;

namespace IntroductionToNoSql
{
    public static class BookDatabase
    {
        private static List<Book> _books;
        public static List<Book> Books
        {
            get
            {
                if (_books == null)
                    Initialize();

                return _books;
            }
        }

        private static void Initialize()
        {
            _books = new List<Book>
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
                    Genre =  new List<string> {"fantasy"},
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
                    Genre =  new List<string> {"kids"},
                    Year = 2001
                },
            };
        }
    }
}