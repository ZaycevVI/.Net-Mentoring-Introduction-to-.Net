using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace CustomizationMethodSamples.TestHelpers
{
	public class SoapFormatterTester<T> : SerializationTester<T, SoapFormatter>
	{
		public SoapFormatterTester(SoapFormatter serializer, bool showResult = false) : base(serializer, showResult)
		{ }

		internal override T Deserialization(MemoryStream stream)
		{
			return (T)serializer.Deserialize(stream);
		}

		internal override void Serialization(T data, MemoryStream stream)
		{
			serializer.Serialize(stream, data);
		}
	}
}
