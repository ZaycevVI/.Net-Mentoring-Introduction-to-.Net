use Northwind;

-- 1.1
-- Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate)
-- включительно и которые доставлены с ShipVia >= 2. Запрос должен возвращать только колонки OrderID,
-- ShippedDate и ShipVia.

Select o.OrderID, o.ShippedDate, o.ShipVia 
From Orders as o
Where o.ShippedDate >= CONVERT(datetime, '1998-05-06')
		And ShipVia >= 2

-- Написать запрос, который выводит только недоставленные заказы из таблицы Orders. 
-- В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’ 
-- (использовать системную функцию CASЕ). Запрос должен возвращать только колонки OrderID и ShippedDate.
-- Использование Case в данном задании бессмысленно

Select OrderID, 'Not shipped' as ShippedDate
From Orders
Where ShippedDate Is Null

-- Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate)
-- не включая эту дату или которые еще не доставлены. В запросе должны возвращаться только 
-- колонки OrderID (переименовать в Order Number) и ShippedDate (переименовать в Shipped Date).
-- В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’,
-- для остальных значений возвращать дату в формате по умолчанию.

Select OrderID as [Order Number],
CASE
	WHEN o.ShippedDate Is Null Then 'Not Shipped'
	ELSE Cast(o.ShippedDate as nvarchar(30))
End as [Shipped Date]
From Orders o
Where o.ShippedDate >= CONVERT(datetime, '1998-05-06')
		Or o.ShippedDate Is Null

-- 1.2
-- Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. 
-- Запрос сделать с только помощью оператора IN. Возвращать колонки с именем 
-- пользователя и названием страны в результатах запроса. Упорядочить результаты 
-- запроса по имени заказчиков и по месту проживания.

Select c.ContactName, c.Country
From Customers c
Where c.Country IN ('USA', 'Canada')
Order by c.ContactName, c.Country

-- Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada.
-- Запрос сделать с помощью оператора IN. Возвращать колонки с именем пользователя
-- и названием страны в результатах запроса. Упорядочить результаты запроса по имени заказчиков.

Select c.ContactName, c.Country
From Customers c
Where c.Country NOT IN ('USA', 'Canada')
Order by c.ContactName

-- Выбрать из таблицы Customers все страны, в которых проживают заказчики.
-- Страна должна быть упомянута только один раз и список отсортирован по убыванию.
-- Не использовать предложение GROUP BY. Возвращать только одну колонку в результатах запроса.

Select distinct c.Country
From Customers c
Order by c.Country desc

-- 1.3
-- Выбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повторяться),
-- где встречаются продукты с количеством от 3 до 10 включительно – это колонка Quantity
-- в таблице Order Details. Использовать оператор BETWEEN. Запрос должен возвращать только колонку OrderID.

Select distinct o.OrderID
From [Order Details] o
Where o.Quantity Between 3 And 10

-- Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается
-- на буквы из диапазона b и g. Использовать оператор BETWEEN. Проверить, что в результаты
-- запроса попадает Germany. Запрос должен возвращать только колонки CustomerID и Country
-- и отсортирован по Country.

Select c.CustomerID, c.Country
From Customers c
Where Left(c.Country, 1) Between 'b' And 'g' 

-- Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается
-- на буквы из диапазона b и g, не используя оператор BETWEEN.

Select c.CustomerID, c.Country
From Customers c
Where Left(c.Country, 1) >= 'b' 
	And Left(c.Country, 1) <= 'g' 

-- 1.4
-- В таблице Products найти все продукты (колонка ProductName), где встречается подстрока 'chocolade'. 
-- Известно, что в подстроке 'chocolade' может быть изменена одна буква 'c' в середине - найти все продукты,
-- которые удовлетворяют этому условию.

Select p.ProductName
From Products p
Where p.ProductName like '%cho_olade%'

-- 2
-- 2.1
-- Найти общую сумму всех заказов из таблицы Order Details с учетом количества
-- закупленных товаров и скидок по ним. Результатом запроса должна быть одна запись
-- с одной колонкой с названием колонки 'Totals'.

Select Sum(UnitPrice * Quantity * (1 - Discount)) as Totals
From [Order Details]

-- По таблице Orders найти количество заказов, которые еще не были доставлены
-- (т.е. в колонке ShippedDate нет значения даты доставки). Использовать при этом
-- запросе только оператор COUNT. Не использовать предложения WHERE и GROUP.

Select Count(*) - Count(ShippedDate)
From Orders

-- По таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы.
-- Использовать функцию COUNT и не использовать предложения WHERE и GROUP.

