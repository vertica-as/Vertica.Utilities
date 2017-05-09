using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	internal class PropertyIEqualizer : ChainableEqualizer<EqualitySubject>
	{
		protected override bool DoEquals(EqualitySubject x, EqualitySubject y)
		{
			return x.I.Equals(y.I);
		}

		protected override int DoGetHashCode(EqualitySubject obj)
		{
			return obj.I.GetHashCode();
		}
	}
}