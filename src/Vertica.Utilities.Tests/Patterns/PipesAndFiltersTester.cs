using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns
{
	[TestFixture]
	public class PipesAndFiltersTester
	{
		 [Test]
		public void ctor_NullActions_NoFail()
		{
			var subject = new Pipeline<int>(null);
			subject.Execute();
		}

		[Test]
		public void ctor_EmptyActions_NoFail()
		{
			var subject = new Pipeline<int>(new IOperation<int>[0]);
			subject.Execute();
		}

		[Test]
		public void Execute_SingleFromArgsCtor_ExecutesOperation()
		{
			execute_SingleOperation_ExecutesOperation<int>(operation => new Pipeline<int>(operation));
		}

		[Test]
		public void Execute_SingleFromEnumerableCtor_ExecutesOperation()
		{
			execute_SingleOperation_ExecutesOperation<int>(operation => new Pipeline<int>(new[] { operation }));
		}

		[Test]
		public void Execute_SingleFromRegistration_ExecutesOperation()
		{
			execute_SingleOperation_ExecutesOperation<int>(operation => new Pipeline<int>().Register(operation));
		}

		private static void execute_SingleOperation_ExecutesOperation<T>(Func<IOperation<T>, Pipeline<T>> arrange)
		{
			var operation = Substitute.For<IOperation<T>>();
			
			var subject = arrange(operation);

			subject.Execute();

			var emptyList = Arg.Is<IEnumerable<T>>(l => !l.Any());
			operation.Received().Execute(emptyList);
		}

		[Test]
		public void Execute_TwoOperationsFromArgsCtor_ExecutesOperation()
		{
			execute_TwoOperations_ExecuteBothUsingOutputOfPrevious<int>((first, second) => new Pipeline<int>(first, second));
		}

		[Test]
		public void Execute_TwoOperationsFromEnumerableCtor_ExecutesOperation()
		{
			execute_TwoOperations_ExecuteBothUsingOutputOfPrevious<int>((first, second) => new Pipeline<int>(new[] { first, second }));
		}

		[Test]
		public void Execute_TwoOperationsFromRegistration_ExecutesOperation()
		{
			execute_TwoOperations_ExecuteBothUsingOutputOfPrevious<int>((first, second) => new Pipeline<int>().Register(first).Register(second));
		}

		private static void execute_TwoOperations_ExecuteBothUsingOutputOfPrevious<T>(Func<IOperation<T>, IOperation<T>, Pipeline<T>> arrange)
		{
			IEnumerable<T> firstOutput = new[] { default(T) };

			IOperation<T> first = Substitute.For<IOperation<T>>(),
				second = Substitute.For<IOperation<T>>();

			var subject = arrange(first, second);
			var emptyList = Arg.Is<IEnumerable<T>>(l => !l.Any());
			first.Execute(emptyList).Returns(firstOutput);
			
			subject.Execute();
			
			second.Received().Execute(firstOutput);
		}

		[Test]
		public void Sample_AppendNegativeSquareForTenFirstIntegers()
		{
			IList<int> context = new List<int>(10);
			new Pipeline<int>()
				.Register(new TenFirstIntegers())
				.Register(new Square())
				.Register(new Negate())
				.Register(new Append(context))
				.Execute();

			Assert.That(context, Is.EqualTo(new[]{-1, -4, -9, -16, -25, -36, -49, -64, -81, -100}));
		}

		[Test]
		public void Sample_AppendNegativeSquareForOddsInTenFirstIntegers()
		{
			IList<int> context = new List<int>(10);
			new Pipeline<int>()
				.Register(new TenFirstIntegers())
				.Register(new OnlyOdds())
				.Register(new Square())
				.Register(new Negate())
				.Register(new Append(context))
				.Execute();

			Assert.That(context, Is.EqualTo(new[] { -1, -9, -25, -49, -81 }));
		}
	}

	internal class TenFirstIntegers : IOperation<int>
	{
		public IEnumerable<int> Execute(IEnumerable<int> input)
		{
			return Enumerable.Range(1, 10);
		}
	}

	internal class Square : IOperation<int>
	{
		public IEnumerable<int> Execute(IEnumerable<int> input)
		{
			return input.Select(i => i * i);
		}
	}

	internal class Negate : IOperation<int>
	{
		public IEnumerable<int> Execute(IEnumerable<int> input)
		{
			return input.Select(i => -i);
		}
	}

	internal class Append : IOperation<int>
	{
		private readonly ICollection<int> _context;

		public Append(ICollection<int> context)
		{
			_context = context;
		}

		public IEnumerable<int> Execute(IEnumerable<int> input)
		{
			foreach (var i in input)
			{
				_context.Add(i);
				yield return i;
			}
		}
	}

	internal class OnlyOdds : IOperation<int>
	{
		public IEnumerable<int> Execute(IEnumerable<int> input)
		{
			return input.Where(i => i % 2 != 0);
		}
	}
}