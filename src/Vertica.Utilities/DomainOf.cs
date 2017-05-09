using System.Collections.Generic;

namespace Vertica.Utilities
{
	public static class DomainOf
	{
		public static DomainOfValues<T> Values<T>(params T[] expectedValues)
		{
			return new DomainOfValues<T>(expectedValues);
		}

		public static DomainOfValues<T> Values<T>(IEnumerable<T> expectedValues)
		{
			return new DomainOfValues<T>(expectedValues);
		}
	}
}