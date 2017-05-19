using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities.Comparisons;
using Vertica.Utilities.Web;

namespace Vertica.Utilities.Tests.Web
{
	[TestFixture]
	public class QualifiedCollectionTester
	{
		[Test]
		public void Ctor_SomeNulls_Skipped()
		{
			Qualified one = new Qualified("one", Quality.Default),
				two = new Qualified("two", Quality.Default);

			var subject = new QualifiedCollection(new[] { one, null, two });

			Assert.That(subject, Is.EquivalentTo(new[] { one, two }));
		}

		[Test]
		public void Ctor_OrderByQualityDescending()
		{
			Qualified pointThree = new Qualified("pointThree", new Quality(.3f)),
				pointSeven = new Qualified("pointSeven", new Quality(.7f));

			var subject = new QualifiedCollection(new[] { pointThree, pointSeven });

			Assert.That(subject, Is.EqualTo(new[] { pointSeven, pointThree }));
		}

		[Test, TestCase(null), TestCase("")]
		public void TryParse_Empty_EmptyCollection(string empty)
		{
			var subject = QualifiedCollection.TryParse(empty);

			Assert.That(subject, Is.Empty);
		}


		[Test]
		public void TryParse_Languages_QualityOrderedCollection()
		{
			string languages = "da, en-gb;q=0.8, en;q=0.7";

			var subject = QualifiedCollection.TryParse(languages);

			Assert.That(subject, Is.EqualTo(new[]
			{
				new Qualified("da", Quality.Default),
				new Qualified("en-gb", new Quality(.8f)),
				new Qualified("en", new Quality(.7f))
			}).Using(_equalizer));
		}

		[Test]
		public void TryParse_Encodings_QualityOrderedCollection()
		{
			string languages = "gzip;q=1.0, identity; q=0.5, *;q=0";

			var subject = QualifiedCollection.TryParse(languages);

			Assert.That(subject, Is.EqualTo(new[]
			{
				new Qualified("gzip", Quality.Default),
				new Qualified("identity", new Quality(.5f)),
				new Qualified("*", Quality.Min)
			}).Using(_equalizer));
		}

		[Test]
		public void TryParse_ContentTypes_QualityOrderedCollection()
		{
			string languages = "text/plain; q=0.5, text/html,text/x-dvi; q=0.8, text/x-c";

			var subject = QualifiedCollection.TryParse(languages);

			Assert.That(subject, Is.EqualTo(new[]
			{
				new Qualified("text/html", Quality.Default),
				new Qualified("text/x-c", Quality.Default),
				new Qualified("text/x-dvi", new Quality(.8f)),
				new Qualified("text/plain", new Quality(.5f))
			}).Using(_equalizer));
		}

		[Test]
		public void TryParse_AllowCustomQualifiedComparison_ForDomainSpecificSorting()
		{
			string languages = "text/*, text/plain";

			Predicate<Qualified> isSubtype = q => q.Value.EndsWith("*");

			Comparison<Qualified> subtypesComeLater = (q1, q2) =>
			{
				var quality = q1.Quality.CompareTo(q2.Quality);
				if (quality == 0)
				{
					if (isSubtype(q1) && !isSubtype(q2)) return -1;
					if (!isSubtype(q1) && isSubtype(q2)) return 1;
					return StringComparer.Ordinal.Compare(q1.Value, q2.Value);
				}
				return quality;
			};

			var subject = QualifiedCollection.TryParse(languages, Cmp<Qualified>.By(subtypesComeLater));

			Assert.That(subject, Is.EqualTo(new[]
			{
				new Qualified("text/plain", Quality.Default),
				new Qualified("text/*", Quality.Default)
			}).Using(_equalizer));
		}

		private static readonly IEqualityComparer<Qualified> _equalizer = Eq<Qualified>.By((x, y) =>
			StringComparer.Ordinal.Equals(x.Value, y.Value) &&
			x.Quality.Factor.Equals(y.Quality.Factor),
			Hasher.Zero);
	}
}
