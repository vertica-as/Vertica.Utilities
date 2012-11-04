using System;

namespace Vertica.Utilities_v4
{
	public static class Guard
	{
		public static void Against(bool assertion)
		{
			Against<InvalidOperationException>(assertion);
		}

		/// <summary>
		/// Will throw a <see cref="InvalidOperationException"/> if the assertion  is true, with the specificied formatted message.
		/// </summary>
		public static void Against(bool assertion, string message, params string[] formattingArguments)
		{
			Against<InvalidOperationException>(assertion, message, formattingArguments);
		}

		/// <summary>
		/// Will throw exception of type <typeparamref name="TException"/> with the specified message if the assertion is true
		/// </summary>
		public static void Against<TException>(bool assertion, string message, params string[] formattingArguments) where TException : Exception
		{
			if (!assertion) return;
			ExceptionHelper.Throw<TException>(message, formattingArguments);
		}

		public static void Against<TException>(bool assertion) where TException : Exception, new()
		{
			if (!assertion) return;
			throw new TException();
		}

		/// <summary>
		/// Will throw a <see cref="ArgumentNullException"/> if object is null.
		/// </summary>
		public static void AgainstNullArgument(string paramName, object param)
		{
			AgainstArgument<ArgumentNullException>(paramName, param == null);
		}

		/// <summary>
		/// Will throw a <see cref="ArgumentException"/> if the assertion  is true.
		/// </summary>
		public static void AgainstArgument(string paramName, bool assertion)
		{
			if (!assertion) return;
			ExceptionHelper.ThrowArgumentException(paramName);
		}

		/// <summary>
		/// Will throw a <see cref="ArgumentException"/> if the assertion  is true, with the specificied formatted message.
		/// </summary>
		public static void AgainstArgument(string paramName, bool assertion, string message, params string[] formattingArguments)
		{
			if (!assertion) return;
			ExceptionHelper.ThrowArgumentException(paramName, message, formattingArguments);
		}

		/// <summary>
		/// Will throw a <see cref="TArgumentException"/> if the assertion  is true.
		/// </summary>
		public static void AgainstArgument<TArgumentException>(string paramName, bool assertion) where TArgumentException : ArgumentException
		{
			if (!assertion) return;
			ExceptionHelper.ThrowArgumentException<TArgumentException>(paramName);
		}

		/// <summary>
		/// Will throw a <see cref="TArgumentException"/> if the assertion  is true, with the specificied formatted message.
		/// </summary>
		public static void AgainstArgument<TArgumentException>(string paramName, bool assertion, string message, params string[] formattingArguments) where TArgumentException : ArgumentException
		{
			if (!assertion) return;
			ExceptionHelper.ThrowArgumentException<TArgumentException>(paramName, message, formattingArguments);
		}
	}
}
