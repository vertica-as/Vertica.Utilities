namespace Vertica.Utilities.Eventing
{
	public class ValueIndexCancelEventArgs<T> : ValueIndexEventArgs<T>, ICancelEventArgs
	{
		public ValueIndexCancelEventArgs(T value, int index) : base(value, index) { }

		public bool IsCanceled { get; private set; }

		public void Cancel()
		{
			IsCanceled = true;
		}
	}
}