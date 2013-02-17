﻿using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;

namespace Vertica.Utilities_v4.Tests.Collections
{
	[TestFixture]
	public class ChainTester
	{
		[Test]
		public void From_NoArguments_Empty()
		{
			IEnumerable<int> none = Chain.From<int>();

			Assert.That(none, Is.Empty);
		}

		[Test]
		public void From_SomeArguments_Enumerable()
		{
			IEnumerable<int> some = Chain.From(1, 2, 3, 4);

			Assert.That(some, Is.EqualTo(new[] { 1, 2, 3, 4 }));
		}

		[Test]
		public void Empty_EmptyEnumerable()
		{
			Assert.That(Chain.Empty<int>(), Is.Empty);
		}

		[Test]
		public void Null_NullEnumerable()
		{
			Assert.That(Chain.Null<int>(), Is.Null);
		}
	}
}