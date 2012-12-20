using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Hasher
	{
		public static int Default<T>(T t)
		{
			return t.GetHashCode();
		}

		public static int Zero<T>(T t)
		{
			return 0;
		}
	}
}