using System;
using System.Linq;
using Northwind.Entity;

namespace Northwind
{
    public static class NorthwindQuery
    {
        public static void ShowProductsByCategory()
        {
            using (var db = new NorthwindDb())
            {
                var orders = db.Orders
                    .Where(o => o.Order_Details.All(od => od.Product.Category.CategoryID == 1))
                    .Select(o => new
                    {
                        o.Customer.CompanyName,
                        o.Order_Details,
                        Products = o.Order_Details.Select(od => od.Product)
                    });

                foreach (var order in orders)
                {
                    Console.WriteLine("Company Name: {0}", order.CompanyName);
                    Console.WriteLine("\t\tProducts:");

                    foreach (var product in order.Products)
                    {
                        Console.WriteLine("\t\t\tName: {0}, Category: {1}", product.ProductName, product.Category.CategoryName);
                    }
                }
            }
        }
    }
}
