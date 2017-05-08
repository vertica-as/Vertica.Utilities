using System.Collections.Generic;

namespace Vertica.Utilities_v4
{
	public interface IMapper<in TFrom, TTo>
	{
		TTo Map(TFrom from);
		TTo Map(TFrom from, TTo defaultTo);
		IEnumerable<TTo> Map(IEnumerable<TFrom> from);
	}
}
