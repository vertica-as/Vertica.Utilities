using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities_v4.Collections
{
	public static class Chain
	{
		public static IEnumerable<T> From<T>(params T[] items)
		{
			return items;
		}

		public static IEnumerable<T> Empty<T>()
		{
			return Enumerable.Empty<T>();
		}

		public static IEnumerable<T> Null<T>()
		{
			return null;
		}
	}
}