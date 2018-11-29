using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ManageSerializationSamples.TestHelpers
{
	public class BinaryFormatterTester<T> : SerializationTester<T, BinaryFormatter>
	{
		public BinaryFormatterTester(BinaryFormatter serializer) : base(serializer)
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
