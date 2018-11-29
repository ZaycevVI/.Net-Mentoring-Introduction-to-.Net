using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using ManageSerializationSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ManageSerializationSamples
{
	[TestClass]
	public class GraphSerialization
	{
		[Serializable]
		[DataContract]
		public class A
		{
			[DataMember]
			public int P1 { get; set; }
		}

		[Serializable]
		[DataContract]
		public class B
		{
			[DataMember]
			public A A1 { get; set; }

			[DataMember]
			public A A2 { get; set; }
		}

		public B b;

		[TestInitialize]
		public void Initialize()
		{
			A a = new A { P1 = 1 };
			b = new B { A1 = a, A2 = a };
		}

		[TestMethod]
		public void XmlSerializer()
		{
			Console.WriteLine(Object.ReferenceEquals(b.A1, b.A2));
			var tester = new XmlSerializerTester<B>(new XmlSerializer(typeof(B)));
			var result = tester.SerializeAndDeserialize(b);
			Console.WriteLine(Object.ReferenceEquals(result.A1, result.A2));
		}

		[TestMethod]
		public void DataContractSerializerTree()
		{
			Console.WriteLine(Object.ReferenceEquals(b.A1, b.A2));
			var tester = new DataContractSerializerTester<B>(new DataContractSerializer(typeof(B)));
			var result = tester.SerializeAndDeserialize(b);
			Console.WriteLine(Object.ReferenceEquals(result.A1, result.A2));
		}


		[TestMethod]
		public void DataContractSerializerGraph()
		{
			Console.WriteLine(Object.ReferenceEquals(b.A1, b.A2));
			var tester = new DataContractSerializerTester<B>(new DataContractSerializer(typeof(B), 
				new DataContractSerializerSettings { PreserveObjectReferences = true }));
			var result = tester.SerializeAndDeserialize(b);
			Console.WriteLine(Object.ReferenceEquals(result.A1, result.A2));
		}

		[TestMethod]
		public void BinaryFormatter()
		{
			Console.WriteLine(Object.ReferenceEquals(b.A1, b.A2));
			var tester = new BinaryFormatterTester<B>(new BinaryFormatter());
			var result = tester.SerializeAndDeserialize(b);
			Console.WriteLine(Object.ReferenceEquals(result.A1, result.A2));
		}

	}
}
