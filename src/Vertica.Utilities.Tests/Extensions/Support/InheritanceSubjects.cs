using System;

namespace Vertica.Utilities.Tests.Extensions.Support
{
	public class BaseType
	{
		public BaseType(string name)
		{
			Name = name;
		}

		public string Name;

		public override string ToString()
		{
			return "BaseType.Name=" + Name;
		}
	}

	public class DerivedType : BaseType
	{
		public DerivedType(string name, int id)
			: base(name)
		{
			ID = id;
		}

		public int ID;

		public override string ToString()
		{
			return string.Format("DerivedType.Name={0}, ID={1}", Name, ID);
		}
	}
}