namespace Vertica.Utilities.Eventing
{
	public interface IValueEventArgs<out T> { T Value { get; } }

	public interface ICancelEventArgs
	{
		bool IsCancelled { get; }
		void Cancel();
	}

	public interface INewValueEventArgs<out T> { T NewValue { get; } }

	public interface IOldValueEventArgs<out T> { T OldValue { get; } }

	public interface IIndexEventArgs { int Index { get; } }

	public interface IMutableValueEventArgs<T> { T Value { get; set; } }
}
