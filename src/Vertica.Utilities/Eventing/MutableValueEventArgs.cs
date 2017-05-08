using System;

namespace Vertica.Utilities_v4.Eventing
{
	public class MutableValueEventArgs<T> : EventArgs, IMutableValueEventArgs<T>
	{
		public T Value { get; set; }
	}
}