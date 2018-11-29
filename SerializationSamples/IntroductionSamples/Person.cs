using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionSamples
{
	public enum Gender
	{
		Male,
		Female
	}

	[Serializable]
	public class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public Gender Gender { get; set; }


		public override string ToString()
		{
			return string.Format("Name = {0}, Age = {1}, Gender = {2}",
				Name, Age, Gender);
		}


		public static Person Instance = new Person() { Name = "Smit", Age = 22, Gender = Gender.Male };
	}
}
