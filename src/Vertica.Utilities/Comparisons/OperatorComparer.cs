using System;
using System.Linq.Expressions;

namespace Vertica.Utilities.Comparisons
{
	public class OperatorComparer<T> : ChainableComparer<T>
	{
		private static class Operations
		{
			internal static readonly Func<T, T, bool> gt, lt;
			static Operations()
			{
				Type t = typeof(T);
				ParameterExpression param1 = Expression.Parameter(t, "x");
				ParameterExpression param2 = Expression.Parameter(t, "y");

				gt = Expression.Lambda<Func<T, T, bool>>(
					Expression.GreaterThan(param1, param2), param1, param2)
					.Compile();

				lt = Expression.Lambda<Func<T, T, bool>>(
					Expression.LessThan(param1, param2), param1, param2)
					.Compile();
			}
		}

		public OperatorComparer(Direction direction = Direction.Ascending): base(direction) { }

		protected override int DoCompare(T x, T y)
		{
			return throwingMeaningfulException(() =>
			{
				if (Operations.gt(x, y)) return 1;
				if (Operations.lt(x, y)) return -1;
				return 0;
			});
			
		}

		private int throwingMeaningfulException(Func<int> comparison)
		{
			try
			{
				return comparison();
			}
			catch (TypeInitializationException ex)
			{
				// throw the more expressive exception of the static constructor
				if (ex.InnerException != null) throw ex.InnerException;
				else throw;
			}
		}
	}
}
