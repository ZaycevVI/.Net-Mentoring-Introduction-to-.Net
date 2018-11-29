using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CustomizationMethodSamples.TestHelpers;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace CustomizationMethodSamples
{
	[Serializable]
	public class F
	{
		public string PropertyA { get; set; }

		public int PropertyB { get; set; }

		internal int SerializationCounter { get; set; }
	}

	public class SerializationSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var f = (F)obj;
			info.AddValue("PropertyA", f.PropertyA);
			info.AddValue("PropertyB", f.PropertyB);
			info.AddValue("_counter", f.SerializationCounter + 1);
		}

		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var f = (F)obj;
			f.PropertyA = info.GetString("PropertyA");
			f.PropertyB = info.GetInt32("PropertyB");
			f.SerializationCounter = info.GetInt32("_counter");

			return f;

		}
	}

	[TestClass]
	public class ISerializationSurrogateSamples
	{

		[TestMethod]
		public void NoSurrogateSample()
		{
			var tester = new SoapFormatterTester<F>(
				new SoapFormatter(), true);
			var f1 = new F() { PropertyA = "AAAAA" };
			var f2 = tester.SerializeAndDeserialize(f1);

			Console.WriteLine(f1.PropertyA);
			Console.WriteLine(f2.PropertyA);
		}

		[TestMethod]
		public void SurrogateSample()
		{
			var selector = new SurrogateSelector();
			selector.AddSurrogate(
				typeof(F), 
				new StreamingContext(StreamingContextStates.Persistence, null), 
				new SerializationSurrogate());

			var tester = new SoapFormatterTester<F>(
				new SoapFormatter(selector, new StreamingContext()), true);
			var f1 = new F() { PropertyA = "AAAAA" };
			var f2 = 
				tester.SerializeAndDeserialize(
					tester.SerializeAndDeserialize(
						tester.SerializeAndDeserialize(f1)));

			Console.WriteLine(f1.SerializationCounter);
			Console.WriteLine(f2.SerializationCounter);

		}

	}
}
