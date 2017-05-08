using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Patterns;
using Vertica.Utilities_v4.Tests.Patterns.Support;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	[TestFixture]
	public class SpecificationTester
	{
		class MoreThan10 : Specification<int>
		{
			public override bool IsSatisfiedBy(int item)
			{
				return item > 10;
			}
		}

		class LessThan5 : Specification<int>
		{
			public override bool IsSatisfiedBy(int item)
			{
				return item < 5;
			}
		}

		class LessThan10 : Specification<int>
		{
			public override bool IsSatisfiedBy(int item)
			{
				return item < 10;
			}
		}

		class MoreThan5 : Specification<int>
		{
			public override bool IsSatisfiedBy(int item)
			{
				return item > 5;
			}
		}

		class LengthBetween5And10 : Specification<string>
		{
			public override bool IsSatisfiedBy(string item)
			{
				return item.Length >= 5 && item.Length <= 10;
			}
		}

		#region documentation

		[Test, Category("Exploratory")]
		public void Satisfaction()
		{
			var spec = new LessThan10();
			Assert.That(spec.IsSatisfiedBy(5), Is.True);
			Assert.That(spec.IsSatisfiedBy(11), Is.False);
		}

		[Test, Category("Exploratory")]
		public void Negation()
		{
			ISpecification<int> spec = new LessThan10().Not();
			Assert.That(spec.IsSatisfiedBy(5), Is.False);
			Assert.That(spec.IsSatisfiedBy(11), Is.True);
		}

		#endregion

		#region interface operations

		[Test]
		public void ISpecification_LengthBetween5And10()
		{
			ISpecification<string> subject = new LengthBetween5And10();

			Assert.That(subject, Must.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Not.Be.SatisfiedBy("1234").Or("1234567890123"));
		}

		[Test]
		public void Not_LengthBetween5And10()
		{
			ISpecification<string> lengthBetween5And10 = new LengthBetween5And10();
			ISpecification<string> subject = lengthBetween5And10.Not();

			Assert.That(subject, Must.Not.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Be.SatisfiedBy("1234").And("1234567890123"));
		}

		[Test]
		public void MoreThan5_And_LessThan10()
		{
			ISpecification<int> lessThan10 = new LessThan10();
			ISpecification<int> moreThan5 = new MoreThan5();
			ISpecification<int> subject = lessThan10.And(moreThan5);

			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void MoreThan10_Or_LessThan5()
		{
			ISpecification<int> moreThan10 = new MoreThan10();
			ISpecification<int> lessThan5 = new LessThan5();
			ISpecification<int> subject = lessThan5.Or(moreThan10);

			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}

		#endregion

		#region complex composition

		class Foo_LengthOf2 : Specification<ComplexType>
		{
			public override bool IsSatisfiedBy(ComplexType item)
			{
				return item.Foo.Length == 2;
			}
		}

		class Bar_Even : Specification<ComplexType>
		{
			public override bool IsSatisfiedBy(ComplexType item)
			{
				return item.Bar % 2 != 0;
			}
		}

		class ComplexType_Enabled : Specification<ComplexType>
		{
			public override bool IsSatisfiedBy(ComplexType item)
			{
				return item.Enabled;
			}
		}

		[Test]
		public void ComplexComposition_PredicateUsage_FoundMatchingElements()
		{
			var data = new List<ComplexType>(new ComplexContainer());
			Assert.That(data.Find(new Foo_LengthOf2().IsSatisfiedBy).Bar, Is.EqualTo(2));

			Assert.That(data.FindIndex(new Bar_Even().Not().IsSatisfiedBy), Is.EqualTo(2));

			Specification<ComplexType> enabled = new ComplexType_Enabled(), barEven = new Bar_Even();
			Predicate<ComplexType> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
			Assert.That(data.FindAll(enabledOrDisabledAndBarEven), Has.Count.EqualTo(6));
		}

		[Test]
		public void ComplexComposition_LinqUsage_FoundMatchingElements()
		{
			IEnumerable<ComplexType> data = new ComplexContainer();
			var fooLengthOf2 = new Foo_LengthOf2();
			var q1 = from c in data where fooLengthOf2.IsSatisfiedBy(c) select c.Bar;
			Assert.That(q1.First(), Is.EqualTo(2));

			Func<ComplexType, bool> notEven = c => new Bar_Even().Not().IsSatisfiedBy(c);
			var q2 = data.Where(notEven).Select(c => c.Foo);
			Assert.That(q2.First(), Is.EqualTo("12"));

			Specification<ComplexType> enabled = new ComplexType_Enabled(), barEven = new Bar_Even();
			Func<ComplexType, bool> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
			var q3 = from c in data where enabledOrDisabledAndBarEven(c) select c;
			Assert.That(q3, Must.Have.Count(Is.EqualTo(6)));
		}

		#endregion
	}
}