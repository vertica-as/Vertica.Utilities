using Testing.Commons;

namespace Vertica.Utilities.Tests.Support
{
	internal static class MustExtensions
	{
		public static AgeConstraint Age(this Must.BeEntryPoint entry)
		{
			return new AgeConstraint();
		}
	}
}