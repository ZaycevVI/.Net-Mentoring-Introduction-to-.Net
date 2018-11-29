using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CustomizationMethodSamples.TestHelpers
{
	public class XmlSerializerTester<T> : SerializationTester<T, XmlSerializer>
	{
		public XmlSerializerTester(XmlSerializer serializer) : base(serializer)
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
