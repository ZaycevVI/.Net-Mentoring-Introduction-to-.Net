using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomizationMethodSamples.TestHelpers
{
	public class XmlDataContractSerializerTester<T> : SerializationTester<T, DataContractSerializer>
	{
		public XmlDataContractSerializerTester(
			DataContractSerializer serializer, bool showResult = false) : base(serializer, showResult)
		{ }

		internal override T Deserialization(MemoryStream stream)
		{
			return (T)serializer.ReadObject(stream);
		}

		internal override void Serialization(T data, MemoryStream stream)
		{
			serializer.WriteObject(stream, data);
		}
	}
}
