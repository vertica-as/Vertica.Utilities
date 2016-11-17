using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web
{
	[TestFixture]
	public class QualifiedTester
	{
		[Test]
		public void ToString_TokenizedConcatenation()
		{
			var subject = new Qualified("da", new Quality(.2f));

			Assert.That(subject.ToString(), Is.EqualTo("da;q=0.2"));
		}

		[Test, TestCase(null), TestCase("")]
		public void TryParse_Empty_Null(string empty)
		{
			Assert.That(Qualified.TryParse(empty), Is.Null);
		}

		[Test]
		public void TryParse_SingleValue_SingleValueSet()
		{
			var subject = Qualified.TryParse("gzip;q=0.5");

			Assert.That(subject.Value, Is.EqualTo("gzip"));
			Assert.That(subject.Quality, Is.EqualTo(new Quality(.5f)));
		}

		[Test]
		public void TryParse_MultiValue_MultiValueSet()
		{
			var subject = Qualified.TryParse("text/html;level=1;q=0.5");

			Assert.That(subject.Value, Is.EqualTo("text/html;level=1"));
			Assert.That(subject.Quality, Is.EqualTo(new Quality(.5f)));
		}

		[Test]
		public void TryParse_MultiValueDefaultQuality_MultiValueSet()
		{
			var subject = Qualified.TryParse("text/html;level=1");

			Assert.That(subject.Value, Is.EqualTo("text/html;level=1"));
			Assert.That(subject.Quality, Is.EqualTo(Quality.Default));
		}

		[Test]
		public void TryParse_NotQualifiedString_SingleValueSet()
		{
			var subject = Qualified.TryParse("not qualified");

			Assert.That(subject.Value, Is.EqualTo("not qualified"));
			Assert.That(subject.Quality, Is.EqualTo(Quality.Default));
		}

		[Test]
		public void CompareTo_OnlyQualityTakenIntoAccount()
		{
			Qualified subject = new Qualified("anything", new Quality(.5f)),
				striclySmaller = new Qualified("smaller", new Quality(.3f)),
				striclyLarger = new Qualified("larger", new Quality(.7f)),
				equal = new Qualified("same", new Quality(.5f));

			Assert.That(subject, Is.GreaterThan(striclySmaller));
			Assert.That(subject, Is.LessThan(striclyLarger));
			Assert.That(subject, Is.LessThanOrEqualTo(equal));
			Assert.That(subject, Is.GreaterThanOrEqualTo(equal));
			Assert.That(subject.CompareTo(null), Is.GreaterThan(0));
		}

		[Test]
		public void CompareTo_DoesNotTakeIntoAccountValueSemantics()
		{
			Qualified html = new Qualified("text/html", new Quality(.5f)),
				text = new Qualified("text/*", new Quality(.5f));

			Assert.That(text.CompareTo(html), Is.EqualTo(0),
				"if value semantics were used, html should be more important than any text");
		}
	}
}