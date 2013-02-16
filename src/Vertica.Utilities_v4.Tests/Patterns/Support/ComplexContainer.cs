using System.Collections;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Tests.Patterns.Support
{
	internal class ComplexContainer : IEnumerable<ComplexType>
	{
		private readonly List<ComplexType> _inner;
		public ComplexContainer()
		{
			_inner = new List<ComplexType>
			{
				new ComplexType(true, "12345", 5), new ComplexType(false, "12345", 5),
				new ComplexType(true, "12", 2), new ComplexType(false, "12", 2),
				new ComplexType(true, "12345", 2), new ComplexType(false, "12345", 2),
				new ComplexType(true, "12", 5), new ComplexType(false, "12", 5)
			};
		}

		public IEnumerator<ComplexType> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}