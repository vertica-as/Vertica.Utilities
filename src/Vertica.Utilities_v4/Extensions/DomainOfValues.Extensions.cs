using System.Collections.Generic;

namespace Vertica.Utilities_v4.Extensions.DomainExt
{
	public static class DomainOfValuesExtensions
	{
		public static bool CheckAgainst<TDomain>(this TDomain actualValue, params TDomain[] expectedDomainValues)
		{
			return DomainOf.Values(expectedDomainValues).CheckContains(actualValue);
		}

		public static bool CheckAgainst<TDomain>(this TDomain actualValue, IEnumerable<TDomain> expectedDomainValues)
		{
			return DomainOf.Values(expectedDomainValues).CheckContains(actualValue);
		}

		public static void AssertAgainst<TDomain>(this TDomain actualValue, params TDomain[] expectedDomainValues)
		{
			DomainOf.Values(expectedDomainValues).AssertContains(actualValue);
		}

		public static void AssertAgainst<TDomain>(this TDomain actualValue, IEnumerable<TDomain> expectedDomainValues)
		{
			DomainOf.Values(expectedDomainValues).AssertContains(actualValue);
		}
	}
}
