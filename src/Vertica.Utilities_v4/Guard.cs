using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertica.Utilities_v4.Extensions.StringExt;

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

		public static void AgainstNullArgument<T>(T container) where T: class
		{
			if (container == null) throw new ArgumentNullException("container");

			NullChecker<T>.Check(container);
		}

		private static class NullChecker<T> where T : class
		{
			private static readonly Func<T, string> _nullSeeker;

			static NullChecker()
			{

				Expression body = Expression.Constant(null, typeof(string));
				var param = Expression.Parameter(typeof(T), "obj");

				foreach (PropertyInfo property in typeof(T).GetProperties())
				{
					Type propType = property.PropertyType;
					if (propType.IsValueType && Nullable.GetUnderlyingType(propType) == null)
					{
						continue; // can't be null
					}

					body = Expression.Condition(
						Expression.Equal(
							Expression.Property(param, property),
							Expression.Constant(null, propType)),
						Expression.Constant(property.Name, typeof(string)),
						body);
				}
				_nullSeeker = Expression.Lambda<Func<T, string>>(body, param).Compile();
			}

			internal static void Check(T item)
			{
				string nullArg = _nullSeeker(item);
				if (nullArg.IsNotEmpty())
				{
					throw new ArgumentNullException(nullArg);
				}
			}

		}
	}
}
