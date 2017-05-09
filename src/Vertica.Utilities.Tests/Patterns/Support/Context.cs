namespace Vertica.Utilities.Tests.Patterns.Support
{
	internal class Context
	{
		public Context(string s)
		{
			S = s;
		}

		public string S { get; set; }
	}
}