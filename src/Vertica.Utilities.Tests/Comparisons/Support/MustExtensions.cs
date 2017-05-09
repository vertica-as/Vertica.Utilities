using Testing.Commons;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	internal static class MustExtensions
	{
		internal static StringRepresentationConstraint RepresentableAs(this Must.BeEntryPoint entry, string representation)
		{
			return new StringRepresentationConstraint(representation);
		}
	}
}