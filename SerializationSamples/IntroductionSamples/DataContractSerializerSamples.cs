using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.IO;

namespace IntroductionSamples
{
	[TestClass]
	public class DataContractSerializerSamples
	{
		const string FileName = "DataContractSerializer.xml";

		[TestMethod]
		public void Serialize()
		{
			var serializer = new DataContractSerializer(typeof(Person));
			var stream = new FileStream(FileName, FileMode.Create);
			serializer.WriteObject(stream, Person.Instance);
			stream.Close();
		}

		[TestMethod]
		public void Deserialize()
		{
			var serializer = new DataContractSerializer(typeof(Person));
			var person = serializer.ReadObject(new FileStream(FileName, FileMode.Open)) as Person;

			Console.WriteLine(person);
		}
	}
}
