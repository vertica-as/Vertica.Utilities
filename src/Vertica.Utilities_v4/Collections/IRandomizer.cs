using System.Collections.Generic;

namespace Vertica.Utilities_v4.Collections
{
	public interface IRandomizer : IEnumerable<int>
	{
		int Next(int maxValue); 
	}
}