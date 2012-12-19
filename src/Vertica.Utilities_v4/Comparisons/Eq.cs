using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Eq<T>
	{
		public static ChainableEqualizer<T> By<K>(Func<T, K> keySelector)
		{
			return new SelectorEqualizer<T, K>(keySelector);
		}

		public static ChainableEqualizer<T> By(Func<T, T, bool> equals)
		{
			return new DelegatedEqualizer<T>(equals);
		}

		public static ChainableEqualizer<T> By(Func<T, T, bool> equals, Func<T, int> hasher)
		{
			return new DelegatedEqualizer<T>(equals, hasher);
		}
	}
}