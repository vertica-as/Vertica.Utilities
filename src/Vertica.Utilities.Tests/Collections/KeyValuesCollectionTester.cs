using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class KeyValuesCollectionTester
	{
		[Test]
		public void Ctor_NoItems_Empty()
		{
			var subject = new KeyValuesCollection();
			Assert.That(subject, Has.Count.EqualTo(0));
		}

		[Test]
		public void Add_NonExistingKeys_AddsTopLevelElements()
		{
			var subject = new KeyValuesCollection();
			subject.Add("k", "v");

			Assert.That(subject, Has.Count.EqualTo(1));
			Assert.That(subject["k"], Is.EqualTo(new[] { "v" }));
		}

		[Test]
		public void Add_ExistingKeys_AddsValues()
		{
			var subject = new KeyValuesCollection { { "k", "v1" } };
			subject.Add("k", "v2");

			Assert.That(subject, Has.Count.EqualTo(1));
			Assert.That(subject["k"], Is.EqualTo(new[] { "v1", "v2" }));
		}

		[Test]
		public void Add_DifferentComparer_ChangeHowKeysAreCompared()
		{
			var subject = new KeyValuesCollection(StringComparer.Ordinal) { { "y", "v1" } };
			subject.Add("Y", "v2");

			Assert.That(subject, Has.Count.EqualTo(2));

			subject = new KeyValuesCollection(StringComparer.OrdinalIgnoreCase) { { "y", "v1" } };
			subject.Add("Y", "v2");

			Assert.That(subject, Has.Count.EqualTo(1));
		}

		[Test]
		public void Add_NullKey_Exception()
		{
			var subject = new KeyValuesCollection();
			Assert.That(() => subject.Add(null, "v"), Throws.ArgumentNullException);
		}

		[Test]
		public void Add_EmptyKey_NoProblem()
		{
			var subject = new KeyValuesCollection();
			Assert.That(() => subject.Add(string.Empty, "v"), Throws.Nothing);
			Assert.That(subject, Has.Count.EqualTo(1));
		}

		[Test]
		public void Add_NullValue_NoProblem()
		{
			var subject = new KeyValuesCollection();
			Assert.That(() => subject.Add("k", null), Throws.Nothing);
			Assert.That(subject.Count, Is.EqualTo(1));
			Assert.That(subject["k"], Must.Have.Count(Is.EqualTo(1)));
		}

		[Test]
		public void AddRange_AddMultipleValues_ToSameKey()
		{
			var subject = new KeyValuesCollection();
			subject.AddRange("k", "a", "b", "c"); // using params
			Assert.That(subject["k"], Must.Have.Count(Is.EqualTo(3)));

			subject.AddRange("k", Enumerable.Repeat("x", 2)); // using collection
			Assert.That(subject["k"], Must.Have.Count(Is.EqualTo(5)));
		}

		[Test]
		public void AddRange_NullValues_Exception()
		{
			var subject = new KeyValuesCollection();
			IEnumerable<string> @null = null;
			Assert.That(() => subject.AddRange("k", @null), Throws.ArgumentNullException);
		}

		[Test]
		public void AddRange_EmptyValues_KeyNotAdded()
		{
			var subject = new KeyValuesCollection();
			subject.AddRange("k", Enumerable.Empty<string>());

			Assert.That(subject, Is.Empty);
		}

		[Test]
		public void AddRange_AnotherLookup_NewKeysAddedAndExistingMerged()
		{
			var one = new KeyValuesCollection { { "1", "one" } };
			var two = new KeyValuesCollection { { "2", "two" }, { "1", "uno" } };
			one.AddRange(two);

			Assert.That(one["1"], Is.EqualTo(new[] { "one", "uno" }));
			Assert.That(one["2"], Is.EqualTo(new[] { "two" }));
		}

		[Test]
		public void AddRange_NullLookup_Exception()
		{
			var subject = new KeyValuesCollection<int, int>();
			ILookup<int, int> @null = null;
			Assert.That(()=>subject.AddRange(@null), Throws.ArgumentNullException);
		}

		[Test]
		public void Indexer_ExistingKey_CollectionOfValues()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };

			Assert.That(subject["k"], Is.EqualTo(new[] { "v" }));
		}

		[Test]
		public void Indexer_NonExistingKey_Empty()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };

			Assert.That(subject["missing"], Is.Not.Null.And.Empty);
		}

		[Test]
		public void Indexer_DifferentComparer_ChangeHowValuesAreAccessed()
		{
			var subject = new KeyValuesCollection(StringComparer.Ordinal) { { "y", "v1" }, { "Y", "v2" } };
			Assert.That(subject["y"], Is.EqualTo(new[] { "v1" }));
			Assert.That(subject["Y"], Is.EqualTo(new[] { "v2" }));

			subject = new KeyValuesCollection(StringComparer.OrdinalIgnoreCase) { { "y", "v1" }, { "Y", "v2" } };
			Assert.That(subject["y"], Is.EqualTo(new[] { "v1", "v2" }));
			Assert.That(subject["Y"], Is.EqualTo(new[] { "v1", "v2" }));
		}

		[Test]
		public void Contains_ElementsInCollection_True()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };

			Assert.That(subject.Contains("k"), Is.True);
		}

		[Test]
		public void Contains_ElementsNotInCollection_False()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };

			Assert.That(subject.Contains("v"), Is.False, "element with key v is not there");
		}

		[Test]
		public void Contains_DifferentComparer_ChangeHowKeysAreChecked()
		{
			var subject = new KeyValuesCollection(StringComparer.Ordinal) { { "y", "v" } };
			Assert.That(subject.Contains("y"), Is.True);
			Assert.That(subject.Contains("Y"), Is.False);

			subject = new KeyValuesCollection(StringComparer.OrdinalIgnoreCase) { { "y", "v" } };
			Assert.That(subject.Contains("y"), Is.True);
			Assert.That(subject.Contains("Y"), Is.True);
		}

		[Test]
		public void Contains_Null_Exception()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };

			Assert.That(() => subject.Contains(null), Throws.ArgumentNullException);
		}

		[Test]
		public void Contains_Empty_TrueIfThere()
		{
			var subject = new KeyValuesCollection { { "k", "v" } };
			Assert.That(subject.Contains(string.Empty), Is.False);

			subject.Add(string.Empty, "v");
			Assert.That(subject.Contains(string.Empty), Is.True);
		}

		[Test]
		public void Remove_ExistingKey_TrueAndOutOfCollection()
		{
			var subject = new KeyValuesCollection{{"a", "b"}};

			Assert.That(subject.Remove("a"), Is.True);
			Assert.That(subject.Contains("a"), Is.False);
		}

		[Test]
		public void Remove_MissingKey_TrueAndOutOfCollection()
		{
			var subject = new KeyValuesCollection { { "a", "b" } };

			Assert.That(subject.Remove("missing"), Is.False);
			Assert.That(subject, Has.Count.EqualTo(1));
		}

		[Test]
		public void GenericEnumerator_YieldsIGroupings()
		{
			var subject = new KeyValuesCollection<int, string> { { 1, "one" }, { 2, "two" } };

			Assert.That(subject, Has.All.InstanceOf<IGrouping<int, string>>());

			IEnumerable<int> keys = subject.Select(g => g.Key);
			Assert.That(keys, Is.EqualTo(new[] { 1, 2 }));

			IEnumerable<string> values = subject.SelectMany(g => g);
			Assert.That(values, Is.EqualTo(new[] { "one", "two" }));
		}

		[Test]
		public void NonGenericEnumerator_YieldsIGroupings()
		{
			var subject = new KeyValuesCollection<int, string> { { 1, "one" }, { 2, "two" } };

			System.Collections.IEnumerable nonGeneric = subject;

			Assert.That(nonGeneric, Has.All.InstanceOf<IGrouping<int, string>>());
		}
	}
}