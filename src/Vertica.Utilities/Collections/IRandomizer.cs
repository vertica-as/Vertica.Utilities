using System.Collections.Generic;

namespace Vertica.Utilities.Collections
{
	public interface IRandomizer : IEnumerable<int>
	{
		int Next(int maxValue); 
	}
}