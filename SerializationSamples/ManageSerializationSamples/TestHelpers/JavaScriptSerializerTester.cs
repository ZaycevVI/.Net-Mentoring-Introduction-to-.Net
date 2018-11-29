using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace ManageSerializationSamples.TestHelpers
{
	public class JavaScriptSerializerTester<T> : SerializationTester<T, JavaScriptSerializer>
	{
		public JavaScriptSerializerTester(JavaScriptSerializer serializer) : base(serializer, true)
		{ }

		internal override T Deserialization(MemoryStream stream)
		{
			return serializer.Deserialize<T>(new StreamReader(stream).ReadToEnd());
		}

		internal override void Serialization(T data, MemoryStream stream)
		{
			var writer = new StreamWriter(stream);
			writer.Write(serializer.Serialize(data));
			writer.Flush();
		}
	}
}
