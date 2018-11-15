using System.Linq;
using DataModels;
using LinqToDB;
using LinqToDB.Common;

namespace Demonstration
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Linq.AllowMultipleQuery = true;
            Products();
            EmployeesWithRegion();
            RegionStatistics();
            EmployeeWithShipper();
        }

        // Список продуктов с категорией и поставщиком
        static void Products()
        {
            using (var db = new NorthwindDB())
            {
                var products = db.Products
                            .LoadWith(p => p.Category)
                            .LoadWith(p => p.Supplier)
                            .ToList();
            }
        }

        // Список сотрудников с указанием региона, 
        // за который они отвечают
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
                    (t1, t2) =>  new { t1.EmployeeID, t2.Region })
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
                            (o, s) => new {o.EmployeeID, s.CompanyName})
                            .Distinct();

                var result = employees.LeftJoin(
                    shippers,
                    (e, s) => e.EmployeeID == s.EmployeeID,
                    (e, s) => new { Employee = e, s.CompanyName })
                    .ToList();
            }
        }
    }
}
