using System;
using NUnit.Framework;
using Testing.Commons.Globalization;
using Vertica.Utilities.Web;

namespace Vertica.Utilities.Tests.Web
{
	[TestFixture]
	public class QualityTester
	{
		[Test]
		public void Ctor_ConstrainedFactor_FactorSet()
		{
			float qualityFactor = .33f;
			var subject = new Quality(qualityFactor);

			Assert.That(subject.Factor, Is.EqualTo(qualityFactor));
		}

		[Test]
		public void ToString_FactorRepresentation()
		{
			float qualityFactor = .33f;
			var subject = new Quality(qualityFactor);

			Assert.That(subject.ToString(), Is.EqualTo("q=0.33"));
		}

		[Test]
		public void ToString_FactorRepresentation_CultureIndependent()
		{
			float qualityFactor = .33f;
			var subject = new Quality(qualityFactor);

			string dotCultureRepresentation, commaCultureRepresentation;
			using (CultureReseter.Set("en-US"))
			{
				dotCultureRepresentation = subject.ToString();
			}

			using (CultureReseter.Set("da-DK"))
			{
				commaCultureRepresentation = subject.ToString();
			}
			Assert.That(dotCultureRepresentation, Is.EqualTo(commaCultureRepresentation));
		}

		[Test, TestCase(1.0001f), TestCase(-.99999f)]
		public void Ctor_UnboundedFactor_ExceptionWithCorrectRange(
			float unboundedFactor)
		{
			Assert.That(() => new Quality(unboundedFactor), Throws.InstanceOf<ArgumentOutOfRangeException>().With
				.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(unboundedFactor)
				.And.Message.Contains("within [0..1]."));
		}

		[Test]
		public void NonCtorCreation_Spec()
		{
			Assert.That(Quality.Default.Factor, Is.EqualTo(1f), "default is 1");
			Assert.That(Quality.Min.Factor, Is.EqualTo(0f), "min is 0");
			Assert.That(new Quality().Factor, Is.EqualTo(0f), "default ctor is 0");
		}

		[Test]
		public void TryParse_QuantifiedValue_FactorSet()
		{
			Quality? subject = Quality.TryParse("q=0.2");

			Assert.That(subject, Is.Not.Null.And
				.Property(nameof(Quality.Factor)).EqualTo(.2f));
		}

		[Test]
		public void TryParse_Spaces_DontMatter()
		{
			Quality? subject = Quality.TryParse(" q = 0.2 ");

			Assert.That(subject, Is.Not.Null.And
				.Property(nameof(Quality.Factor)).EqualTo(.2f));
		}

		[Test, TestCase(""), TestCase(null)]
		public void TryParse_Nothing_Null(string empty)
		{
			Quality? subject = Quality.TryParse(empty);

			Assert.That(subject, Is.Null);
		}

		[Test]
		public void TryParse_NotANumber_Null()
		{
			Quality? subject = Quality.TryParse("q=abc");

			Assert.That(subject, Is.Null);
		}

		[Test]
		public void TryParse_UnboundedFactor_Null()
		{
			Quality? subject = Quality.TryParse("q=2");

			Assert.That(subject, Is.Null);
		}

		[Test]
		public void CompareTo_AccordingToSpecification()
		{
			Quality subject= new Quality(.5f),
				striclySmaller = new Quality(.3f),
				striclyLarger = new Quality(.7f), equal = new Quality(.5f);

			Assert.That(subject, Is.GreaterThan(striclySmaller));
			Assert.That(subject, Is.LessThan(striclyLarger));
			Assert.That(subject, Is.LessThanOrEqualTo(equal));
			Assert.That(subject, Is.GreaterThanOrEqualTo(equal));
		}
	}
}