using Northwind.Entity;

namespace Northwind.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Northwind.Entity.NorthwindDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Northwind.Entity.NorthwindDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Categories.AddOrUpdate(c => c.CategoryID,
                new Category { CategoryName = "Alcohol Drinks", CategoryID = 1 },
                new Category { CategoryName = "Software", CategoryID = 2 },
                new Category { CategoryName = "Service", CategoryID = 3 });

            context.Regions.AddOrUpdate(r => r.RegionID, 
                new Region { RegionID = 1, RegionDescription = "Europe"},
                new Region { RegionID = 2, RegionDescription = "Asia" },
                new Region { RegionID = 3, RegionDescription = "Africa" });

            context.Territories.AddOrUpdate(t => t.TerritoryID,
                new Territory { TerritoryID = "1", RegionID = 1, TerritoryDescription = "Minsk"},
                new Territory { TerritoryID = "2", RegionID = 2, TerritoryDescription = "Tokio"},
                new Territory { TerritoryID = "3", RegionID = 3, TerritoryDescription = "Rabat"});
        }
    }
}
