using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using ManageSerializationSamples.TestHelpers;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

namespace ManageSerializationSamples
{
	[TestClass]
	public class DeserializationSample
	{
		[Serializable]
		public class A
		{
			public string PropertyA { get; set; }
			public int PropertyB { get; set; }

			public A()
			{
				Console.WriteLine("Call A constructor");
			}

			[OnSerializing]
			public void OnSerializing(StreamingContext context)
			{
				Console.WriteLine("OnSerializing");
			}
			[OnSerialized]
			public void OnSerialized(StreamingContext context)
			{
				Console.WriteLine("OnSerialized");
			}
			[OnDeserializing]
			public void OnDeserializing(StreamingContext context)
			{
				Console.WriteLine("OnDeserializing");
			}
			[OnDeserialized]
			public void OnDeserialized(StreamingContext context)
			{
				Console.WriteLine("OnDeserialized");
			}
		}


		[TestMethod]
		public void XmlSerializer()
		{
			var tester = new XmlSerializerTester<A>(new XmlSerializer(typeof(A)));
			tester.SerializeAndDeserialize(new A());
		}

		[TestMethod]
		public void XmlDataContractSerializer()
		{
			var tester = new DataContractSerializerTester<A>(
				new DataContractSerializer(typeof(A)));
			tester.SerializeAndDeserialize(new A());
		}

		[TestMethod]
		public void JavaScriptSerializer()
		{
			var tester = new JavaScriptSerializerTester<A>(
				new JavaScriptSerializer());
			tester.SerializeAndDeserialize(new A());
		}
	}
}
