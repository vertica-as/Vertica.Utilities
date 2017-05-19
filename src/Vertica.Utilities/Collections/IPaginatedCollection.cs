using System.Collections.Generic;

namespace Vertica.Utilities.Collections
{
	public interface IPaginatedCollection<out T> : IEnumerable<T>
	{
		uint CurrentPage { get; }
		uint Pagesize { get; }
		uint NumberOfPages { get; }
		IEnumerable<T> Collection { get; }
	}
}