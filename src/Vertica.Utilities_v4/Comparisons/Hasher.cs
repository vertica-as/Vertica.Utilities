using System;

namespace Vertica.Utilities_v4.Comparisons
{
	public static class Hasher
	{
		public static int Default<T>(T t)
		{
			return (t == null) ? 0 : t.GetHashCode();
		}

		public static int Zero<T>(T t)
		{
			return 0;
		}

		public static int Canonical(params object[] args)
		{
			unchecked
			{
				int? hash = null;
				Func<object, int> getHashCode = o => o != null ? o.GetHashCode() : 0;
				for (int i = 0; i < args.Length; i++)
				{
					if (!hash.HasValue)
					{
						hash = getHashCode(args[i]);
					}
					else
					{
						hash = (hash * 397) ^ getHashCode(args[i]);
					}
				}
				return hash.GetValueOrDefault();
			}
		}
	}
}