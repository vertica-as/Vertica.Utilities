using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4
{
	public interface IMapper<in TFrom, TTo>
	{
		TTo Map(TFrom from);
		TTo Map(TFrom from, TTo defaultTo);
		IEnumerable<TTo> Map(IEnumerable<TFrom> from);
	}

	public abstract class ClassMapper<TFrom, TTo> : IMapper<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		public TTo Map(TFrom from)
		{
			return ClassMapper.MapIfNotNull(from, () => MapOne(from));
		}

		public TTo Map(TFrom from, TTo defaultTo)
		{
			return ClassMapper.MapIfNotNull(from, () => MapOne(from), defaultTo);
		}

		public IEnumerable<TTo> Map(IEnumerable<TFrom> from)
		{
			return ClassMapper.MapIfNotNull(from, partialFrom => ClassMapper.MapIfNotNull(partialFrom, () => Map(partialFrom)));
		}

		public abstract TTo MapOne(TFrom from);
	}

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
