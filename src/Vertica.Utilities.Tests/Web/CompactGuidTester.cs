using System;
using NUnit.Framework;
using Vertica.Utilities.Web;
using Testing.Commons;

namespace Vertica.Utilities.Tests.Web
{
	[TestFixture]
	public class CompactGuidTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			var subject = new CompactGuid(Guid.Empty);
			subject = new CompactGuid("EREREREREREREREREREREQ");

			Guid ones = GuidBuilder.Build(1);
			subject = new CompactGuid(ones);
			Assert.That(subject.Value, Is.EqualTo("EREREREREREREREREREREQ"));

			subject = new CompactGuid(ones);
			Assert.That(subject.Value, Is.EqualTo("EREREREREREREREREREREQ"));

			subject = new CompactGuid("EREREREREREREREREREREQ");
			Assert.That(subject.Guid, Is.EqualTo(ones));

			string representation = CompactGuid.Encode(ones);
			Assert.That(representation, Is.EqualTo("EREREREREREREREREREREQ"));
			Assert.That(CompactGuid.Decode(representation), Is.EqualTo(ones));
		}


		private string shortestRepresentation(Guid id)
		{
			return id.ToString("N");
		}

		#region construction

		[Test]
		public void Ctor_FromGuid_ShorterGuidRepresentation()
		{
			Guid ones = GuidBuilder.Build(1);
			var subject = new CompactGuid(ones);

			Assert.That(subject.Guid, Is.EqualTo(ones));
			Assert.That(subject.Value, Has.Length.LessThan(shortestRepresentation(ones).Length));
		}

		[Test]
		public void Ctor_FromValue_ReconstructsGuid()
		{
			var compact = "EREREREREREREREREREREQ";
			var subject = new CompactGuid(compact);

			Assert.That(subject.Guid, Is.EqualTo(GuidBuilder.Build(1)));
			Assert.That(subject.Value, Is.EqualTo(compact));
		}

		[Test]
		public void ToString_SameAsValue()
		{
			var compact = "EREREREREREREREREREREQ";
			var subject = new CompactGuid(compact);

			Assert.That(subject.ToString(), Is.EqualTo(compact));
		}

		#endregion

		#region codification

		[Test]
		public void Encode_String_CompactsGuidRepresentation()
		{
			string representation = shortestRepresentation(GuidBuilder.Build(1));

			string compact = CompactGuid.Encode(representation);
			string compactOnes = "EREREREREREREREREREREQ";
			Assert.That(compact, Is.EqualTo(compactOnes));
		}

		[Test]
		public void Encode_NotAGuid_Exception()
		{
			Assert.That(() => CompactGuid.Encode("notAGuid"), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Encode_Guid_CompactGuidRepresentation()
		{
			string compact = CompactGuid.Encode(GuidBuilder.Build(1));
			string compactOnes = "EREREREREREREREREREREQ";
			Assert.That(compact, Is.EqualTo(compactOnes));
		}

		[Test]
		public void Decode_CompactRepresentation_Guid()
		{
			string compactOnes = "EREREREREREREREREREREQ";
			Assert.That(CompactGuid.Decode(compactOnes), Is.EqualTo(GuidBuilder.Build(1)));
		}

		[Test]
		public void Decode_NotACompactRepresentation_Exception()
		{
			Assert.That(() => CompactGuid.Decode("notAGuid"), Throws.InstanceOf<FormatException>());
		}

		#endregion


		[Test]
		public void Equality_TrueIfSameGuid()
		{
			CompactGuid one = new CompactGuid(GuidBuilder.Build(1)),
				anotherOne = new CompactGuid(GuidBuilder.Build(1));

			Assert.That(one.Equals(anotherOne), Is.True);
			Assert.That(anotherOne == one, Is.True);
			Assert.That(one != anotherOne, Is.False);
		}

		[Test]
		public void Inequality_FalseIfDifferentGuid()
		{
			CompactGuid one = new CompactGuid(GuidBuilder.Build(1)),
				two = new CompactGuid(GuidBuilder.Build(2));

			Assert.That(one.Equals(two), Is.False);
			Assert.That(two == one, Is.False);
			Assert.That(one != two, Is.True);
		}
	}
}