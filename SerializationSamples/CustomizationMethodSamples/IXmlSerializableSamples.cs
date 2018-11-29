using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CustomizationMethodSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace CustomizationMethodSamples
{
	[Serializable]
	public class E : IXmlSerializable
	{
		public string PropertyA { get; set; }

		public int PropertyB { get; set; }

		public E()
		{
			Console.WriteLine("Call E constructor");
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement("E");
			PropertyA = reader.ReadElementContentAsString();
			
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("PropertyA");
			writer.WriteValue(PropertyA);
			writer.WriteEndElement();
		}
	}

	[TestClass]
	public class IXmlSerializableSamples
	{
		[TestMethod]
		public void XmlDataContractSerializer()
		{
			var tester = new XmlDataContractSerializerTester<E>(
				new DataContractSerializer(typeof(E)), true);
			var e1 = new E() { PropertyA = "EEEEE" };
			var e2 = tester.SerializeAndDeserialize(e1);

			Console.WriteLine(e1.PropertyA);
			Console.WriteLine(e2.PropertyA);
		}
	}
}
