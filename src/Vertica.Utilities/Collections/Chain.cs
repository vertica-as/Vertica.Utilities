using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities.Collections
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

		// ReSharper disable FunctionNeverReturns
		public static IEnumerable<T> Of<T>(T element)
		{
			while (true) yield return element;
		}
		// ReSharper restore FunctionNeverReturns
	}
}