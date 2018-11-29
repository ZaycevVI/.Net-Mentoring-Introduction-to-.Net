using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace IntroductionSamples
{
	[TestClass]
	public class BinaryFormatterSamples
	{
		const string FileName = "BinaryFormatter.dat";

		[TestMethod]
		public void Serialize()
		{			
			var serializer = new BinaryFormatter();
			var stream = new FileStream(FileName, FileMode.Create);
			serializer.Serialize(stream, Person.Instance);
			stream.Close();
		}

		[TestMethod]
		public void Deserialize()
		{
			var serializer = new BinaryFormatter();
			var person = serializer.Deserialize(new FileStream(FileName, FileMode.Open)) as Person;

			Console.WriteLine(person);
		}
	}
}
