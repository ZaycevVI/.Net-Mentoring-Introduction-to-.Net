using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ManageSerializationSamples.TestHelpers;
using System.Web.Script.Serialization;

namespace ManageSerializationSamples
{
	[TestClass]
	public class HierarhySample
	{
		[KnownType(typeof(B)), KnownType(typeof(C))]
		[XmlInclude(typeof(B)), XmlInclude(typeof(C))]
		public class A
		{
			public string PropertyA { get; set; }
		}

		public class B : A
		{
			public string PropertyB { get; set; }
		}

		public class C : A
		{
			public string PropertyC { get; set; }
		}

		public A[] aArray = new A[]
				{
					new A { PropertyA = "A" },
					new B { PropertyA = "A", PropertyB = "B"},
					new C { PropertyA = "A", PropertyC = "C"}
				};

		[TestMethod]
		public void XmlSerializer()
		{
			var tester = new XmlSerializerTester<A[]>(
				new XmlSerializer(typeof(A[])));
			tester.SerializeAndDeserialize(aArray);
		}

		[TestMethod]
		public void DataContractSerializer()
		{
			var tester = new DataContractSerializerTester<A[]>(
				new DataContractSerializer(typeof(A[])));
			var result = tester.SerializeAndDeserialize(aArray);
		}

		[TestMethod]
		public void JavaScriptSerializer()
		{
			var tester = new JavaScriptSerializerTester<A[]>(
				new JavaScriptSerializer());
			var result = tester.SerializeAndDeserialize(aArray);
		}
	}
}
