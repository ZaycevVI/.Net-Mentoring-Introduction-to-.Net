using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CustomizationMethodSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CustomizationMethodSamples
{
	[Serializable]
	public class D : ISerializable
	{
		
		public string PropertyA { get; set; }

		public int PropertyB { get; set; }

		public D()
		{
			Console.WriteLine("Call default D constructor");
		}

		private D(SerializationInfo information, StreamingContext context)
		{
			PropertyA = information.GetString("PropertyA");
			PropertyB = information.GetInt32("PropertyB");
		}


		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("PropertyA", PropertyA);
			info.AddValue("PropertyB", PropertyB);
			info.AddValue("TimeStamp", DateTime.UtcNow);
		}
	}

	[TestClass]
	public class ISerializableSamples
	{

		[TestMethod]
		public void XmlDataContractSerializer()
		{
			var tester = new XmlDataContractSerializerTester<D>(
				new DataContractSerializer(typeof(D)), true);
			var d1 = new D() { PropertyA = "AAAAA" };
			var d2 = tester.SerializeAndDeserialize(d1);

			Console.WriteLine(d1.PropertyA);
			Console.WriteLine(d2.PropertyA);

		}
	}
}
