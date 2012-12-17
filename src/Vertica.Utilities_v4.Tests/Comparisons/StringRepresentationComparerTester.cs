﻿using System;
using System.Globalization;
using NUnit.Framework;
using Vertica.Utilities_v4.Comparisons;
using Vertica.Utilities_v4.Tests.Comparisons.Support;

namespace Vertica.Utilities_v4.Tests.Comparisons
{
	[TestFixture]
	public class StringRepresentationComparerTester
	{
		[Test]
		public void Ctor_DefaultsToAscending()
		{
			var subject = new StringRepresentationComparer<string>(str => str);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Ascending));
		}

		[Test]
		public void Ctor_SetsDirection()
		{
			var subject = new StringRepresentationComparer<string>(str => str, Direction.Descending);
			Assert.That(subject.SortDirection, Is.EqualTo(Direction.Descending));
		}

		[TestCase("1", "2", -1)]
		[TestCase("2", "1", 1)]
		[TestCase("2", "2", 0)]
		[TestCase("11", "2", -1)]
		public void Strings_SameAsDefaultComparison(string left, string right, int expected)
		{
			Func<string, string> converter = str => str;
			var subject = new StringRepresentationComparer<string>(converter);
			Assert.That(subject.Compare(left, right), Is.EqualTo(left.CompareTo(right)));
			Assert.That(subject.Compare(left, right), Is.EqualTo(expected));
		}

		[TestCase("1", "2", -1)]
		[TestCase("2", "1", 1)]
		[TestCase("2", "2", 0)]
		[TestCase("11", "2", 1)]
		public void Integers_ComparesIntNotString(string left, string right, int expected)
		{
			Func<string, int> converter = int.Parse;
			var subject = new StringRepresentationComparer<int>(converter);
			Assert.That(subject.Compare(left, right), Is.EqualTo(expected));
		}

		[TestCase("01/01/2008", "01/02/2008", -1)]
		[TestCase("11/03/2008", "12/02/2008", 1)]
		public void DateTime_ComparesDateTime(string left, string right, int expected)
		{
			Func<string, DateTime> converter = str => DateTime.ParseExact(str, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			var subject = new StringRepresentationComparer<DateTime>(converter);
			Assert.That(subject.Compare(left, right), Is.EqualTo(expected));
		}

		[Test]
		public void Compare_ComparedIntegers_HonoringDirection()
		{
			Func<string, int> converter = int.Parse;

			var subject = new StringRepresentationComparer<int>(converter, Direction.Ascending);
			Assert.That(subject.Compare("1", "2"), Is.LessThan(0));

			subject = new StringRepresentationComparer<int>(converter, Direction.Descending);
			Assert.That(subject.Compare("1", "2"), Is.GreaterThan(0));
		}
	}
}