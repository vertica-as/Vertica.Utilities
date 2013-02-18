using System.Collections;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Collections
{
	/// <summary>
	/// Type chaining an IEnumerable&lt;T&gt; to allow the iterating code
	/// to detect the first and last entries simply.
	/// </summary>
	/// <typeparam name="T">Type to iterate over</typeparam>
	public class SmartEnumerable<T> : IEnumerable<SmartEntry<T>>
	{
		/// <summary>
		/// Enumerable we proxy to
		/// </summary>
		readonly IEnumerable<T> _wrapped;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="enumerable">Collection to enumerate. Must not be null.</param>
		public SmartEnumerable(IEnumerable<T> enumerable)
		{
			Guard.AgainstNullArgument("enumerable", enumerable);
			_wrapped = enumerable;
		}

		/// <summary>
		/// Returns an enumeration of Entry objects, each of which knows
		/// whether it is the first/last of the enumeration, as well as the
		/// current value.
		/// </summary>
		public IEnumerator<SmartEntry<T>> GetEnumerator()
		{
			using (IEnumerator<T> enumerator = _wrapped.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					yield break;
				}
				bool isFirst = true;
				bool isLast = false;
				int index = 0;
				while (!isLast)
				{
					T current = enumerator.Current;
					isLast = !enumerator.MoveNext();
					yield return new SmartEntry<T>(isFirst, isLast, current, index++);
					isFirst = false;
				}
			}
		}

		/// <summary>
		/// Non-generic form of GetEnumerator.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
	}
}