using System.Xml.Linq;
using Testing.Commons;

namespace Vertica.Utilities_v4.Tests.Web.Support
{
	public static partial class MustExtensions
	{
		public static EquivalentSiteMapConstraint EquivalentTo(this Must.BeEntryPoint entry, XDocument expected)
		{
			return new EquivalentSiteMapConstraint(expected);
		}
	}
}