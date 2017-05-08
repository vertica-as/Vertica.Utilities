using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Patterns;
using Vertica.Utilities_v4.Tests.Patterns.Support;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	[TestFixture]
	public class ExpressionSpecificationTeste
	{
		class MoreThan10 : ExpressionSpecification<int>
		{
			public MoreThan10() : base(i => i > 10) { }
		}

		ISpecification<int> lessThanFive = new ExpressionSpecification<int>(i => i < 5);


		#region interface operations

		[Test]
		public void ISpecification_LengthBetween5And10_()
		{
			ISpecification<string> subject = new ExpressionSpecification<string>(s => s.Length >= 5 && s.Length <= 10);

			Assert.That(subject.IsSatisfiedBy("123456"), Is.True);
			Assert.That(subject, Must.Not.Be.SatisfiedBy("1234").Or("1234567890123"));
		}

		[Test]
		public void Not_LengthBetween5And10()
		{
			var lengthBetween5And10 = new ExpressionSpecification<string>(s => s.Length >= 5 && s.Length <= 10);
			ISpecification<string> subject = lengthBetween5And10.Not();

			Assert.That(subject, Must.Not.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Be.SatisfiedBy("1234").And("1234567890123"));
		}

		[Test]
		public void MoreThan5_And_LessThan10()
		{
			var lessThan10 = ExpressionSpecification.For<int>(i => i < 10);
			var moreThan5 = new ExpressionSpecification<int>(i => i > 5);
			ISpecification<int> subject = lessThan10.And(moreThan5);

			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void MoreThan10_Or_LessThan5()
		{
			ISpecification<int> moreThan10 = ExpressionSpecification.For<int>(i => i > 10);
			ISpecification<int> lessThan5 = new ExpressionSpecification<int>(i => i < 5);
			ISpecification<int> subject = lessThan5.Or(moreThan10);

			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}

		#endregion

		#region Operators

		[Test]
		public void NotOp_LengthBetween5And10()
		{
			var lengthBetween5And10 = new ExpressionSpecification<string>(s => s.Length >= 5 && s.Length <= 10);
			ExpressionSpecification<string> subject = !lengthBetween5And10;

			Assert.That(subject, Must.Not.Be.SatisfiedBy("123456"));
			Assert.That(subject, Must.Be.SatisfiedBy("1234").And("1234567890123"));
		}

		[Test]
		public void MoreThan5_AndOp_LessThan10()
		{
			ExpressionSpecification<int> lessThan10 = ExpressionSpecification.For<int>(i => i < 10);
			var moreThan5 = new ExpressionSpecification<int>(i => i > 5);
			ExpressionSpecification<int> subject = lessThan10 && moreThan5;

			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void MoreThan10_OrOp_LessThan5()
		{
			ExpressionSpecification<int> moreThan10 = ExpressionSpecification.For<int>(i => i > 10);
			var lessThan5 = new ExpressionSpecification<int>(i => i < 5);
			ExpressionSpecification<int> subject = lessThan5 || moreThan10;

			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void Implicit_To_Predicate()
		{
			var lessThan5 = new ExpressionSpecification<int>(i => i < 5);
			var l = new List<int>(new[] { 2, 4, 6, 8, 10 });
			Predicate<int> p = lessThan5;
			Assert.That(l.FindAll(p), Has.Count.EqualTo(2));
		}

		[Test]
		public void Implicit_To_Func()
		{
			var lessThan5 = new ExpressionSpecification<int>(i => i < 5);
			Func<int, bool> f = lessThan5.Function;
			f = lessThan5;
			Assert.That(f(1), Is.True);
			Assert.That(f(6), Is.False);
		}

		[Test]
		public void Implicit_To_Expression()
		{
			var lessThan5 = new ExpressionSpecification<int>(i => i < 5);
			Expression<Func<int, bool>> e = lessThan5.Expression;
			e = lessThan5;
			Assert.That(e.Compile().Invoke(1), Is.True);
			Assert.That(e.Compile().Invoke(6), Is.False);
		}

		#endregion

		#region class encapsulation tests

		class LessThan10ExpSpecSubject : ExpressionSpecification<int>
		{
			public LessThan10ExpSpecSubject() : base(i => i < 10) { }
		}

		class MoreThan5ExpSpecSubject : ExpressionSpecification<int>
		{
			public MoreThan5ExpSpecSubject() : base(i => i > 5) { }
		}

		class MoreThan10SpecExprSubject : ExpressionSpecification<int>
		{
			public MoreThan10SpecExprSubject() : base(s => s > 10) { }
		}

		class LessThan5SpecExprSubject : ExpressionSpecification<int>
		{
			public LessThan5SpecExprSubject() : base(s => s < 5) { }
		}

		[Test]
		public void EncapsulatedExpr_LessThan10_ExpectedBehavior()
		{
			var subject = new LessThan10ExpSpecSubject();

			Assert.That(subject, Must.Be.SatisfiedBy(5));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(11));
		}

		[Test]
		public void EncapsulatedExpr_NegatedMoreThan5_ExpectedBehavior()
		{
			var subject = new MoreThan5ExpSpecSubject().Not();

			Assert.That(subject, Must.Not.Be.SatisfiedBy(6));
			Assert.That(subject, Must.Be.SatisfiedBy(3));
		}

		[Test]
		public void EncapsulatedExpr_AndComposition_ExpectedBehavior()
		{
			var lessThan10 = new LessThan10ExpSpecSubject();
			var moreThan5 = new MoreThan5ExpSpecSubject();

			var subject = lessThan10.And(moreThan5);
			Assert.That(subject, Must.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Not.Be.SatisfiedBy(3).Or(13));
		}

		[Test]
		public void EncapsulatedExpr_OrComposition_ExpectedBehavior()
		{
			var moreThan10 = new MoreThan10SpecExprSubject();
			var lessThan5 = new LessThan5SpecExprSubject();

			ISpecification<int> subject = lessThan5.Or(moreThan10);
			Assert.That(subject, Must.Not.Be.SatisfiedBy(7));
			Assert.That(subject, Must.Be.SatisfiedBy(3).And(13));
		}


		#endregion

		#region complex composition

		private readonly Specification<ComplexType> FooLengthOf2 = new ExpressionSpecification<ComplexType>(c => c.Foo.Length == 2);

		class BarEven : ExpressionSpecification<ComplexType>
		{
			public BarEven() : base(item => item.Bar % 2 != 0) { }
		}

		class ComplexTypeEnabled : ExpressionSpecification<ComplexType>
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