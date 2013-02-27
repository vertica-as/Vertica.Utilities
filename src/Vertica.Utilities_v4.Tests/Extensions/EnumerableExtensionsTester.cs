﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Extensions.EnumerableExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

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

		#region SkipNulls

		[Test]
		public void SkipNulls_NullItems_NotEnumerated()
		{
			var source = new[] { "1", null, "2", "3" };
			Assert.That(source.SkipNulls(), Is.EqualTo(new[] { "1", "2", "3" }));
		}

		[Test]
		public void SkipNulls_NullOrEmpty_Empty()
		{
			Assert.That(Chain.Null<string>().SkipNulls(), Is.Empty);
			Assert.That(Chain.Empty<string>().SkipNulls(), Is.Empty);
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
			Assert.That(new[] { 1, 2 }.HasOne(), Is.False);
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
			int times = 0;

			Enumerable.Range(1, 4).ForEach(i =>
			{
				times += 1;
			});

			Assert.That(times, Is.EqualTo(4));
		}

		[Test]
		public void For_InvokesActionWithItemaAndIndex()
		{
			int count = 0;

			Enumerable.Range(2, 4).For((i, idx) =>
			{
				count += 1;
				Assert.That(i, Is.EqualTo(count + 1));
			});

			Assert.That(count, Is.EqualTo(4));
		}

		[Test]
		public void For_PerformsActionOnIndexes()
		{
			int acc = 0;
			Enumerable.Range(1, 4).For((i, item) =>
			{
				acc += item + i;
			}, new[] { 1, 2 });

			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
		}

		[Test]
		public void For_PerformsActionOnIndexesThatCanBeSpecifiedAsOptionalParameters()
		{
			int acc = 0;
			Action<int, int> action = (i, item) => acc += item + i;
			Enumerable.Range(1, 4).For(action, new[] { 1, 2 });

			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
		}

		#endregion

		[Test]
		public void Convert_AllElementsConvertible_AllElementsConverted()
		{
			IEnumerable<DerivedType> derived = new[] { new DerivedType("Jim", 2) };
			Assert.That(derived.Convert<DerivedType, BaseType>(), Is.All.InstanceOf<BaseType>());
		}

		[Test]
		public void Convert_NullOrEmpty_Empty()
		{
			Assert.That(Chain.Null<DerivedType>().Convert<DerivedType, BaseType>(), Is.Empty);
			Assert.That(Chain.Empty<DerivedType>().Convert<DerivedType, BaseType>(), Is.Empty);
		}

		#region ToDelimitedString

		[Test]
		public void ToDelimitedString_PopulatedCollections_DelimitedString()
		{
			var enumerable = new[] { 1, 2, 3, 4 };
			Func<int, string> doubleToStringFunction = i => (i * 2).ToString(CultureInfo.InvariantCulture);

			Assert.That(enumerable.ToDelimitedString(), Is.EqualTo("1, 2, 3, 4"));
			Assert.That(enumerable.ToDelimitedString(doubleToStringFunction), Is.EqualTo("2, 4, 6, 8"));
			Assert.That(enumerable.ToDelimitedString("-"), Is.EqualTo("1-2-3-4"));
			Assert.That(enumerable.ToDelimitedString("*", doubleToStringFunction), Is.EqualTo("2*4*6*8"));
		}

		[Test]
		public void ToDelimitedString_NullEnumerable_Empty()
		{
			Assert.That(Chain.Null<string>().ToDelimitedString(), Is.Empty);
			Assert.That(Chain.Null<string>().ToDelimitedString(s => string.Empty), Is.Empty);
			Assert.That(Chain.Null<string>().ToDelimitedString("*"), Is.Empty);
			Assert.That(Chain.Null<string>().ToDelimitedString("_", s => string.Empty), Is.Empty);
		}

		[Test]
		public void ToDelimitedString_EmptyEnumerable_Empty()
		{
			Assert.That(Chain.Empty<string>().ToDelimitedString(), Is.Empty);
			Assert.That(Chain.Empty<string>().ToDelimitedString(s => string.Empty), Is.Empty);
			Assert.That(Chain.Empty<string>().ToDelimitedString("*"), Is.Empty);
			Assert.That(Chain.Empty<string>().ToDelimitedString("_", s => string.Empty), Is.Empty);
		}

		[Test]
		public void ToCsv_SameBehaviorAsToDelimitedString()
		{
			var enumerable = new[] { 1, 2, 3, 4 };
			Func<int, string> doubleToStringFunction = i => (i * 2).ToString(CultureInfo.InvariantCulture);

			Assert.That(enumerable.ToCsv(), Is.EqualTo("1,2,3,4"));
			Assert.That(enumerable.ToCsv(doubleToStringFunction), Is.EqualTo("2,4,6,8"));
			Assert.That(Chain.Null<int>().ToCsv(), Is.Empty);
			Assert.That(Chain.Null<int>().ToCsv(doubleToStringFunction), Is.Empty);
			Assert.That(Chain.Empty<int>().ToCsv(), Is.Empty);
			Assert.That(Chain.Empty<int>().ToCsv(doubleToStringFunction), Is.Empty);
		}

		#endregion

		#region enumerable generation

		#region ToCircular

		[Test]
		public void ToCircular_NonEmpty_CouldRepeatForever()
		{
			IEnumerable<int> circular = new[] { 1, 2, 3 }.ToCircular();

			Assert.That(circular.Take(7), Is.EqualTo(new[] { 1, 2, 3, 1, 2, 3, 1 }));
		}

		[Test]
		public void ToCircular_NullOrEmpty_Empty()
		{
			Assert.That(Chain.Null<string>().ToCircular(), Is.Empty);
			Assert.That(Chain.Empty<string>().ToCircular(), Is.Empty);
		}

		#endregion

		#region ToStepped

		[Test]
		public void ToStepped_One_RegularIterator()
		{
			Assert.That(new[] { 1, 2, 3 }.ToStepped(1), Is.EqualTo(new[] { 1, 2, 3 }));
		}

		[Test]
		public void ToStepped_NotOne_SkipsStepElements()
		{
			Assert.That(Enumerable.Range(1, 7).ToStepped(2u), Is.EqualTo(new[] { 1, 3, 5, 7 }));
			Assert.That(Enumerable.Range(1, 7).ToStepped(3u), Is.EqualTo(new[] { 1, 4, 7 }));
			Assert.That(Enumerable.Range(1, 7).ToStepped(4u), Is.EqualTo(new[] { 1, 5 }));
		}

		[Test]
		public void ToStepped_ZeroOrNegative_ExceptionWhenEnumerating()
		{
			IEnumerable<int> stepped = null;
			Assert.That(() => stepped = new[] { 1 }.ToStepped(0), Throws.Nothing);
			Assert.That(() => stepped.Iterate(), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[TestCase(3u)]
		[TestCase(4u)]
		[TestCase(10u)]
		public void ToStepped_StepMoreThanLength_AlwaysEnumeratesFirst(uint step)
		{
			IEnumerable<int> stepped = new[] { 1, 2 }.ToStepped(step);

			Assert.That(stepped.Count(), Is.EqualTo(1));
		}

		[Test]
		public void ToStepped_Empty_FirstIsNotEnumerated()
		{
			Assert.That(Chain.Null<int>().ToStepped(1), Is.Empty);
			Assert.That(Chain.Empty<int>().ToStepped(1), Is.Empty);
		}

		#endregion

		#region Merge

		#region Merge -- Pair<T>

		[Test]
		public void MergePair_SameLength_MergedEnumeration()
		{
			int[] firsts = new[] { 1, 2, 3 }, seconds = new[] { 2, 4, 6 };

			IEnumerable<Pair<int>> merged = firsts.Merge<int>(seconds);
			Assert.That(merged, Is.EqualTo(new[]
			{
				new Pair<int>(1, 2),
				new Pair<int>(2, 4),
				new Pair<int>(3, 6)
			}));
		}

		[Test]
		public void MergePair_DifferentLength_ExceptionWhenEnumerating()
		{
			int[] firsts = new[] { 1, 2, 3 }, seconds = new[] { 2, 4 };

			IEnumerable<Pair<int>> merged = firsts.Merge<int>(seconds);
			Assert.That(() => merged.Iterate(), Throws.ArgumentException);
		}

		[Test]
		public void MergePair_EmptyEnumerables_Empty()
		{
			int[] firsts = new int[] { }, seconds = new int[] { };

			IEnumerable<Pair<int>> merged = firsts.Merge<int>(seconds);
			Assert.That(merged, Is.Empty);
		}

		[Test]
		public void MergePair_NullEnumerables_Empty()
		{
			IEnumerable<Pair<int>> merged = Chain.Null<int>().Merge<int>(Chain.Null<int>());
			Assert.That(merged, Is.Empty);
		}

		#endregion

		#region Merge -- Tuple<t, U>

		[Test]
		public void MergeTuple_SameLength_MergedEnumeration()
		{
			var firsts = new[] { 1, 2, 3 };
			var seconds = new[] { "1", "2", "3" };

			IEnumerable<Tuple<int, string>> merged = firsts.Merge(seconds);
			Assert.That(merged, Is.EqualTo(new[]
			{
				Tuple.Create(1, "1"),
				Tuple.Create(2, "2"),
				Tuple.Create(3, "3")
			}));
		}

		[Test]
		public void MergeTuple_DifferentLength_ExceptionWhenEnumerating()
		{
			var firsts = new[] { 1, 2, 3 };
			var seconds = new[] { "2", "4" };

			IEnumerable<Tuple<int, string>> merged = firsts.Merge(seconds);
			Assert.That(() => merged.Iterate(), Throws.ArgumentException);
		}

		[Test]
		public void MergeTuple_EmptyEnumerables_Empty()
		{
			var firsts = new int[] { };
			var seconds = new string[] { };

			IEnumerable<Tuple<int, string>> merged = firsts.Merge(seconds);
			Assert.That(merged, Is.Empty);
		}

		[Test]
		public void MergeTuple_NullEnumerables_Empty()
		{
			IEnumerable<Tuple<int, string>> merged = Chain.Null<int>().Merge(Chain.Null<string>());
			Assert.That(merged, Is.Empty);
		}

		#endregion

		#endregion

		#region Interlace

		[Test]
		public void Interlace_SameSizeEnumerations_AlternatesOneElementFromEach()
		{
			IEnumerable<int> odds = new[] { 1, 3, 5 }, evens = new[] { 2, 4, 6 };

			IEnumerable<int> oneToSix = odds.Interlace(evens);
			Assert.That(oneToSix, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
		}

		[Test]
		public void Interlace_SomeIsNull_Empty()
		{
			Assert.That(Chain.Null<int>().Interlace(new[] { 1 }), Is.Empty);
			Assert.That(new[] { 1 }.Interlace(Chain.Null<int>()), Is.Empty);
		}

		[Test]
		public void Interlace_SomeIsEmpty_Empty()
		{
			Assert.That(Chain.Empty<int>().Interlace(new[] { 1 }), Is.Empty);
			Assert.That(new[] { 1 }.Interlace(Chain.Empty<int>()), Is.Empty);
		}

		[Test]
		public void Interlace_DifferentSizes_ShortesRules()
		{
			IEnumerable<int> odds = new[] { 1, 3, 5 }, evens = new[] { 2, 4 };

			Assert.That(odds.Interlace(evens), Is.EqualTo(new[] { 1, 2, 3, 4 }));
			Assert.That(evens.Interlace(odds), Is.EqualTo(new[] { 2, 1, 4, 3 }));
		}

		[Test]
		public void Interlace_SampleWithWords()
		{
			var source = new[] { "The", "quick", "brown", "fox" };

			string result = source.Interlace(streamOfSpaces())
				.Aggregate(string.Empty, (a, b) => a + b)
				.TrimEnd();

			Assert.That(result, Is.EqualTo("The quick brown fox"));
		}

		// ReSharper disable FunctionNeverReturns
		private static IEnumerable<string> streamOfSpaces()
		{
			while (true) yield return " ";
		}
		// ReSharper restore FunctionNeverReturns

		#endregion

		#endregion

		#region concantenation helpers

		[Test]
		public void Append_AddToTheEnd()
		{
			Assert.That(Enumerable.Range(1, 4).Append(5, 6), Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
		}

		[Test]
		public void Append_Nothing_Same()
		{
			Assert.That(Enumerable.Range(1, 4).Append(), Is.EqualTo(new[] { 1, 2, 3, 4 }));

			Assert.That(Enumerable.Range(1, 4).Append((int[])null), Is.EqualTo(new[] { 1, 2, 3, 4 }));
		}

		[Test]
		public void Prepend_AddToTheBeginning()
		{
			Assert.That(Enumerable.Range(1, 4).Prepend(5, 6), Is.EqualTo(new[] { 5, 6, 1, 2, 3, 4 }));
		}

		[Test]
		public void Prepend_Nothing_Same()
		{
			Assert.That(Enumerable.Range(1, 4).Prepend(), Is.EquivalentTo(new[] { 1, 2, 3, 4 }));

			Assert.That(Enumerable.Range(1, 4).Prepend((int[])null), Is.EquivalentTo(new[] { 1, 2, 3, 4 }));
		}

		#endregion
	}
}