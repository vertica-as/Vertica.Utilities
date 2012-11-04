namespace Vertica.Utilities_v4.Eventing
{
	public class MultiEventArgs<T, U> : ValueEventArgs<T>
	{
		public MultiEventArgs(T value, U value2) : base(value)
		{
			Value2 = value2;
		}

		public U Value2 { get; private set; }
	}
}