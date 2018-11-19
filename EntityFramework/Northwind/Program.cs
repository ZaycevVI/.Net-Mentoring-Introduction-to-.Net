using System;
using System.Data.Entity;
using Northwind.Entity;
using Northwind.Migrations;

namespace Northwind
{
    class Program
    {
        static void Main(string[] args)
        {

            // NorthwindQuery.ShowProductsByCategory();
            //Database.SetInitializer(new CreateDatabaseIfNotExists<NorthwindDb>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NorthwindDb, Configuration>());
            ShowRegions();
            PrintSeparator();
            Console.WriteLine("After changes: ");
            ChangeCategories();
            ShowRegions();
        }

        static void ChangeCategories()
        {
            using (var db = new NorthwindDb())
            {
                var regions = db.Regions;

                foreach (var region in regions)
                    region.RegionDescription = "New value";

                db.SaveChanges();
            }
        }

        static void ShowRegions()
        {
            using (var db = new NorthwindDb())
            {
                foreach (var region in db.Regions)
                {
                    Console.WriteLine("Region description: {0}", region.RegionDescription);

                    foreach (var territory in region.Territories)
                    {
                        Console.WriteLine("\t\tTerritory description: {0}", territory.TerritoryDescription);
                    }
                }
            }
        }

        static void PrintSeparator()
        {
            Console.WriteLine("============================================================");
        }
    }
}
