using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class UniqueIdentifierTester
	{
		[Test]
		public void Guessable_IdsThatGrow()
		{
			IComparer<string> comparer = StringComparer.OrdinalIgnoreCase;
			var combRepresentations = new string[10];
			for (int i = 0; i < 10; i++)
			{
				combRepresentations[i] = UniqueIdentifier.Guessable().ToString("N");
				if (i > 0)
				{
					// compare with previous
					Assert.That(combRepresentations[i], Is.GreaterThan(combRepresentations[i -1]).Using(comparer));
				}
			}
		}
	}
}
