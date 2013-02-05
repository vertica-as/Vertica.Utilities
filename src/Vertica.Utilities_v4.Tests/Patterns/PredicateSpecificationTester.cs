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
	public class PredicateSpecificationTester
	{
		#region interface operations

		[Test]
		public void ISpecification_LengthBetween5And10()
		{
			ISpecification<string> subject = new PredicateSpecification<string>(s => s.Length >= 5 && s.Length <= 10);

			Assert.That(subject, Must.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Not.Be.SatisfiedBy("1234").Or("1234567890123"));
		}

		[Test]
		public void Not_LengthBetween5And10()
		{
			ISpecification<string> lengthBetween5And10 = new PredicateSpecification<string>(s => s.Length >= 5 && s.Length <= 10);
			ISpecification<string> subject = lengthBetween5And10.Not();

			Assert.That(subject, Must.Not.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Be.SatisfiedBy("1234").And("1234567890123"));
		}

		[Test]
		public void MoreThan5_And_LessThan10()
		{
			ISpecification<int> lessThan10 = PredicateSpecification<int>.CreateFor(i => i < 10);
			ISpecification<int> moreThan5 = new PredicateSpecification<int>(i => i > 5);
			ISpecification<int> subject = lessThan10.And(moreThan5);

			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void MoreThan10_Or_LessThan5()
		{
			ISpecification<int> moreThan10 = PredicateSpecification<int>.CreateFor(i => i > 10);
			ISpecification<int> lessThan5 = new PredicateSpecification<int>(i => i < 5);
			ISpecification<int> subject = lessThan5.Or(moreThan10);

			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}

		#endregion

		#region Operators

		[Test]
		public void NotOp_LengthBetween5And10()
		{
			var lengthBetween5And10 = new PredicateSpecification<string>(s => s.Length >= 5 && s.Length <= 10);
			PredicateSpecification<string> subject = !lengthBetween5And10;

			Assert.That(subject, Must.Not.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Be.SatisfiedBy("1234").And("1234567890123"));
		}

		[Test]
		public void MoreThan5_AndOp_LessThan10()
		{
			PredicateSpecification<int> lessThan10 = PredicateSpecification<int>.CreateFor(i => i < 10);
			var moreThan5 = new PredicateSpecification<int>(i => i > 5);
			PredicateSpecification<int> subject = lessThan10 && moreThan5;

			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void MoreThan10_OrOp_LessThan5()
		{
			PredicateSpecification<int> moreThan10 = PredicateSpecification<int>.CreateFor(i => i > 10);
			var lessThan5 = new PredicateSpecification<int>(i => i < 5);
			PredicateSpecification<int> subject = lessThan5 || moreThan10;

			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}

		[Test]
		public void Implicit_To_Predicate()
		{
			var lessThan5 = new PredicateSpecification<int>(i => i < 5);
			var l = new List<int>(new[] { 2, 4, 6, 8, 10 });
			Predicate<int> p = lessThan5;
			Assert.That(l.FindAll(p), Has.Count.EqualTo(2));
		}

		#endregion

		#region class encapsulation tests

		class LessThan10SpecSubject : PredicateSpecification<int>
		{
			public LessThan10SpecSubject() : base(i => i < 10) { }
		}

		class MoreThan5SpecSubject : PredicateSpecification<int>
		{
			public MoreThan5SpecSubject() : base(i => i > 5) { }
		}

		class MoreThan10SpecSubject : PredicateSpecification<int>
		{
			public MoreThan10SpecSubject() : base(i => i > 10) { }
		}

		class LessThan5SpecSubject : PredicateSpecification<int>
		{
			public LessThan5SpecSubject() : base(i => i < 5) { }
		}

		[Test]
		public void Encapsulated_LessThan10_ExpectedBehavior()
		{
			var subject = new LessThan10SpecSubject();

			Assert.That(subject, Must.Be.SatisfiedBy(5));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(11));
		}

		[Test]
		public void Encapsulated_NegatedMoreThan5_ExpectedBehavior()
		{
			var subject = new MoreThan5SpecSubject().Not();

			Assert.That(subject, Must.Not.Be.SatisfiedBy(6));
			Assert.That(subject, Must.Be.SatisfiedBy(3));
		}

		[Test]
		public void Encapsulated_AndComposition_ExpectedBehavior()
		{
			var lessThan10 = new LessThan10SpecSubject();
			var moreThan5 = new MoreThan5SpecSubject();

			var subject = lessThan10.And(moreThan5);
			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));

			subject = lessThan10.And(moreThan5);
			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void Encapsulated_OrComposition_ExpectedBehavior()
		{
			Specification<int> moreThan10 = new MoreThan10SpecSubject();
			Specification<int> lessThan5 = new LessThan5SpecSubject();

			ISpecification<int> subject = lessThan5.Or(moreThan10);
			Assert.IsFalse(subject.IsSatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));

			subject = lessThan5.Or(moreThan10);
			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}

		#endregion

		#region complex composition

		// ReSharper disable FieldCanBeMadeReadOnly.Local
		private readonly Specification<ComplexType> FooLengthOf2 = new PredicateSpecification<ComplexType>(c => c.Foo.Length == 2);
		// ReSharper restore FieldCanBeMadeReadOnly.Local

		class BarEven : PredicateSpecification<ComplexType>
		{
			public BarEven() : base(item => item.Bar % 2 != 0) { }
		}

		class ComplexTypeEnabled : PredicateSpecification<ComplexType>
		{
			public ComplexTypeEnabled() : base(c => c.Enabled) { }
		}

		[Test]
		public void ComplexComposition_PredicateUsage_FoundMatchingElements()
		{
			var data = new List<ComplexType>(new ComplexContainer());
			Assert.That(data.Find(FooLengthOf2.IsSatisfiedBy).Bar, Is.EqualTo(2));

			Assert.That(data.FindIndex(new BarEven().Not().IsSatisfiedBy), Is.EqualTo(2));

			Specification<ComplexType> enabled = new ComplexTypeEnabled(), barEven = new BarEven();
			Predicate<ComplexType> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
			Assert.That(data.FindAll(enabledOrDisabledAndBarEven), Has.Count.EqualTo(6));
		}

		[Test]
		public void ComplexComposition_LinqUsage_FoundMatchingElements()
		{
			IEnumerable<ComplexType> data = new ComplexContainer();
			var q1 = from c in data where FooLengthOf2.IsSatisfiedBy(c) select c.Bar;
			Assert.That(q1.First(), Is.EqualTo(2));

			Func<ComplexType, bool> notEven = c => new BarEven().Not().IsSatisfiedBy(c);
			var q2 = data.Where(notEven).Select(c => c.Foo);
			Assert.That(q2.First(), Is.EqualTo("12"));

			Specification<ComplexType> enabled = new ComplexTypeEnabled(), barEven = new BarEven();
			Func<ComplexType, bool> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
			var q3 = from c in data where enabledOrDisabledAndBarEven(c) select c;
			Assert.That(q3, Must.Have.Count(Is.EqualTo(6)));
		}

		#endregion
	}
}