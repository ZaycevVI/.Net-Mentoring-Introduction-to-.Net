using System;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Tools;
// ReSharper disable AccessToDisposedClosure
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable PossibleMultipleEnumeration

namespace Demonstration
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Linq.AllowMultipleQuery = true;
            ProductList();
            EmployeesWithRegion();
            RegionStatistics();
            EmployeeWithShipper();
            InsertEmployeeWithTerritory();
            MoveProductsToNewCategory();
            ReplaceProductsForNotUsedOrders();
            AddProductList();
        }

        #region DQL

        // Список продуктов с категорией и поставщиком
        static void ProductList()
        {
            using (var db = new NorthwindDB())
            {
                var products = db.Products
                            .LoadWith(p => p.Category)
                            .LoadWith(p => p.Supplier)
                            .ToList();
            }
        }

        // Список сотрудников с указанием региона, за который они отвечают
        static void EmployeesWithRegion()
        {
            using (var db = new NorthwindDB())
            {
                var employees = db.Employees
                            .LoadWith(e => e.EmployeeTerritories);

                var employeeTerritotries = employees.SelectMany(e => e.EmployeeTerritories);
                var result = employeeTerritotries
                    .InnerJoin(db.Territories.LoadWith(t => t.Region),
                    (t1, t2) => t1.TerritoryID == t2.TerritoryID,
                    (t1, t2) => new { t1.EmployeeID, t2.Region })
                    .Distinct()
                    .InnerJoin(employees,
                    (region, employee) => region.EmployeeID == employee.EmployeeID,
                    (region, employee) => new { employee, region.Region })
                    .ToList();
            }
        }

        // Статистики по регионам: количества сотрудников по регионам
        static void RegionStatistics()
        {
            using (var db = new NorthwindDB())
            {
                var employees = db.Employees
                    .LoadWith(e => e.EmployeeTerritories);

                var result = employees.SelectMany(e => e.EmployeeTerritories)
                    .InnerJoin(db.Territories.LoadWith(t => t.Region),
                        (t1, t2) => t1.TerritoryID == t2.TerritoryID,
                        (t1, t2) => new { t1.EmployeeID, t2.Region })
                    .Distinct()
                    .InnerJoin(employees,
                        (region, employee) => region.EmployeeID == employee.EmployeeID,
                        (region, employee) => new { employee, region.Region })
                     .GroupBy(arg => arg.Region.RegionDescription)
                     .Select(grouping => new { Region = grouping.Key, Count = grouping.Select(g => g.employee).Count() })
                     .ToList();
            }
        }

        // Списка «сотрудник – с какими грузоперевозчиками работал» (на основе заказов)
        static void EmployeeWithShipper()
        {
            using (var db = new NorthwindDB())
            {
                var employees = db.Employees
                    .LoadWith(e => e.Orders);

                var shippers = employees.SelectMany(e => e.Orders)
                            .InnerJoin(db.Shippers,
                            (o, s) => o.ShipVia == s.ShipperID,
                            (o, s) => new { o.EmployeeID, s.CompanyName })
                            .Distinct();

                var result = employees.LeftJoin(
                    shippers,
                    (e, s) => e.EmployeeID == s.EmployeeID,
                    (e, s) => new { Employee = e, s.CompanyName })
                    .ToList();
            }
        }

        #endregion

        #region DML

        // Добавить нового сотрудника, и указать ему список территорий, за которые он несет ответственность.
        static void InsertEmployeeWithTerritory()
        {
            using (var db = new NorthwindDB())
            {
                var employee = Employee;
                var territory = Territory;

                var employeeTerritories = new EmployeeTerritory
                {
                    Employee = employee,
                    Territory = territory
                };

                employee.EmployeeID = db.InsertWithInt32Identity(employee);
                territory.TerritoryID = "11111";

                employeeTerritories.EmployeeID = employee.EmployeeID;
                employeeTerritories.TerritoryID = territory.TerritoryID;

                db.InsertOrReplace(territory);
                db.Insert(employeeTerritories);
            }
        }

        // Перенести продукты из одной категории в другую
        static void MoveProductsToNewCategory()
        {
            using (var db = new NorthwindDB())
            {
                db.Products
                    .Where(p => p.CategoryID == 3)
                    .Set(p => p.CategoryID, 4)
                    .Update();
            }
        }

        // Добавить список продуктов со своими поставщиками и категориями (массовое занесение),
        // при этом если поставщик или категория с таким названием есть, то использовать их – иначе создать новые.
        static void AddProductList()
        {
            var products = Products;

            using (var db = new NorthwindDB())
            {
                var categoriesToInsert = products.Where(p => p.Category.CategoryName
                                                .NotIn(db.Categories.Select(c => c.CategoryName)))
                                                .Select(p => p.Category);
                var suppliersToInsert = products.Where(p => p.Supplier.CompanyName
                                                        .NotIn(db.Suppliers.Select(c => c.CompanyName)))
                                                        .Select(p => p.Supplier);

                categoriesToInsert.Select(c => db.InsertWithInt32Identity(c));
                suppliersToInsert.Select(s => db.InsertWithInt32Identity(s));

                var resultSet = products.Select(p =>
                {
                    p.CategoryID = db.Categories.FirstOrDefault(c => c.CategoryName == p.Category.CategoryName)
                        .CategoryID;
                    p.SupplierID = db.Suppliers.FirstOrDefault(s => s.CompanyName == p.Supplier.CompanyName)
                        .SupplierID;
                    return p;
                });

                foreach (var item in resultSet)
                    db.InsertWithInt32Identity(item);
            }
        }

        // Замена продукта на аналогичный: во всех еще неисполненных заказах 
        // (считать таковыми заказы, у которых ShippedDate = NULL) заменить один продукт на другой.
        static void ReplaceProductsForNotUsedOrders()
        {
            using (var db = new NorthwindDB())
            {
                var orderDetails = db.OrderDetails
                    .InnerJoin(
                            db.Orders,
                            (d, o) => o.OrderID == d.OrderID,
                            (d, o) => new { Order = o, OrderDetail = d })
                    .Where(o => o.Order.ShippedDate == null)
                    .Select(r => r.OrderDetail).ToList();

                foreach (var orderDetail in orderDetails)
                {
                    var productIdForOrder = db.OrderDetails
                        .Where(o => o.OrderID == orderDetail.OrderID)
                        .Select(o => o.ProductID).ToList();

                    var productId = db.Products.Select(p => p.ProductID).FirstOrDefault(p => p.NotIn(productIdForOrder));

                    db.OrderDetails
                        .Where(o => o.OrderID == orderDetail.OrderID && o.ProductID == orderDetail.ProductID)
                        .Set(o => o.ProductID, productId)
                        .Update();
                }
            }
        }

        #endregion

        public static List<Product> Products => new List<Product>
        {
            new Product
            {
                ProductName = "Fanta",
                Category = new Category { CategoryName = "Beverages" },
                Supplier = new Supplier { CompanyName = "Coca-Cola" }
            },
            new Product
            {
                ProductName = "Mojito",
                Category = new Category { CategoryName = "Beverages"  },
                Supplier = new Supplier { CompanyName = "Exotic Liquids" }
            },
            new Product
            {
                ProductName = "Latte",
                Category = new Category { CategoryName = "Coffee"  },
                Supplier = new Supplier { CompanyName = "Lavazza" }
            }
        };

        public static Employee Employee => new Employee
        {
            LastName = "Zaitsau",
            FirstName = "Uladzimir",
            Title = "Sales Representative",
            TitleOfCourtesy = "Ms.",
            BirthDate = new DateTime(1992, 09, 14),
            HireDate = DateTime.Now,
            Address = "16 Garrett Hill",
            City = "Minsk",
            PostalCode = "12345",
            Country = "Belarus",
            HomePhone = "123456",
            Extension = "123"
        };

        public static Territory Territory => new Territory
        {
            TerritoryDescription = "Minsk",
            RegionID = 1
        };

    }
}
