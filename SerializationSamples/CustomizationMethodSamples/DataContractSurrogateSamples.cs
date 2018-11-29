using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomizationMethodSamples.TestHelpers;
using System.Runtime.Serialization;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CustomizationMethodSamples
{
	public class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public Credentials Credentials { get; set; }
	}

	public class Credentials
	{
		public string Login { get; set; }
		public string Password { get; set; }
	}

	public class PersonSurrogate : IDataContractSurrogate
	{
		#region Not implemented
		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			throw new NotImplementedException();
		}

		public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
		{
			throw new NotImplementedException();
		}
		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			throw new NotImplementedException();
		}

		public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			throw new NotImplementedException();
		}
		public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
		{
			throw new NotImplementedException();
		}
		#endregion

		public Type GetDataContractType(Type type)
		{
			if (type == typeof(Credentials))
				return typeof(SecurityCredentials);
			return type;
		}

		public object GetObjectToSerialize(object obj, Type targetType)
		{
			Credentials credentials = obj as Credentials;
			if (credentials != null)
			{
				var result = new SecurityCredentials();
				result.SecurityString = credentials.Login + ":" + credentials.Password;
				return result;
			}
			return obj;
		}

		public object GetDeserializedObject(object obj, Type targetType)
		{
			SecurityCredentials scred = obj as SecurityCredentials;
			if (scred != null)
			{
				var creds = scred.SecurityString.Split(':');
				return new Credentials { Login = creds[0], Password = creds[1] };
			}
			return obj;
		}
	}

	public class SecurityCredentials
	{
		public string SecurityString { get; set; }
	}

	[TestClass]
	public class DataContractSurrogateSamples
	{
		Person person = new Person
		{
			Name = "Peter Abel",
			Age = 32,
			Credentials = new Credentials
			{
				Login = "peterab",
				Password = "123"
			}
		};

		[TestMethod]
		public void NoSurrogateSample()
		{
			var tester = new XmlDataContractSerializerTester<Person>(
				new DataContractSerializer(typeof(Person)), true);
			tester.SerializeAndDeserialize(person);
		}

		[TestMethod]
		public void SurrogateSample()
		{
			var tester = new XmlDataContractSerializerTester<Person>(
				new DataContractSerializer(
					typeof(Person), 
					new DataContractSerializerSettings {
						DataContractSurrogate = new PersonSurrogate()
					}), true);
			var person2 = tester.SerializeAndDeserialize(person);

			Console.WriteLine("{0} : {1}", person2.Credentials.Login, person2.Credentials.Password);
		}
	}
}
