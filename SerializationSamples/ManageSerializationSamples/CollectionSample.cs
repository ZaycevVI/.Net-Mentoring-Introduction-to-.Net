using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ManageSerializationSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ManageSerializationSamples
{
	[TestClass]
	public class CollectionSample
	{
		public class A
		{
			[XmlArray("Cities"), XmlArrayItem("City")]
			public List<string> C { get; set; }

			internal static A Instance = new A
			{
				C = new List<string> { "Moscow", "Izhevsk", "Minsk" }
			};
		}

	
		[TestMethod]
		public void XmlSerializer()
		{
			var tester = new XmlSerializerTester<A>(new XmlSerializer(typeof(A)));
			tester.SerializeAndDeserialize(A.Instance);
		}


		[CollectionDataContract(Name = "Cities", ItemName = "City")]
		public class Cities : List<string> { };

		public class B
		{
			public Cities C { get; set; }

			internal static B Instance = new B
			{
				C = new Cities { "Moscow", "Izhevsk", "Minsk" }
			};

		}
		
		[TestMethod]
		public void DataContractSerializer ()
		{
			var tester = new DataContractSerializerTester<B>(new DataContractSerializer(typeof(B)));
			tester.SerializeAndDeserialize(B.Instance);
		}

	}
}
