using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Extensions.EnumerableExt;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class EnumerableExtensionsTester
	{
		#region nullability

		#region EmptyIfNull

		[Test]
		public void EmptyIfNull_NonEmptySource_ReturnsSource()
		{
			var source = new[] { 1, 2, 3 };
			Assert.That(source.EmptyIfNull(), Is.SameAs(source));
		}

		[Test]
		public void EmptyIfNull_NullSource_ReturnsEmpty()
		{
			Assert.That(Chain.Null<int>().EmptyIfNull(), Is.Empty);
		}

		[Test]
		public void EmptyIfNull_EmptySource_ReturnsEmpty()
		{
			Assert.That(Chain.Empty<int>().EmptyIfNull(), Is.Empty);
		}

		#endregion

		#region NullIfEmpty

		[Test]
		public void NullIfEmpty_NonEmptySource_ReturnsSource()
		{
			var source = new[] { 1, 2, 3 };
			Assert.That(source.NullIfEmpty(), Is.SameAs(source));
		}

		[Test]
		public void NullIfEmpty_NullSource_ReturnsNull()
		{
			Assert.That(Chain.Null<int>().NullIfEmpty(), Is.Null);
		}

		[Test]
		public void NullIfEmpty_EmptySource_ReturnsNull()
		{
			Assert.That(Chain.Empty<int>().NullIfEmpty(), Is.Null);
		}

		#endregion

		#endregion

		#region count constraints

		[Test]
		public void HasOne_OneElement_True()
		{
			Assert.That(new[] { 1 }.HasOne(), Is.True);
		}

		[Test]
		public void HasOne_MopreOrLessThanOneElement_False()
		{
			Assert.That(Chain.Null<int>().HasOne(), Is.False);
			Assert.That(Chain.Empty<int>().HasOne(), Is.False);
			Assert.That(new []{1, 2}.HasOne(), Is.False);
		}

		#region HasAtLeast

		[Test]
		public void HasAtLeast_Null_AlwaysFalse()
		{
			Assert.That(Chain.Null<int>().HasAtLeast(1), Is.False);
			Assert.That(Chain.Null<DateTime>().HasAtLeast(0), Is.False, "A null collection does not have even 0 elements.");
		}

		[Test]
		public void HasAtLeast_Empty_HasAtMost0()
		{
			Assert.That(Chain.Empty<int>().HasAtLeast(0), Is.True, "An empty collection has at least 0 elements");
			Assert.That(Chain.Empty<int>().HasAtLeast(1), Is.False);
			Assert.That(Chain.Empty<DateTime>().HasAtLeast(2), Is.False);
		}


		[TestCase(0u), TestCase(1u), TestCase(2u), TestCase(3u), TestCase(4u)]
		public void HasAtLeast_LessOrEqThanLength_True(uint length)
		{
			Assert.That(Enumerable.Range(1, 4).HasAtLeast(length), Is.True);
		}


		[TestCase(5u), TestCase(6u)]
		public void HasAtLeast_MoreThanLength_False(uint length)
		{
			Assert.That(Enumerable.Range(1, 4).HasAtLeast(length), Is.False);
		}

		#endregion

		#endregion

		#region iteration

		[Test]
		public void Foreach_GoesOverIntCounting_SameLength()
		{
			int count = 0;
			
			Enumerable.Range(1, 4).ForEach(i =>
			{
				count += 1;
				Assert.That(count, Is.EqualTo(i));
			});

			Assert.That(count, Is.EqualTo(4));
		}

		[Test]
		public void For_PerformsActionOnIndexes()
		{
			int acc = 0;
			Enumerable.Range(1, 4).For(new[] { 1, 2 }, (i, item) =>
			{
				acc += item + i;
			});
			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
		}

		[Test]
		public void For_PerformsActionOnIndexesThatCanBeSpecifiedAsOptionalParameters()
		{
			int acc = 0;
			Action<int, int> action = (i, item) => acc += item + i;
			Enumerable.Range(1, 4).For(action, 1, 2);

			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
		}

		#endregion


	}
}