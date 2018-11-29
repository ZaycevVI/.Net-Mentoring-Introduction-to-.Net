using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using ManageSerializationSamples.TestHelpers;

namespace ManageSerializationSamples
{
	[TestClass]
	public class CustomizeOutputSample
	{
		[XmlType(TypeName = "ClassA")]
		public class A
		{
			[XmlAttribute]
			public string PropertyA { get; set; }
			[XmlAttribute("B")]
			public string PropertyB { get; set; }
			[XmlElement("C")]
			public string PropertyC { get; set; }
		}

		[TestMethod]
		public void Serialize()
		{
			var tester = new XmlSerializerTester<A>(new XmlSerializer(typeof(A)));
			tester.SerializeAndDeserialize(
				new A
				{
					PropertyA = "A",
					PropertyB = "B",
					PropertyC = "C"
				});
		}
	}
}
