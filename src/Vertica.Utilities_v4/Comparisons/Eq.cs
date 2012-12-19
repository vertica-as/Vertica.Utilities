using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Eq<T>
	{
		public static ChainableEqualizer<T> By<K>(Func<T, K> keySelector)
		{
			return new SelectorEqualizer<T, K>(keySelector);
		}
	}
}