using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Tests.Collections.Support;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class SmartEnumerableTester
	{
		[Test]
		public void Ctor_NullEnumerable_Exception()
		{
			Assert.That(() => new SmartEnumerable<string>(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Enumerate_EmptyEnumerable_Empty()
		{
			var subject = new SmartEnumerable<string>(Enumerable.Empty<string>());

			Assert.That(subject, Is.Empty);
		}

		[Test]
		public void Enumerate_SingleEntry_JustOneEntry()
		{
			var subject = new SmartEnumerable<string>(new[] { "x" });

			Assert.That(subject, Must.Be.Constrained(
				Must.Be.Entry(0, "x", isFirst: true, isLast: true)));
		}

		[Test]
		public void Enumerate_DoubleEntry_TwoEntries()
		{
			var subject = new SmartEnumerable<string>(new[] { "x", "y" });

			Assert.That(subject, Must.Be.Constrained(
				Must.Be.Entry(0, "x", isFirst: true, isLast: false),
				Must.Be.Entry(1, "y", isFirst: false, isLast: true)));
		}

		[Test]
		public void Enumerate_TripleEntry_ThreeEntries()
		{
			IEnumerable<SmartEntry<string>> subject = new SmartEnumerable<string>(new[] { "x", "y", "z" });

			Assert.That(subject, Must.Be.Constrained(
				Must.Be.Entry(0, "x", isFirst: true, isLast: false),
				Must.Be.Entry(1, "y", isFirst: false, isLast: false),
				Must.Be.Entry(2, "z", isFirst: false, isLast: true)));
		}
	}
}