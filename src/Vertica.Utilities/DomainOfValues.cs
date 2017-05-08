using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Vertica.Utilities_v4
{
	public class DomainOfValues<T> : IEnumerable<T>
	{
		private readonly IEnumerable<T> _expectedValues;
		public DomainOfValues(params T[] expectedValues) : this(expectedValues.AsEnumerable()) { }

		public DomainOfValues(IEnumerable<T> expectedValues)
		{
			_expectedValues = expectedValues ?? Enumerable.Empty<T>();
		}

		#region Check

		public bool CheckContains(T actualValue)
		{
			return _expectedValues != null && _expectedValues.Contains(actualValue);
		}

		public bool CheckContains(T actualValue, IEqualityComparer<T> comparer)
		{
			return _expectedValues != null && _expectedValues.Contains(actualValue, comparer);
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
			return _expectedValues.GetEnumerator();
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
		protected InvalidDomainException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public InvalidDomainException(T actualValue, IEnumerable<T> expectedDomainValues)
			: this(buildMessage(actualValue, expectedDomainValues)) { }

		private static string buildMessage(T actualValue, IEnumerable<T> expectedDomainValues)
		{
			string csValues = string.Join(", ", expectedDomainValues ?? Enumerable.Empty<T>());
			return string.Format(Resources.Exceptions.InvalidDomainException_MessageTemplate, actualValue, csValues);
		}
	}
}
