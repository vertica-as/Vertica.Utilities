using System;

namespace Vertica.Utilities.Eventing
{
	public class MutableValueEventArgs<T> : EventArgs, IMutableValueEventArgs<T>
	{
		public T Value { get; set; }
	}
}