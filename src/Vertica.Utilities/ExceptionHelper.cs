using System;

namespace Vertica.Utilities
{
	public static class ExceptionHelper
	{
		public static void Throw<T>(string templateMessage, params string[] arguments) where T : Exception
		{
			string message = string.Format(templateMessage, arguments);
			var ex = (Exception)Activator.CreateInstance(typeof(T), message);
			throw ex;
		}

		public static void Throw<T>(Exception innerException, string templateMessage, params string[] arguments)
			where T : Exception
		{
			string message = string.Format(templateMessage, arguments);
			var ex = (Exception)Activator.CreateInstance(typeof(T), message, innerException);
			throw ex;
		}

		public static void ThrowArgumentException(string paramName, string templateMessage, params string[] arguments)
		{
			string message = string.Format(templateMessage, arguments);
			var ex = new ArgumentException(message, paramName);
			throw ex;
		}

		public static void ThrowArgumentException(string paramName)
		{
			var ex = new ArgumentException(string.Empty, paramName);
			throw ex;
		}

		public static void ThrowArgumentException<T>(string paramName) where T : ArgumentException
		{
			var ex = (ArgumentException)Activator.CreateInstance(typeof(T), paramName);
			throw ex;
		}

		public static void ThrowArgumentException<T>(string paramName, string templateMessage, params string[] arguments) where T : ArgumentException
		{
			string message = string.Format(templateMessage, arguments);
			var ex = (ArgumentException)Activator.CreateInstance(typeof(T), paramName, message);
			throw ex;
		}
	}
}
