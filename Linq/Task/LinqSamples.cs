// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private readonly DataSource _dataSource = new DataSource();

        [Category("Linq QA")]
        [Title("Task 1")]
        [Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)")]
        public void Linq1()
        {
            var total = 10;

            var customers = _dataSource.Customers
                .Where(c => c.Orders.All(
                    o => o.Total > total));

            foreach (var c in _dataSource.Customers)
            {
                ObjectDumper.Write(c);
            }

            total = 20000;

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Linq QA")]
        [Title("Task 2")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]
        public void Linq2()
        {
            var result = _dataSource.Customers
                .Join(_dataSource.Suppliers,
                    c => new { c.Country, c.City },
                    s => new { s.Country, s.City },
                    (customer, supplier) => new
                    {
                        customer.CompanyName,
                        customer.Country,
                        customer.City,
                        supplier.SupplierName,
                        Country1 = supplier.Country,
                        City1 = supplier.City,
                    });

            foreach (var r in result)
            {
                ObjectDumper.Write(r);
            }
        }

        [Category("Linq QA")]
        [Title("Task 3")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq3()
        {
            const int order = 1000;

            var customers = _dataSource.Customers
                .Where(c => c.Orders.Any(o => o.Total > order));

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Linq QA")]
        [Title("Task 4")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {
            var customers = _dataSource.Customers
                    .Select(c =>
                {
                    var date = c.Orders?
                        .OrderBy(o => o.OrderDate)
                        .FirstOrDefault()?.OrderDate;
                    return new { c.CompanyName, date?.Month, date?.Year };
                });

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Linq QA")]
        [Title("Task 5")]
        [Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
        public void Linq5()
        {
            var customers = _dataSource.Customers
                .Select(c =>
                {
                    var order = c.Orders?
                        .OrderBy(o => o.OrderDate)
                        .FirstOrDefault();
                    return new
                    {
                        c.CompanyName,
                        order?.OrderDate.Month,
                        order?.OrderDate.Year,
                        order?.Total
                    };
                })
                .OrderBy(c => c.Year)
                .ThenBy(c => c.Month)
                .ThenByDescending(c => c.Total)
                .ThenBy(c => c.CompanyName);

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Linq QA")]
        [Title("Task 6")]
        [Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»)")]
        public void Linq6()
        {
            var customers = _dataSource.Customers
                .Where(c => c.PostalCode.Any(p => !char.IsDigit(p)
                || string.IsNullOrEmpty(c.Region)
                || !c.Phone.StartsWith("(")));

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Linq QA")]
        [Title("Task 7")]
        [Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq()
        {
            var customers = _dataSource.Products
                .GroupBy(p => p.Category)
                .Select(p => new
                {
                    Category = p.Key,
                    StockGroup = p.GroupBy(product => product.UnitsInStock).Select(products => new
                    {
                        UnitsInStock = products.Key,
                        Products = products.Select(p1 => p1).OrderBy(p2 => p2.UnitPrice)
                    })
                });

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }
    }
}
