using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ManageSerializationSamples
{
	[TestClass]
	public class Versioning
	{
		[Serializable]
		public class Address
		{
			public string Street;
			public string City;

			[OptionalField(VersionAdded = 2)]
			public string Country;
		}


		const string FileName = "Versioning.dat";

		[TestMethod]
		public void Serialize()
		{
			var serializer = new BinaryFormatter();
			var stream = new FileStream(FileName, FileMode.Create);
			serializer.Serialize(stream, new Address { Street = "Street", City = "City", /*Country = "Country"*/ });
			stream.Close();
		}

		[TestMethod]
		public void Deserialize()
		{
			var serializer = new BinaryFormatter();
			var person = serializer.Deserialize(new FileStream(FileName, FileMode.Open)) as Address;

			Console.WriteLine(person);
		}
	}
}
