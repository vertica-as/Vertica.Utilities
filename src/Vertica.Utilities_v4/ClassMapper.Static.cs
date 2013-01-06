using System.Collections.Generic;

namespace Vertica.Utilities_v4
{
	public abstract class ClassMapper<TFrom, TTo> : IMapper<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		public TTo Map(TFrom from)
		{
			return ClassMapper.MapIfNotNull(from, MapOne);
		}

		public TTo Map(TFrom from, TTo defaultTo)
		{
			return ClassMapper.MapIfNotNull(from, MapOne, defaultTo);
		}

		public IEnumerable<TTo> Map(IEnumerable<TFrom> from)
		{
			return ClassMapper.MapIfNotNull(from, partialFrom => ClassMapper.MapIfNotNull(partialFrom, Map));
		}

		public abstract TTo MapOne(TFrom from);
	}
}
