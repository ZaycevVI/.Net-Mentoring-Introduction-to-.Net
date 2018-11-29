using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;
using System.IO;

namespace IntroductionSamples
{
	[TestClass]
	public class JavaScriptSerializerSamples
	{
		const string FileName = "JavaScriptSerializer.json";

		[TestMethod]
		public void Serialize()
		{
			var serializer = new JavaScriptSerializer();
			var str = serializer.Serialize(Person.Instance);

			File.WriteAllText(FileName, str);
		}

		[TestMethod]
		public void Deserialize()
		{
			var serializer = new JavaScriptSerializer();
			var str = File.ReadAllText(FileName);
			var person = serializer.Deserialize<Person>(str);

			Console.WriteLine(person);
		}
	}
}
