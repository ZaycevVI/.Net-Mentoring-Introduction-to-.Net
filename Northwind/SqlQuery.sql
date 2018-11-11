﻿use Northwind;

-- 1.1
--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate) включительно и которые доставлены с ShipVia >= 2. Запрос должен возвращать только колонки OrderID, ShippedDate и ShipVia.

Select o.OrderID, o.ShippedDate, o.ShipVia 
From Northwind.Orders as o
Where o.ShippedDate >= CONVERT(datetime, '1998-05-06')
		And ShipVia >= 2

-- Написать запрос, который выводит только недоставленные заказы из таблицы Orders. В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’ (использовать системную функцию CASЕ). Запрос должен возвращать только колонки OrderID и ShippedDate.
-- Использование Case в данном задании бессмысленно

Select OrderID, 'Not shipped' as ShippedDate
From Northwind.Orders
Where ShippedDate Is Null

--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate) не включая эту дату или которые еще не доставлены. В запросе должны возвращаться только колонки OrderID (переименовать в Order Number) и ShippedDate (переименовать в Shipped Date). В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’, для остальных значений возвращать дату в формате по умолчанию.

Select OrderID as [Order Number],
CASE
	WHEN o.ShippedDate Is Null Then 'Not Shipped'
	ELSE Cast(o.ShippedDate as nvarchar(30))
End as [Shipped Date]
From Northwind.Orders o
Where o.ShippedDate >= CONVERT(datetime, '1998-05-06')
		Or o.ShippedDate Is Null

-- 1.2
-- Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. Запрос сделать с только помощью оператора IN. Возвращать колонки с именем пользователя и названием страны в результатах запроса. Упорядочить результаты запроса по имени заказчиков и по месту проживания.

Select c.ContactName, c.Country
From Northwind.Customers c
Where c.Country IN ('USA', 'Canada')
Order by c.ContactName, c.Country

-- Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada. Запрос сделать с помощью оператора IN. Возвращать колонки с именем пользователя и названием страны в результатах запроса. Упорядочить результаты запроса по имени заказчиков.

Select c.ContactName, c.Country
From Northwind.Customers c
Where c.Country NOT IN ('USA', 'Canada')
Order by c.ContactName

-- Выбрать из таблицы Customers все страны, в которых проживают заказчики. Страна должна быть упомянута только один раз и список отсортирован по убыванию. Не использовать предложение GROUP BY. Возвращать только одну колонку в результатах запроса.

Select distinct c.Country
From Northwind.Customers c
Order by c.Country desc

-- 1.3
-- Выбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повторяться), где встречаются продукты с количеством от 3 до 10 включительно – это колонка Quantity в таблице Order Details. Использовать оператор BETWEEN. Запрос должен возвращать только колонку OrderID.

Select distinct o.OrderID
From Northwind.[Order Details] o
Where o.Quantity Between 3 And 10

-- Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается на буквы из диапазона b и g. Использовать оператор BETWEEN. Проверить, что в результаты

Select c.CustomerID, c.Country
From Northwind.Customers c
Where Left(c.Country, 1) Between 'b' And 'g' 

-- Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается на буквы из диапазона b и g, не используя оператор BETWEEN.

Select c.CustomerID, c.Country
From Northwind.Customers c
Where Left(c.Country, 1) >= 'b' 
	And Left(c.Country, 1) <= 'g' 

-- 1.4
-- В таблице Products найти все продукты (колонка ProductName), где встречается подстрока 'chocolade'. Известно, что в подстроке 'chocolade' может быть изменена одна буква 'c' в середине - найти все продукты, которые удовлетворяют этому условию.

Select p.ProductName
From Northwind.Products p
Where p.ProductName like '%cho_olade%'

-- 2
-- 2.1
-- Найти общую сумму всех заказов из таблицы Order Details с учетом количества закупленных товаров и скидок по ним. Результатом запроса должна быть одна запись с одной колонкой с названием колонки 'Totals'.

Select Sum((UnitPrice * Quantity) * Discount) as Totals
From Northwind.[Order Details]

-- По таблице Orders найти количество заказов, которые еще не были доставлены (т.е. в колонке ShippedDate нет значения даты доставки). Использовать при этом запросе только оператор COUNT. Не использовать предложения WHERE и GROUP.

Select Count(*) - Count(ShippedDate)
From Northwind.Orders

-- По таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы. Использовать функцию COUNT и не использовать предложения WHERE и GROUP.

Select distinct CustomerId, count(*) over (partition by CustomerId) as Count
From Northwind.Orders

-- 2.2
-- По таблице Orders найти количество заказов с группировкой по годам. В результатах запроса надо возвращать две колонки c названиями Year и Total. Написать проверочный запрос, который вычисляет количество всех заказов.

Select DATEPART(yyyy, o.OrderDate) [Year], Count(DATEPART(yyyy, o.OrderDate))
From Northwind.Orders o
Group by DATEPART(yyyy, o.OrderDate)

Select Count(OrderDate)
From Northwind.Orders

-- По таблице Orders найти количество заказов, cделанных каждым продавцом. Заказ для указанного продавца – это любая запись в таблице Orders, где в колонке EmployeeID задано значение для данного продавца. В результатах запроса надо возвращать колонку с именем продавца (Должно высвечиваться имя полученное конкатенацией LastName & FirstName. Эта строка LastName & FirstName должна быть получена отдельным запросом в колонке основного запроса. Также основной запрос должен использовать группировку по EmployeeID.) с названием колонки ‘Seller’ и колонку c количеством заказов возвращать с названием 'Amount'. Результаты запроса должны быть упорядочены по убыванию количества заказов.

Select 
	(Select (LastName + ' ' + FirstName) 
	From Northwind.Employees e
	Where e.EmployeeID = o.EmployeeID) Seller, 
	Sum(o.EmployeeId) Amount
From Northwind.Orders o
Group by o.EmployeeID

-- По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя. Необходимо определить это только для заказов, сделанных в 1998 году.

Select EmployeeID, CustomerID, Count(EmployeeID)
From Northwind.Orders
Where DATEPART(yyyy, ShippedDate) = 1998
Group by EmployeeID, CustomerID 

-- Найти покупателей и продавцов, которые живут в одном городе. Если в городе живут только один или несколько продавцов, или только один или несколько покупателей, то информация о таких покупателя и продавцах не должна попадать в результирующий набор. Не использовать конструкцию JOIN.

Select *
From
(Select ContactName, City, 'Customer' [Role] 
From Northwind.Customers
Where City = Any 
	(Select City 
	From Northwind.Employees)
Union
Select LastName, City, 'Employee'
From Northwind.Employees
Where City = Any 
	(Select City 
	From Northwind.Customers)) as t
	Order by t.City

-- Найти всех покупателей, которые живут в одном городе.

Select c1.City, c2.ContactName
From
(Select City
From Northwind.Customers
Group by City) as c1
Left Join (Select ContactName, City
From Northwind.Customers) as c2
On c1.City = c2.City

-- По таблице Employees найти для каждого продавца его руководителя.

Select e1.EmployeeID, e1.LastName, 
e2.EmployeeID, e2.LastName 
From Northwind.Employees e1
Left Join Northwind.Employees e2
On e1.ReportsTo = e2.EmployeeID