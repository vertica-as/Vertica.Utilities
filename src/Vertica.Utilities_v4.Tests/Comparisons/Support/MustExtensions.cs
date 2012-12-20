using Testing.Commons;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal static class MustExtensions
	{
		internal static StringRepresentationConstraint RepresentableAs(this Must.BeEntryPoint entry, string representation)
		{
			return new StringRepresentationConstraint(representation);
		}
	}
}