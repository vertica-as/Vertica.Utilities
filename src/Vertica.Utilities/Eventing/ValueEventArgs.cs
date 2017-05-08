using System;

namespace Vertica.Utilities_v4.Eventing
{
	public class ValueEventArgs<T> : EventArgs, IValueEventArgs<T>
	{
		public ValueEventArgs(T value)
		{
			Value = value;
		}

		public T Value { get; private set; }
	}
}