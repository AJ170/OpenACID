using System;

namespace Database
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DatabaseAttribute : Attribute
	{
		public string Name { get; private set; }

		public DatabaseAttribute(string _name)
		{
			Name = _name;
		}
	}
}
