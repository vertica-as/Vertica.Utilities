using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons.Support
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
