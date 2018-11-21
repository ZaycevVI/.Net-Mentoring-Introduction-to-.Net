using System.Configuration;
using IntroductionToNoSql.Entity;
using MongoDB.Driver;

namespace IntroductionToNoSql.Service
{
    public class LibraryService
    {
        private const string CollectionName = "books";
        private readonly IMongoDatabase _libraryDb;

        public LibraryService()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client = new MongoClient(connectionString);
            _libraryDb = client.GetDatabase("library");
            _libraryDb.DropCollection(CollectionName);
            _libraryDb.CreateCollection(CollectionName);
        }

        public void Insert(params Book[] books)
        {
            BooksCollection.InsertMany(books);
        }

        public  IMongoCollection<Book> BooksCollection => _libraryDb.GetCollection<Book>(CollectionName);

        public IFindFluent<Book, Book> Books => BooksCollection.Find("{}");
    }
}
