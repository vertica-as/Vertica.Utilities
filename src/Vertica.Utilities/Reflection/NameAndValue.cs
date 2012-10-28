using System;
using System.Linq.Expressions;

namespace Vertica.Utilities.Reflection
{
	public static class Name
	{
		public static string Of<T>(Expression<Func<T>> argumentExpression)
		{
			var memberExp = (MemberExpression)argumentExpression.Body;
			return memberExp.Member.Name;
		}

		public static string Of<T, U>(Expression<Func<T, U>> argumentExpression)
		{
			var memberExp = (MemberExpression)argumentExpression.Body;
			return memberExp.Member.Name;
		}
	}

	public static class Value
	{
		public static T Of<T>(Expression<Func<T>> argumentExpression)
		{
			Func<T> func = argumentExpression.Compile();
			return func();
		}
	}
}
