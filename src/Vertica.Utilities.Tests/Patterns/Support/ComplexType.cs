namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	internal class ComplexType
	{
		public ComplexType(bool enabled, string foo, int bar)
		{
			Enabled = enabled;
			Foo = foo;
			Bar = bar;
		}

		public string Foo { get; private set; }
		public int Bar { get; private set; }
		public bool Enabled { get; private set; }
	}
}