Select distinct CustomerId, count(*) over (partition by CustomerId) as Count
From Orders

-- 2.2
-- По таблице Orders найти количество заказов с группировкой по годам. 
-- В результатах запроса надо возвращать две колонки c названиями Year и Total.
-- Написать проверочный запрос, который вычисляет количество всех заказов.

Select year(OrderDate) [Year], Count(OrderDate) Total
From Orders
Group by year(OrderDate)

Select Count(OrderDate)
From Orders

-- По таблице Orders найти количество заказов, cделанных каждым продавцом.
-- Заказ для указанного продавца – это любая запись в таблице Orders, 
-- где в колонке EmployeeID задано значение для данного продавца. 
-- В результатах запроса надо возвращать колонку с именем продавца 
-- (Должно высвечиваться имя полученное конкатенацией LastName & FirstName. 
-- Эта строка LastName & FirstName должна быть получена отдельным запросом в колонке основного запроса.
-- Также основной запрос должен использовать группировку по EmployeeID.) с названием колонки ‘Seller’
-- и колонку c количеством заказов возвращать с названием 'Amount'. Результаты запроса должны быть
-- упорядочены по убыванию количества заказов.

Select 
	(Select (LastName + ' ' + FirstName) 
	From Employees e
	Where e.EmployeeID = o.EmployeeID) Seller, 
	Sum(o.EmployeeId) Amount
From Orders o
Group by o.EmployeeID

-- По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя.
-- Необходимо определить это только для заказов, сделанных в 1998 году.

Select EmployeeID, CustomerID, Count(EmployeeID) [Count]
From Orders
Where year(ShippedDate) = 1998
Group by EmployeeID, CustomerID 

-- Найти покупателей и продавцов, которые живут в одном городе.
-- Если в городе живут только один или несколько продавцов, или только один или несколько покупателей, 
-- то информация о таких покупателя и продавцах не должна попадать в результирующий набор.
-- Не использовать конструкцию JOIN.

Select *
From
(Select ContactName, City, 'Customer' [Role] 
From Customers
Where City = Any 
	(Select City 
	From Employees)
Union
Select LastName, City, 'Employee'
From Employees
Where City = Any 
	(Select City 
	From Customers)) as t
	Order by t.City

-- Найти всех покупателей, которые живут в одном городе.

Select c1.City, c2.ContactName
From
(Select City
From Customers
Group by City) as c1
Left Join (Select ContactName, City
From Customers) as c2
On c1.City = c2.City

-- По таблице Employees найти для каждого продавца его руководителя.

Select e1.EmployeeID, e1.LastName, 
e2.EmployeeID, e2.LastName 
From Employees e1
Left Join Employees e2
On e1.ReportsTo = e2.EmployeeID

-- 2.3
-- Определить продавцов, которые обслуживают регион 'Western' (таблица Region).

Select distinct e.EmployeeID, e.FirstName, e.LastName, r.RegionDescription
From Employees e
Inner Join EmployeeTerritories et
On e.EmployeeID = et.EmployeeID
Inner Join Territories t
On et.TerritoryID = t.TerritoryID
Inner Join [Regions] r
On t.RegionID = r.RegionID
Where RegionDescription = 'Western'

-- Выдать в результатах запроса имена всех заказчиков из таблицы Customers и суммарное количество
-- их заказов из таблицы Orders. Принять во внимание, что у некоторых заказчиков нет заказов,
-- но они также должны быть выведены в результатах запроса. Упорядочить результаты запроса по возрастанию
-- количества заказов.

Select c.CustomerID, Count(c.CustomerID) [Amount of orders]
From Customers c
Left Join Orders o
On c.CustomerID = o.CustomerID
Group by c.CustomerID
Order by Count(c.CustomerId)

-- 2.4
-- Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), у которых нет 
-- хотя бы одного продукта на складе (UnitsInStock в таблице Products равно 0). 
-- Использовать вложенный SELECT для этого запроса с использованием оператора IN.

Select CompanyName
From Suppliers s
Where s.SupplierID In (
	Select SupplierID
	From Products
	Where UnitsInStock = 0)

-- Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.

Select CustomerID 
From Customers c
Where CustomerId In (Select CustomerID
			From Orders
			Group by CustomerID
			Having Count(CustomerId) > 15)

-- Выдать всех заказчиков (таблица Customers), которые не имеют ни одного заказа (подзапрос по таблице Orders).
-- Использовать оператор EXISTS

Select * 
From Customers c
Where Not Exists (Select * 
					From Orders o
					Where c.CustomerID = o.CustomerID)
