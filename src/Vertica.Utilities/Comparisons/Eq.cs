using System;

namespace Vertica.Utilities.Comparisons
{
	public static class Eq<T>
	{
		public static ChainableEqualizer<T> By<K>(Func<T, K> keySelector)
		{
			return new SelectorEqualizer<T, K>(keySelector);
		}

		public static ChainableEqualizer<T> By(Func<T, T, bool> equals, Func<T, int> hasher)
		{
			return new DelegatedEqualizer<T>(equals, hasher);
		}
	}
}