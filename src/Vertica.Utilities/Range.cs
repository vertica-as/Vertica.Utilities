using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vertica.Utilities.Extensions.ComparableExt;
using Vertica.Utilities.Resources;

namespace Vertica.Utilities
{
	public class Range<T> : IEquatable<Range<T>> where T : IComparable<T>
	{
		private readonly IBound<T> _lowerBound;
		private readonly IBound<T> _upperBound;

		private Range()
		{
			_lowerBound = new Open<T>(default(T));
			_upperBound = new Open<T>(default(T));
		}

		public Range(IBound<T> lowerBound, IBound<T> upperBound)
		{
			AssertBounds(lowerBound, upperBound);

			_lowerBound = lowerBound;
			_upperBound = upperBound;
		}

		public Range(T lowerBound, T upperBound)
		{
			AssertBounds(lowerBound, upperBound);

			_lowerBound = new Closed<T>(lowerBound);
			_upperBound = new Closed<T>(upperBound);
		}

		public T LowerBound { get { return _lowerBound.Value; } }

		public T UpperBound { get { return _upperBound.Value; } }

		public virtual bool Contains(T item)
		{
			return _lowerBound.LessThan(item) && _upperBound.MoreThan(item);
		}

		#region Limit

		public virtual T LimitLower(T value)
		{
			return Limit(value, LowerBound, value);
		}

		public virtual T LimitUpper(T value)
		{
			return Limit(value, value, UpperBound);
		}

		public virtual T Limit(T value)
		{
			return Limit(value, LowerBound, UpperBound);
		}

		private static T Limit(T value, T lowerBound, T upperBound)
		{
			T result = value;
			if (value.IsMoreThan(upperBound)) result = upperBound;
			if (value.IsLessThan(lowerBound)) result = lowerBound;
			return result;
		}

		#endregion

		#region bound checking

		/// <summary>
		/// Returns true if bounds are suitable for range creation
		/// </summary>
		public static bool CheckBounds(T lowerBound, T upperBound)
		{
			return lowerBound.IsAtMost(upperBound);
		}

		/// <summary>
		/// Returns true if bounds are suitable for range creation
		/// </summary>
		public static bool CheckBounds(IBound<T> lowerBound, IBound<T> upperBound)
		{
			return lowerBound.LessThan(upperBound.Value);
		}

		public static void AssertBounds(T lowerBound, T upperBound)
		{
			if (!CheckBounds(lowerBound, upperBound))
			{
				throw exception(lowerBound, upperBound);
			}
		}

		public static void AssertBounds(IBound<T> lowerBound, IBound<T> upperBound)
		{
			if (!CheckBounds(lowerBound, upperBound))
			{
				throw exception(lowerBound.Value, upperBound.Value);
			}
		}

		private static ArgumentOutOfRangeException exception(T lowerBound, T upperBound)
		{
			string message = string.Format(Exceptions.Range_UnorderedBounds_Template, lowerBound, upperBound);

			return new ArgumentOutOfRangeException("upperBound", upperBound, message);
		}

		public void AssertArgument(string paramName, T value)
		{
			if (!Contains(value))
			{
				string message = string.Format(Exceptions.Range_ArgumentAssertion_Template,
						_lowerBound.ToAssertion(),
						_upperBound.ToAssertion(),
						this);
				throw new ArgumentOutOfRangeException(paramName, value, message);
			}
		}

		public void AssertArgument(string paramName, IEnumerable<T> values)
		{
			if (values == null) throw new ArgumentNullException("values");

			foreach (var value in values)
			{
				AssertArgument(paramName, value);
			}
		}

		#endregion

		public virtual IEnumerable<T> Generate(Func<T, T> nextGenerator)
		{
			T numberInRange = _lowerBound.Generate(nextGenerator);
			while (_upperBound.MoreThan(numberInRange))
			{
				yield return numberInRange;
				T next = nextGenerator(numberInRange);
				if (next.IsAtMost(numberInRange)) throw new ArgumentException(Exceptions.Range_NotIncrementingGenerator, "nextGenerator");
				numberInRange = next;
			}
		}

		private static Func<T, T> _nextGenerator;
		private static Func<T, T> initNextGenerator(T step)
		{
			ParameterExpression current = Expression.Parameter(typeof(T), "current");
			Expression<Func<T, T>> nextExpr = Expression.Lambda<Func<T, T>>(
				Expression.Add(
				 current,
				 Expression.Constant(step)),
				 current);
			return nextExpr.Compile();
		}

		public virtual IEnumerable<T> Generate(T increment)
		{
			_nextGenerator = _nextGenerator ?? initNextGenerator(increment);
			return Generate(_nextGenerator);
		}

		#region Empty Range

		public static Range<T> Empty { get { return EmptyRange<T>.Instance; } }

