using System;
using System.Linq;
using System.Threading;
using Caching.Caching;
using Caching.Manager;
using NorthwindLibrary;

namespace Caching
{
    class Program
    {
        static void Main(string[] args)
        {
            // Timeout на кеш по умолчанию = 30 секунд.
            // При изменении данных в бд из, например, Management Studio, привдете к удалению кэша
            using (var customerManager = new EntityManager<Customer>("Customers"))
            {
                for (var i = 0; i < 5; i++)
                {
                    Console.WriteLine(customerManager.GetCategories().Count());
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
