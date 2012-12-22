using System;
using System.Collections.Generic;
using Vertica.Utilities_v4.Comparisons;

namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class EqualitySpy : IEquatable<EqualitySpy>
	{
		public bool GetHashCodeCalled { get; private set; }
		public bool EqualsCalled { get; private set; }

		public Func<T, T, bool> GetEquals<T>(bool result)
		{
			return (x, y) =>
			{
				EqualsCalled = true;
				return result;
			};
		}

		public Comparison<T> GetComparison<T>(int result)
		{
			return (x, y) =>
			{
				EqualsCalled = true;
				return result;
			};
		}

		public Func<T, int> GetHashCode<T>(int result)
		{
			return x =>
			{
				GetHashCodeCalled = true;
				return result;
			};
		}

		public IComparer<T> GetComparer<T>(int result)
		{
			return new ComparisonComparer<T>(GetComparison<T>(result));
		}

		public int SelectorCallCount { get; private set; }
		public Func<T, int> GetSelector<T>()
		{
			return x =>
			{
				SelectorCallCount++;
				return 42;
			};
		}
		public override int GetHashCode()
		{
			GetHashCodeCalled = true;
			return base.GetHashCode();
		}

		public bool Equals(EqualitySpy other)
		{
			EqualsCalled = true;
			other.EqualsCalled = true;
			return base.Equals(other);
		}

		public override bool Equals(object obj)
		{
			EqualsCalled = true;
			return base.Equals(obj);
		}

		public T FakeASelector<T>(T selectorValue) where T : class
		{
			SelectorCallCount++;
			return selectorValue;
		}
	}
}