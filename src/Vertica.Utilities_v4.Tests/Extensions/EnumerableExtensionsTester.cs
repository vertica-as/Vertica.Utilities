using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Comparisons;
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
		public void Selecting_GoesOverIntCounting_SameLengthAndComposable()
		{
			int times = 0;

			var oneToFour = Enumerable.Range(1, 4).Selecting(i =>
		   {
			   times += 1;
		   });

			// this enumeration is important, otherwise, lazyness will prevent enuemration
			Assert.That(oneToFour, Is.EqualTo(new[] { 1, 2, 3, 4 }));
			Assert.That(times, Is.EqualTo(4));
		}

		[Test]
		public void Selecting_OnlyAction_IsLazy()
		{
			int times = 0;

			var oneToFour = Enumerable.Range(1, 4).Selecting(i =>
		   {
			   times += 1;
		   });

			Assert.That(times, Is.EqualTo(0), "Because we have not enumerated the collection");
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			oneToFour.ToArray();
			Assert.That(times, Is.EqualTo(4), "Because the collection has been enumerated");
		}

		[Test]
		public void For_InvokesActionWithItemAndIndex()
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
		public void Selecting_InvokesActionWithItemAndIndex()
		{
			int count = 0;

			var twoToFour = Enumerable.Range(2, 4).Selecting((i, idx) =>
		   {
			   count += 1;
			   Assert.That(i, Is.EqualTo(count + 1));
		   });

			// this enumeration is important, otherwise, lazyness will prevent enuemration
			Assert.That(twoToFour, Is.EqualTo(new[] { 2, 3, 4, 5 }));
			Assert.That(count, Is.EqualTo(4));
		}

		[Test]
		public void Selecting_WithIndex_IsLazy()
		{
			int count = 0;

			var twoToFour = Enumerable.Range(2, 4).Selecting((i, idx) =>
		   {
			   count += 1;
			   Assert.That(i, Is.EqualTo(count + 1));
		   });

			Assert.That(count, Is.EqualTo(0), "Because we have not enumerated the collection");
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			twoToFour.ToArray();
			Assert.That(count, Is.EqualTo(4), "Because the collection has been enumerated");
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

		[Test]
		public void Selecting_PerformsActionOnIndexesThatCanBeSpecifiedAsOptionalParameters()
		{
			int acc = 0;
			Action<int, int> action = (i, item) => acc += item + i;
			var oneToFour = Enumerable.Range(1, 4).Selecting(action, new[] { 1, 2 });

			// this enumeration is important, otherwise, lazyness will prevent enuemration
			Assert.That(oneToFour, Is.EqualTo(new[] { 1, 2, 3, 4 }));
			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
		}

		[Test]
		public void Selecting_OptionalIndexes_IsLazy()
		{
			int acc = 0;
			Action<int, int> action = (i, item) => acc += item + i;
			var oneToFour = Enumerable.Range(1, 4).Selecting(action, new[] { 1, 2 });

			Assert.That(acc, Is.EqualTo(0), "Because we have not enumerated the collection");
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			oneToFour.ToArray();
			Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)), "Because the collection has been enumerated");
		}

		#endregion

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

		[Test]
		public void Merge_SameLength_MergedEnumeration()
		{
			int[] firsts = { 1, 2, 3 }, seconds = { 2, 4, 6 };

			IEnumerable<Pair<int>> merged = firsts.Merge(seconds);
			Assert.That(merged, Is.EqualTo(new[]
			{
				new Pair<int>(1, 2),
				new Pair<int>(2, 4),
				new Pair<int>(3, 6)
			}));
		}

		[Test]
		public void Merge_DifferentLength_ExceptionWhenEnumerating()
		{
			int[] firsts = { 1, 2, 3 }, seconds = { 2, 4 };

			IEnumerable<Pair<int>> merged = firsts.Merge(seconds);
			Assert.That(() => merged.Iterate(), Throws.ArgumentException);
		}

		[Test]
		public void Merge_EmptyEnumerables_Empty()
		{
			int[] firsts = { }, seconds = { };

			IEnumerable<Pair<int>> merged = firsts.Merge(seconds);
			Assert.That(merged, Is.Empty);
		}

		[Test]
		public void Merge_NullEnumerables_Empty()
		{
			IEnumerable<Pair<int>> merged = Chain.Null<int>().Merge(Chain.Null<int>());
			Assert.That(merged, Is.Empty);
		}

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

		#region zipping

		[Test]
		public void Zip_Indexed_SelectorIncludesIndex()
		{
			var numbers = new[] { 5, 6, 7, 8 };
			var words = new[] { "cinq", "six", "sept", "huit" };

			var zipped = numbers.Zip(words, (number, word, index) =>
				new { Number = number, Word = word, Index = index });

			Assert.That(zipped, Is.EqualTo(new[]
			{
				new { Number = 5, Word = "cinq", Index = 0 },
				new { Number = 6, Word = "six", Index = 1 },
				new { Number = 7, Word = "sept", Index = 2 },
				new { Number = 8, Word = "huit", Index = 3 }
			}));
		}

		[Test]
		public void Zip_Strict_SameLength_TupleOfElements()
		{
			var numbers = new[] { 5, 6, 7, 8 };
			var words = new[] { "cinq", "six", "sept", "huit" };

			var zipped = numbers.Zip(words);

			Assert.That(zipped, Is.EqualTo(new[]
			{
				Tuple.Create(5, "cinq"),
				Tuple.Create(6, "six"),
				Tuple.Create(7, "sept"),
				Tuple.Create(8, "huit")
			}));
		}


		[Test]
		public void Zip_Strict_DifferentLength_Exception()
		{
			var numbers = new[] { 5, 6, 7, 8 };
			var words = new[] { "cinq", "six", "sept", "huit", "neuf" };

			Assert.That(() => numbers.Zip(words).Iterate(), Throws.InvalidOperationException);
		}

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

		#region batching

		[Test]
		public void InBatchesOf_BatchSizeIsFactorOfInputLength_NumberOfBacthes()
		{
			var result = new[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0
			}.InBatchesOf(10);

			Assert.That(result, Is.EqualTo(new[]
			{
				new[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
				new[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 0}
			}));
		}

		[Test]
		public void InBatchesOf_BatchSizeIsNotFactorOfInputLength_NumberOfBacthesPlusOne()
		{
			var result = new[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0
			}.InBatchesOf(9);

			Assert.That(result, Is.EqualTo(new[]
			{
				new[] {1, 2, 3, 4, 5, 6, 7, 8, 9 },
				new[] {0, 1, 2, 3, 4, 5, 6, 7, 8},
				new[] {9, 0}
			}));
		}

		[Test]
		public void InBatchesOf_BatchSizeIsIsGreaterThanInputLength_OneBatchWithAllElements()
		{
			IEnumerable<int> input = new[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
				1, 2, 3, 4, 5, 6, 7, 8, 9, 0
			};

			Assert.That(input.InBatchesOf(1000), Is.EqualTo(new[]
			{
				input
			}));
		}

		[Test]
		public void InBatchesOf_CallingToArray_ShouldBeTheSameAsNotCallingItAndIterating()
		{
			string[] input =
			{
				"140720",
				"358998",
				"519897",
				"882101",
				"1197596"
			};

			var enumerable = input.InBatchesOf(2);
			var array = input.InBatchesOf(2).ToArray();

			Assert.That(enumerable, Is.EqualTo(array));
			Assert.That(enumerable, Is.EqualTo(new[]
			{
				new [] {"140720" , "358998"},
				new [] {"519897", "882101"},
				new [] {"1197596"},
			}));
		}

		#endregion

		#region shuffle

		[Test]
		public void Shuffle_Default_SameItemsMostLikelyInDifferentOrder()
		{
			var oneToTwenty = Enumerable.Range(1, 20).ToList();

			Assert.That(oneToTwenty.Shuffle(), Is.EquivalentTo(oneToTwenty).And
				.Not.EqualTo(oneToTwenty), "most likely not equal");
		}

		[Test]
		public void Shuffle_Limited_SomeItemsMostLikelyInDifferentOrder()
		{
			var oneToTwenty = Enumerable.Range(1, 20).ToList();

			Assert.That(oneToTwenty.Shuffle(4).ToArray(), Is.SubsetOf(oneToTwenty)
				.With.Length.EqualTo(4));
		}

		[Test]
		public void Shuffle_WithCustomRandomizer_SameItemsAsRandomizerSays()
		{
			var oneToFour = Enumerable.Range(1, 4).ToList();

			var inverter = Substitute.For<IRandomizer>();
			inverter.Next(4).Returns(3);
			inverter.Next(3).Returns(2);
			inverter.Next(2).Returns(1);
			inverter.Next(1).Returns(0);

			Assert.That(oneToFour.Shuffle(inverter), Is.EqualTo(new[] { 4, 3, 2, 1 }));
		}

		[Test]
		public void Shuffle_WithCustomRandomizer_RandomizerAskedAsManyTimesAsItemsInList()
		{
			var oneToFour = Enumerable.Range(1, 4).ToList();

			var randomizer = Substitute.For<IRandomizer>();

			oneToFour.Shuffle(randomizer).Iterate();

			randomizer.Received(4).Next(Arg.Any<int>());
		}

		[Test]
		public void Shuffle_WithLimitedCustomRandomizer_RandomizerAskedAsManyTimesAsCount()
		{
			var oneToFour = Enumerable.Range(1, 4).ToList();

			var randomizer = Substitute.For<IRandomizer>();

			oneToFour.Shuffle(randomizer, 2).Iterate();

			randomizer.Received(2).Next(Arg.Any<int>());
		}

		#endregion

		#region MinBy

		[Test]
		public void MinBy_NullCollection_Exception()
		{
			IEnumerable<OrderSubject> @null = Chain.Null<OrderSubject>();
			Assert.That(() => @null.MinBy(s => s.I1), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void MinBy_EmptyCollection_Exception()
		{
			IEnumerable<OrderSubject> empty = Chain.Empty<OrderSubject>();
			Assert.That(() => empty.MinBy(s => s.I1), Throws.InvalidOperationException);
		}

		[Test]
		public void MinBy_DefaultComparer_MinimumValueAccordingToSelector()
		{
			var twoOne = new OrderSubject(2, 1);
			var collection = new[] { new OrderSubject(1, 2), twoOne };

			Assert.That(collection.MinBy(s => s.I2), Is.SameAs(twoOne));
		}

		[Test]
		public void MinBy_AlternativeComparer_MinimumValueAccordingToSelectorAndComparer()
		{
			var twoOne = new OrderSubject(2, 1);
			var collection = new[] { new OrderSubject(2, -2), twoOne };

			IComparer<int> absComparer = Cmp<int>
				.By((one, other) => Math.Abs(one).CompareTo(Math.Abs(other)));

			Assert.That(collection.MinBy(s => s.I2, absComparer), Is.SameAs(twoOne));
		}

		[Test]
		public void MinBy_SeveralEqualElements_FirstReturned()
		{
			OrderSubject twoOne = new OrderSubject(2, 1), anotherTwoOne = new OrderSubject(2, 1);
			var collection = new[] { new OrderSubject(2, 3), twoOne, anotherTwoOne };

			Assert.That(collection.MinBy(s => s.I2), Is.SameAs(twoOne));
		}

		#endregion

		#region MaxBy

		[Test]
		public void MaxBy_NullCollection_Exception()
		{
			IEnumerable<OrderSubject> @null = Chain.Null<OrderSubject>();
			Assert.That(() => @null.MaxBy(s => s.I1), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void MaxBy_EmptyCollection_Exception()
		{
			IEnumerable<OrderSubject> empty = Chain.Empty<OrderSubject>();
			Assert.That(() => empty.MaxBy(s => s.I1), Throws.InvalidOperationException);
		}

		[Test]
		public void MaxBy_DefaultComparer_MinimumValueAccordingToSelector()
		{
			var twoOne = new OrderSubject(2, 1);
			var collection = new[] { new OrderSubject(1, -1), twoOne };

			Assert.That(collection.MaxBy(s => s.I2), Is.EqualTo(twoOne));
		}

		[Test]
		public void MaxBy_AlternativeComparer_MinimumValueAccordingToSelectorAndComparer()
		{
			var twoMinusTwo = new OrderSubject(2, -2);
			var collection = new[] { new OrderSubject(2, -1), twoMinusTwo };

			IComparer<int> absComparer = Cmp<int>
				.By((one, other) => Math.Abs(one).CompareTo(Math.Abs(other)));

			Assert.That(collection.MaxBy(s => s.I2, absComparer), Is.EqualTo(twoMinusTwo));
		}

		[Test]
		public void MaxBy_SeveralEqualElements_FirstReturned()
		{
			OrderSubject twoOne = new OrderSubject(2, 1), anotherTwoOne = new OrderSubject(2, 1);
			var collection = new[] { new OrderSubject(2, 0), twoOne, anotherTwoOne };

			Assert.That(collection.MaxBy(s => s.I2), Is.SameAs(twoOne));
		}

		#endregion

		#region ToHashSet

		[Test]
		public void ToHashSet_Null_Exception()
		{
			IEnumerable<int> a = null;

			Assert.That(() => a.ToHashSet(), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void ToHashSet_Empty_EmptyHashSet()
		{
			IEnumerable<int> a = new int[0];

			Assert.That(a.ToHashSet(), Is.Empty);
		}

		[Test]
		public void ToHashSet_NoMoreArguments_HashSetWithAllUniqueMembers()
		{
			var subject = new[] { 1, 2, 3, 4, 3 };
			HashSet<int> set = subject.ToHashSet();
			Assert.That(set, Is.EquivalentTo(new[] { 1, 2, 3, 4 }));
		}

		[Test]
		public void ToHashSet_NullSelector_Exception()
		{
			IEnumerable<DerivedType> a = new DerivedType[0];
			Func<DerivedType, int> selector = null;

			Assert.That(() => a.ToHashSet(selector), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void ToHashSet_Selector_HashSetWithUniqueMembers()
		{
			IEnumerable<DerivedType> subject = new[]
			{
				new DerivedType("one", 1),
				new DerivedType("two", 2),
				new DerivedType("ONE", 1)
			};
			HashSet<int> set = subject.ToHashSet(d => d.ID);
			Assert.That(set, Is.EquivalentTo(new[] { 1, 2 }));
		}

		[Test]
		public void ToHashSet_SelectorAndComparer_HashSetWithUniqueMembersAccordingToComparer()
		{
			IEnumerable<DerivedType> subject = new[]
			{
				new DerivedType("one", 1),
				new DerivedType("two", 2),
				new DerivedType("ONE", 1)
			};
			HashSet<string> set = subject.ToHashSet(d => d.Name, StringComparer.OrdinalIgnoreCase);
			Assert.That(set, Is.EquivalentTo(new[] { "one", "two" }));
		}

		[Test]
		public void ToHashSet_NullComparer_DefaultComparer()
		{
			IEnumerable<DerivedType> subject = new[]
			{
				new DerivedType("one", 1),
				new DerivedType("two", 2),
				new DerivedType("ONE", 1)
			};
			HashSet<string> set = subject.ToHashSet(d => d.Name, null);
			Assert.That(set, Is.EquivalentTo(new[] { "one", "two", "ONE" }));
		}

		[Test]
		public void ToHashSet_OnlyComparer_HashSetWithUniqueMembersAsComparer()
		{
			IEnumerable<DerivedType> subject = new[]
			{
				new DerivedType("one", 1),
				new DerivedType("two", 2),
				new DerivedType("ONE", 1)
			};
			IEqualityComparer<DerivedType> comparer = Eq<DerivedType>.By(
				(x, y) => x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase),
				Hasher.Zero);

			HashSet<DerivedType> set = subject.ToHashSet(comparer);
			Assert.That(set, Has.Count.EqualTo(2));
		}

		#endregion

		#region SortBy
		[Test]
		public void SortBy_ListOfExistingKeys_ObjectsInSameOrder()
		{
			OrderSubject one = new OrderSubject(1, 1), two = new OrderSubject(2, 2);
			var subject = new[] { one, two };
			var actual = subject.SortBy(s => s.I1, new[] { 2, 1 });
			Assert.That(actual, Is.EqualTo(new[] { two, one }));
		}
		[Test]
		public void SortBy_SomeMissingKeys_ExistingObjectsInSpecifiedOrder()
		{
			OrderSubject one = new OrderSubject(1, 1), two = new OrderSubject(2, 2);
			var subject = new[] { one, two };
			var actual = subject.SortBy(s => s.I1, new[] { 2, 3, 1 });
			Assert.That(actual, Is.EqualTo(new[] { two, one }));
		}
		[Test]
		public void SortBy_SomeUnexpectedObject_ObjectFilteredOut()
		{
			OrderSubject one = new OrderSubject(1, 1), two = new OrderSubject(2, 2);
			var subject = new[] { one, two };
			var actual = subject.SortBy(s => s.I1, new[] { 3, 2 });
			Assert.That(actual, Is.EqualTo(new[] { two }));
		}
		[Test]
		public void SortBy_CustomEqualizer_EqualizerHonored()
		{
			OrderSubject one = new OrderSubject(1, 1), two = new OrderSubject(2, 2);
			var subject = new[] { one, two };
			IEqualityComparer<int> signDoesNotMatter = Eq<int>.By(
			(x, y) => Math.Abs(x).Equals(Math.Abs(y)),
			i => Math.Abs(i).GetHashCode());
			var actual = subject.SortBy(s => s.I1, new[] { -2, 1 }, signDoesNotMatter);
			Assert.That(actual, Is.EqualTo(new[] { two, one }));
		}
		#endregion
	}
}