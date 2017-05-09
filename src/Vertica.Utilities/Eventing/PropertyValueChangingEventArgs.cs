using System.ComponentModel;

namespace Vertica.Utilities.Eventing
{
	public class PropertyValueChangingEventArgs<T> : PropertyChangingEventArgs, IOldValueEventArgs<T>, INewValueEventArgs<T>, ICancelEventArgs
	{
		public PropertyValueChangingEventArgs(string propertyName) : base(propertyName) { }

		public PropertyValueChangingEventArgs(string propertyName, T oldValue, T newValue) : this(propertyName)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public T OldValue { get; private set; }
		public T NewValue { get; private set; }

		public bool IsCancelled { get; private set; }
		public void Cancel()
		{
			IsCancelled = true;
		}
	}
}