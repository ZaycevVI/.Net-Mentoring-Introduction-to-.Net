using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;

namespace Task.DB
{
    [Serializable]
    public partial class Product : ISerializable
    {
        protected Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32("ProductID");
            ProductName = info.GetString("ProductName");
            SupplierID = info.GetValue("SupplierID", typeof(int?)) as int?;
            CategoryID = info.GetValue("CategoryID", typeof(int?)) as int?;
            QuantityPerUnit = info.GetString("QuantityPerUnit");
            UnitPrice = info.GetValue("UnitPrice", typeof(decimal?)) as decimal?;
            UnitsInStock = info.GetValue("UnitsInStock", typeof(short?)) as short?;
            UnitsOnOrder = info.GetValue("UnitsOnOrder", typeof(short?)) as short?;
            ReorderLevel = info.GetValue("ReorderLevel", typeof(short?)) as short?;
            Category = info.GetValue("Category", typeof(Category)) as Category;
            Supplier = info.GetValue("Supplier", typeof(Supplier)) as Supplier;
            Order_Details = info.GetValue("Order_Details", typeof(ICollection<Order_Detail>)) as ICollection<Order_Detail>;
            Discontinued = info.GetBoolean("Discontinued");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var objectContext = (context.Context as IObjectContextAdapter).ObjectContext;
            objectContext.LoadProperty(this, p => p.Category);
            objectContext.LoadProperty(this, p => p.Order_Details);
            objectContext.LoadProperty(this, p => p.Supplier);

            info.AddValue("ProductID", ProductID);
            info.AddValue("ProductName", ProductName);
            info.AddValue("SupplierID", SupplierID);
            info.AddValue("CategoryID", CategoryID);
            info.AddValue("QuantityPerUnit", QuantityPerUnit);
            info.AddValue("UnitPrice", UnitPrice);
            info.AddValue("UnitsInStock", UnitsInStock);
            info.AddValue("UnitsOnOrder", UnitsOnOrder);
            info.AddValue("ReorderLevel", ReorderLevel);
            info.AddValue("Category", Category);
            info.AddValue("Supplier", Supplier);
            info.AddValue("Order_Details", Order_Details);
            info.AddValue("Discontinued", Discontinued);
        }
    }
}