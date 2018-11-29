using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CustomizationMethodSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Text;

namespace CustomizationMethodSamples
{
	[Serializable]
	public class C
	{
		public string PropertyA { get; set; }

		public int PropertyB { get; set; }

		public C()
		{
			Console.WriteLine("Call C constructor");
		}

		[OnSerializing]
		public void OnSerializing(StreamingContext context)
		{
			var slot = Thread.AllocateNamedDataSlot("C");
			Thread.SetData(slot, PropertyA);

			PropertyA = Convert.ToBase64String(Encoding.UTF8.GetBytes(PropertyA));

		}

		[OnSerialized]
		public void OnSerialized(StreamingContext context)
		{
			var slot = Thread.GetNamedDataSlot("C");
			PropertyA = (string) Thread.GetData(slot);
		}

		
		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			PropertyA = Encoding.UTF8.GetString(Convert.FromBase64String(PropertyA));
		}
	}

	[TestClass]
	public class SerializationEventSamples
	{
		
		[TestMethod]
		public void XmlDataContractSerializer()
		{
			var tester = new XmlDataContractSerializerTester<C>(
				new DataContractSerializer(typeof(C)), true);
			var c1 = new C() { PropertyA = "11111111" };
			var c2 = tester.SerializeAndDeserialize(c1);

			Console.WriteLine(c1.PropertyA);
			Console.WriteLine(c2.PropertyA);
		}
	}
}
