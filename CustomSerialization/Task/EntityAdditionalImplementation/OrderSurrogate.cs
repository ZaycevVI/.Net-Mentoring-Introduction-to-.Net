using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using Task.DB;

namespace Task.EntityAdditionalImplementation
{
    public class OrderSurrogate : IDataContractSurrogate
    {
        private readonly IMapper _mapper;

        public OrderSurrogate()
        {
            // Automapper необходим для трансформации прокси-объектов в энтити
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, Order>();
                cfg.CreateMap<Customer, Customer>()
                    .ForMember(e => e.CustomerDemographics, opt => opt.Ignore())
                    .ForMember(e => e.Orders, opt => opt.Ignore());
                cfg.CreateMap<Shipper, Shipper>()
                .ForMember(shipper => shipper.Orders, opt => opt.Ignore());
                cfg.CreateMap<Employee, Employee>()
                .ForMember(e => e.Employee1, opt => opt.Ignore())
                .ForMember(e => e.Employees1, opt => opt.Ignore())
                .ForMember(e => e.Territories, opt => opt.Ignore())
                .ForMember(e => e.Orders, opt => opt.Ignore());
                cfg.CreateMap<Order_Detail, Order_Detail>()
                    .ForMember(od => od.Order, opt => opt.Ignore())
                    .ForMember(od => od.Product, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();
        }

        #region Not implemented
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }
        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Type GetDataContractType(Type type)
        {
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            switch(obj)
            {
                case IEnumerable<Order> pr:
                    return _mapper.Map<IEnumerable<Order>, IEnumerable<Order>>(pr);
                case Order o:
                    return _mapper.Map<Order, Order>(o);
                case Customer c:
                    return _mapper.Map<Customer, Customer>(c);
                case Order_Detail od:
                    return _mapper.Map<Order_Detail, Order_Detail>(od);
                case Shipper s:
                    return _mapper.Map<Shipper, Shipper>(s);
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        private class OrderFake
        {
            public int OrderID { get; set; }

            public string CustomerID { get; set; }

            public int? EmployeeID { get; set; }

            public DateTime? OrderDate { get; set; }

            public DateTime? RequiredDate { get; set; }

            public DateTime? ShippedDate { get; set; }

            public int? ShipVia { get; set; }

            public decimal? Freight { get; set; }

            public string ShipName { get; set; }

            public string ShipAddress { get; set; }

            public string ShipCity { get; set; }

            public string ShipRegion { get; set; }

            public string ShipPostalCode { get; set; }

            public string ShipCountry { get; set; }

            // Lazy field
            public Customer Customer { get; set; }
        }

        private class CustomerFake
        {
            public string CustomerID { get; set; }

            public string CompanyName { get; set; }

            public string ContactName { get; set; }

            public string ContactTitle { get; set; }

            public string Address { get; set; }

            public string City { get; set; }

            public string Region { get; set; }

            public string PostalCode { get; set; }

            public string Country { get; set; }

            public string Phone { get; set; }

            public string Fax { get; set; }
        }
    }
}
