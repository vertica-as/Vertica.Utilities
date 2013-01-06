using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4
{
	public static class ClassMapper
	{
		public static IEnumerable<TTo> MapIfNotNull<TFrom, TTo>(IEnumerable<TFrom> from, Func<TFrom, TTo> doMapping)
			where TFrom : class
			where TTo : class
		{
			if (from != null)
			{
				foreach (var item in from)
				{
					TFrom partialFrom = item;
					TTo partial = MapIfNotNull(item, () => doMapping(partialFrom));
					if (partial != null) yield return partial;
				}
			}
		}

		public static TTo MapIfNotNull<TFrom, TTo>(TFrom from, Func<TTo> doMapping)
			where TFrom : class
			where TTo : class
		{
			return MapIfNotNull(from, doMapping, null);
		}

		public static TTo MapIfNotNull<TFrom, TTo>(TFrom from, Func<TTo> doMapping, TTo defaultTo)
			where TFrom : class
			where TTo : class
		{
			TTo to = defaultTo;
			if (from != null)
			{
				to = doMapping();
			}
			return to;
		}
	}

	
}