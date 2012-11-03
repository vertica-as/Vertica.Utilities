namespace Vertica.Utilities.Eventing
{
	public class ValueIndexCancelEventArgs<T> : ValueIndexEventArgs<T>, ICancelEventArgs
	{
		public ValueIndexCancelEventArgs(int index, T value) : base(index, value) { }

		public bool IsCancelled { get; private set; }

		public void Cancel()
		{
			IsCancelled = true;
		}
	}
}