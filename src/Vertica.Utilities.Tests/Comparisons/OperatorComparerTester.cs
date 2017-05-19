using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Vertica.Utilities.Tests.Comparisons.Support;
using Vertica.Utilities.Comparisons;

namespace Vertica.Utilities.Tests.Comparisons
{
	[TestFixture]
	public class OperatorComparerTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			IComparer<OperatorsOnly> subject = new OperatorComparer<OperatorsOnly>(Direction.Descending);
			Assert.That(subject.Compare(new OperatorsOnly(1), new OperatorsOnly(2)), Is.GreaterThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(1)), Is.LessThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(2)), Is.EqualTo(0));
		}

		#region subjects

		class OperatorsOnly
		{
			private readonly int _value;

			public OperatorsOnly(int value)
			{
				_value = value;
			}

			public override string ToString()
			{
				return _value.ToString(CultureInfo.InvariantCulture);
			}

			public static bool operator >(OperatorsOnly x, OperatorsOnly y)
			{
				return x._value > y._value;
			}

			public static bool operator <(OperatorsOnly x, OperatorsOnly y)
			{
				return x._value < y._value;
			}
		}

		#endregion

		[Test]
		public void Ctor_DefaultsToAscending()
		{
			var subject = new OperatorComparer<OperatorsOnly>();

			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Ascending));
		}

		[Test]
		public void Ctor_SetsDirection()
		{
			var subject = new OperatorComparer<OperatorsOnly>(Direction.Descending);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Descending));
		}

		[Test]
		public void Compare_HonorsDirection()
		{
			var subject = new OperatorComparer<OperatorsOnly>();

			Assert.That(subject.Compare(new OperatorsOnly(1), new OperatorsOnly(2)), Is.LessThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(1)), Is.GreaterThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(2)), Is.EqualTo(0));

			subject = new OperatorComparer<OperatorsOnly>(Direction.Descending);
			Assert.That(subject.Compare(new OperatorsOnly(1), new OperatorsOnly(2)), Is.GreaterThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(1)), Is.LessThan(0));
			Assert.That(subject.Compare(new OperatorsOnly(2), new OperatorsOnly(2)), Is.EqualTo(0));
		}

		[Test]
		public void TypesWithoutOperators_Throw()
		{
			var subject = new OperatorComparer<ComparisonSubject>();
			Assert.That(() => subject.Compare(ComparisonSubject.One, ComparisonSubject.Two),
				Throws.InstanceOf<InvalidOperationException>());
		}

		[Test]
		public void Clients_DoNotHaveToCareAboutNulls()
		{
			var subject = Cmp<OperatorsOnly>.FromOperators();

			Assert.That(subject.Compare(null, null), Is.EqualTo(0));
			Assert.That(subject.Compare(new OperatorsOnly(1), null), Is.GreaterThan(0));
			Assert.That(subject.Compare(null, new OperatorsOnly(1)), Is.LessThan(0));
		}
	}
}
