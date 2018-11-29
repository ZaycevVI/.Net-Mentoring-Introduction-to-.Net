using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;

namespace IntroductionSamples
{
	[TestClass]
	public class XmlSerializerSamples
	{
		const string FileName = "XmlSerializer.xml";

		[TestMethod]
		public void Serialize()
		{
			var serializer = new XmlSerializer(typeof(Person));
			var stream = new FileStream(FileName, FileMode.Create);
			serializer.Serialize(stream, Person.Instance);
			stream.Close();
		}

		[TestMethod]
		public void Deserialize()
		{
			var serializer = new XmlSerializer(typeof(Person));
			var person = serializer.Deserialize(new FileStream(FileName, FileMode.Open)) as Person;

			Console.WriteLine(person);
		}
	}
}