		private sealed class EmptyRange<U> : Range<U> where U : IComparable<U>
		{
			private EmptyRange() { }
			public override bool Contains(U item) { return false; }
			public override IEnumerable<U> Generate(U increment) { return Enumerable.Empty<U>(); }
			public override IEnumerable<U> Generate(Func<U, U> increment) { return Enumerable.Empty<U>(); }
			public override U Limit(U value) { return value; }
			public override U LimitLower(U value) { return value; }
			public override U LimitUpper(U value) { return value; }
			public override Range<U> Join(Range<U> range) { return range ?? this; }
			public override Range<U> Intersect(Range<U> range) { return this; }
			
			public static Range<U> Instance { get { return Nested.instance; } }
			// ReSharper disable ClassNeverInstantiated.Local
			class Nested
			{
				// Explicit static constructor to tell C# compiler
				// not to mark type as beforefieldinit
				// ReSharper disable EmptyConstructor
				static Nested() { }
				// ReSharper restore EmptyConstructor
				internal static readonly Range<U> instance = new EmptyRange<U>();
				// ReSharper restore ClassNeverInstantiated.Local
			}
		}

		#endregion

		#region Equality Members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Range<T>)) return false;
			return Equals((Range<T>)obj);
		}

		public bool Equals(Range<T> other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other._lowerBound, _lowerBound) && Equals(other._upperBound, _upperBound);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_lowerBound.GetHashCode() * 397) ^ _upperBound.GetHashCode();
			}
		}

		#endregion

		/// <remarks>
		/// <c>..</c> separator used instead of standard <c>,</c> in order to to avoid confusion with rational ranges.
		/// <para><c>[ ]</c> used for closed bounds.</para>
		/// <para><c>( )</c> used for open bounds, as it is more clear than inverted brackets <c>] [</c>.</para>
		/// </remarks>
		public override string ToString()
		{
			return string.Format("{0}..{1}", _lowerBound.Lower(), _upperBound.Upper());
		}

		public virtual Range<T> Join(Range<T> range)
		{
			if (range == null || ReferenceEquals(range, Empty)) return this;

			IBound<T> lower = min(_lowerBound, range._lowerBound, Restrictive.Less),
				upper = max(_upperBound, range._upperBound, Restrictive.Less);

			return new Range<T>(lower, upper);
		}

		internal static class Restrictive
		{
			public static IBound<T> Less(IBound<T> x, IBound<T> y)
			{
				assertArguments(x, y);

				if (x.IsClosed) return x;
				return y.IsClosed ? y : x;
			}

			public static IBound<T> More(IBound<T> x, IBound<T> y)
			{
				assertArguments(x, y);

				if (x.IsClosed) return y;
				return y.IsClosed ? x : y;
			}

			private static void assertArguments(IBound<T> x, IBound<T> y)
			{
				Guard.AgainstArgument("y",
					!x.Value.IsEqualTo(y.Value),
					"Bound values need to be equal to check restrictiveness.");
			}
		}

		private static IBound<T> min(IBound<T> x, IBound<T> y, Func<IBound<T>, IBound<T>, IBound<T>> equalSelection)
		{
			IBound<T> min;
			if (x.Value.IsEqualTo(y.Value))
			{
				min = equalSelection(x, y);
			}
			else
			{
				min = x.Value.IsLessThan(y.Value) ? x : y;
			}
			return min;
		}

		private static IBound<T> max(IBound<T> x, IBound<T> y, Func<IBound<T>, IBound<T>, IBound<T>> equalSelection)
		{
			IBound<T> max;
			if (x.Value.IsEqualTo(y.Value))
			{
				max = equalSelection(x, y);
			}
			else
			{
				max = x.Value.IsMoreThan(y.Value) ? x : y;
			}
			return max;
		}

		public virtual Range<T> Intersect(Range<T> range)
		{
			Range<T> intersection = Empty;

			if (range != null && !ReferenceEquals(range, Empty))
			{
				if (_lowerBound.Touches(range._upperBound))
				{
					intersection = Range.Degenerate(LowerBound);
				}
				else if (_upperBound.Touches(range._lowerBound))
				{
					intersection = Range.Degenerate(UpperBound);
				}
				else if (LowerBound.IsLessThan(range.UpperBound) && UpperBound.IsMoreThan(range.LowerBound))
				{
					IBound<T> lower = max(_lowerBound, range._lowerBound, Restrictive.More),
						upper = min(_upperBound, range._upperBound, Restrictive.More);
					intersection = new Range<T>(lower, upper);
				}
			}
			return intersection;
		}

		public virtual bool Overlaps(Range<T> range)
		{
			if (range == null || ReferenceEquals(range, Empty)) return false;

			bool overlaps = _lowerBound.Touches(range._upperBound) ||
				_upperBound.Touches(range._lowerBound) ||
				(LowerBound.IsLessThan(range.UpperBound) && UpperBound.IsMoreThan(range.LowerBound));
			
			return overlaps;
		}
	}
}
