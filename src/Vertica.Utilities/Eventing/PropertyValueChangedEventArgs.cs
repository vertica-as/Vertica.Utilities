using System.ComponentModel;

namespace Vertica.Utilities_v4.Eventing
{
	public class PropertyValueChangedEventArgs<T> : PropertyChangedEventArgs, IOldValueEventArgs<T>, INewValueEventArgs<T>
	{
		public PropertyValueChangedEventArgs(string propertyName) : base(propertyName) { }

		public PropertyValueChangedEventArgs(string propertyName, T oldValue, T newValue) : this(propertyName)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public T OldValue { get; private set; }
		public T NewValue { get; private set; }
	}
}