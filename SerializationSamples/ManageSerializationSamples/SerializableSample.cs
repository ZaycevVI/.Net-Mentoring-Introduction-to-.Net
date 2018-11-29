using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;
using ManageSerializationSamples.TestHelpers;

namespace ManageSerializationSamples
{
	[TestClass]
	public class SerializableSample
	{
		public class A
		{
			public string PropertyA { get; set; }
			[XmlIgnore]
			public string PropertyB { get; set; }
		}

		[TestMethod]
		public void Serialization()
		{
			var tester = new XmlSerializerTester<A>(
				new XmlSerializer(typeof(A)));

			tester.SerializeAndDeserialize(
				new A { PropertyA = "A", PropertyB = "B" });
		}
	}
}
