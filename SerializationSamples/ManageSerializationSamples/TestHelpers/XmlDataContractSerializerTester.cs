using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManageSerializationSamples.TestHelpers
{
	public class DataContractSerializerTester<T> : SerializationTester<T, DataContractSerializer>
	{
		public DataContractSerializerTester(
			DataContractSerializer serializer) : base(serializer, true)
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
