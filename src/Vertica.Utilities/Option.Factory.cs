namespace Vertica.Utilities
{
	public static class Option
	{
		public static Option<T> Some<T>(T value)
		{
			return Option<T>.Some(value);
		}

		public static Option<T> None<T>(T defaultValue)
		{
			return Option<T>.NoneWithDefault(defaultValue);
		}

		public static Option<T> Maybe<T>(T value)  where T : class
		{
			return value == null ? Option<T>.None : Some(value);
		}
	}
}
