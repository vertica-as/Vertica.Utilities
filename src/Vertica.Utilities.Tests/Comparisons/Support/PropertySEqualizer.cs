using Vertica.Utilities_v4.Comparisons;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class PropertySEqualizer : ChainableEqualizer<EqualitySubject>
	{
		protected override bool DoEquals(EqualitySubject x, EqualitySubject y)
		{
			return x.S.Equals(y.S);
		}

		protected override int DoGetHashCode(EqualitySubject obj)
		{
			return obj.S.GetHashCode();
		}
	}
}
