using Vertica.Utilities_v4.Comparisons;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class PropertyIEqualizer : ChainableEqualizer<EqualitySubject>
	{
		public override bool DoEquals(EqualitySubject x, EqualitySubject y)
		{
			return x.I.Equals(y.I);
		}
		public override int DoGetHashCode(EqualitySubject obj)
		{
			return obj.I.GetHashCode();
		}
	}
}