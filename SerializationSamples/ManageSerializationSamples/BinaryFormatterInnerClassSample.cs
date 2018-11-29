using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ManageSerializationSamples.TestHelpers;
using System.Runtime.Serialization;

namespace ManageSerializationSamples
{
	[TestClass]
	public class BinaryFormatterInnerClassSample
	{
		public class Inner
		{
			public int i;
		}

		[Serializable]
		public class A
		{
			public int j;
			public Inner inner;
		}

		[TestMethod]
        [ExpectedException(typeof(SerializationException))]
		public void NotValidSerialization()
		{
			var tester = new BinaryFormatterTester<A>(new BinaryFormatter());
			tester.SerializeAndDeserialize(new A());
		}
	}
}
