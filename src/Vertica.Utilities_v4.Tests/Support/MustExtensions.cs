using Testing.Commons;

namespace Vertica.Utilities_v4.Tests.Support
{
	internal static class MustExtensions
	{
		public static AgeConstraint Age(this Must.BeEntryPoint entry)
		{
			return new AgeConstraint();
		}
	}
}