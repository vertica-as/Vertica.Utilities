using System;

namespace Vertica.Utilities.Tests.Comparisons.Support
{
	internal class ComparisonSubject : IComparable<ComparisonSubject>
	{
		public ComparisonSubject(string property1, int property2, decimal property3)
		{
			Property1 = property1;
			Property2 = property2;
			Property3 = property3;
		}

		public string Property1 { get; set; }
		public int Property2 { get; set; }
		public decimal Property3 { get; set; }

		public override string ToString()
		{
			return Property1;
		}

		public int CompareTo(ComparisonSubject other)
		{
			return Property1.CompareTo(other.Property1);
		}

		public static readonly ComparisonSubject One = new ComparisonSubject("one", 1, 1m);
		public static readonly ComparisonSubject Two = new ComparisonSubject("two", 2, 2m);
	}
}
