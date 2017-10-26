using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vertica.Utilities
{
	public class DomainOfValues<T> : IEnumerable<T>
	{
		private readonly T[] _expectedValues;
		public DomainOfValues(params T[] expectedValues)
		{ 
			_expectedValues = expectedValues ?? new T[0];
		}

		public DomainOfValues(IEnumerable<T> expectedValues) : this(expectedValues?.ToArray()) { }

		#region Check

		public bool CheckContains(T actualValue)
		{
			return CheckContains(actualValue, EqualityComparer<T>.Default);
		}

		public bool CheckContains(T actualValue, IEqualityComparer<T> comparer)
		{
			bool contains = false;
			if (_expectedValues != null)
			{
				for (int i = 0; i < _expectedValues.Length; i++)
				{
					contains = comparer.Equals(actualValue, _expectedValues[i]);
					if (contains) break;
				}
			}
			return contains;
		}

		#endregion

		#region Assert

		public void AssertContains(T actualValue)
		{
			if (!CheckContains(actualValue))
			{
				throw new InvalidDomainException<T>(actualValue, _expectedValues);
			}
		}

		public void AssertContains(T actualValue, IEqualityComparer<T> comparer)
		{
			if (!CheckContains(actualValue, comparer))
			{
				throw new InvalidDomainException<T>(actualValue, _expectedValues);
			}
		}

		#endregion

		public IEnumerator<T> GetEnumerator()
		{
			return _expectedValues.Cast<T>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class InvalidDomainException<T> : InvalidOperationException
	{
		public InvalidDomainException() {}
		public InvalidDomainException(string message) : base(message) {}
		public InvalidDomainException(string message, Exception inner) : base(message, inner) {}

		public InvalidDomainException(T actualValue, IEnumerable<T> expectedDomainValues)
			: this(buildMessage(actualValue, expectedDomainValues)) { }

		private static string buildMessage(T actualValue, IEnumerable<T> expectedDomainValues)
		{
			string csValues = string.Join(", ", expectedDomainValues ?? Enumerable.Empty<T>());
			return string.Format(Resources.Exceptions.InvalidDomainException_MessageTemplate, actualValue, csValues);
		}
	}
}
