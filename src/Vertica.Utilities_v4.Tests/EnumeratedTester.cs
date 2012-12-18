using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class EnumeratedTester
	{
		#region subjects

		class EnumeratedSubject : Enumerated<EnumeratedSubject>
		{
			private EnumeratedSubject(string name) : base(name) { }

			public static readonly EnumeratedSubject One = new EnumeratedSubject("one");
			public static readonly EnumeratedSubject Two = new EnumeratedSubject("two");
			public static readonly EnumeratedSubject Three = new EnumeratedSubject("three");
		}

		class AnotherEnumeratedSubject : Enumerated<AnotherEnumeratedSubject>
		{
			private AnotherEnumeratedSubject(string name) : base(name) { }

			public static readonly AnotherEnumeratedSubject One = new AnotherEnumeratedSubject("one");
			public static readonly AnotherEnumeratedSubject Two = new AnotherEnumeratedSubject("two");
			public static readonly AnotherEnumeratedSubject Three = new AnotherEnumeratedSubject("three");
		}

		class PoorlyDefinedEnumerated : Enumerated<PoorlyDefinedEnumerated>
		{
			private PoorlyDefinedEnumerated(string name) : base(name) { }

			public static readonly PoorlyDefinedEnumerated One = new PoorlyDefinedEnumerated("one");
			public static readonly PoorlyDefinedEnumerated Same = new PoorlyDefinedEnumerated("one");
		}

		#endregion

		[Test]
		public void Values_GivesAccessToAllValues()
		{
			var subjects = EnumeratedSubject.Values;
			Assert.That(subjects, Is.EqualTo(new[] { EnumeratedSubject.One, EnumeratedSubject.Two, EnumeratedSubject.Three }));
		}

		[Test]
		public void Parse_ExistingValue_GivesTheValue()
		{
			EnumeratedSubject parsed = EnumeratedSubject.Parse("one");
			Assert.That(parsed, Is.SameAs(EnumeratedSubject.One));
		}

		[Test]
		public void TryParse_ExistingValue_True()
		{
			EnumeratedSubject parsed;
			Assert.That(EnumeratedSubject.TryParse("one", out parsed), Is.True);
		}

		[Test]
		public void Parse_NonExistingValue_Exception()
		{
			Assert.That(() => EnumeratedSubject.Parse("nonExisting"),
				Throws.ArgumentException.With.Message.Contains("nonExisting"));
		}

		[Test]
		public void TryParse_NonExistingValue_False()
		{
			EnumeratedSubject parsed;
			Assert.That(EnumeratedSubject.TryParse("nonExisting", out parsed), Is.False);
		}

		[Test]
		public void Parsing_IsCaseSensitive()
		{
			EnumeratedSubject parsed;
			Assert.That(EnumeratedSubject.TryParse("one", out parsed), Is.True);
			Assert.That(EnumeratedSubject.TryParse("One", out parsed), Is.False);
		}

		[Test]
		public void Methods_WorkWithDifferentTypes()
		{
			Assert.That(EnumeratedSubject.Values, Is.EqualTo(new[] { EnumeratedSubject.One, EnumeratedSubject.Two, EnumeratedSubject.Three }));
			Assert.That(AnotherEnumeratedSubject.Values, Is.EqualTo(new[] { AnotherEnumeratedSubject.One, AnotherEnumeratedSubject.Two, AnotherEnumeratedSubject.Three }));

			Assert.That(EnumeratedSubject.Parse("one"), Is.SameAs(EnumeratedSubject.One));
			Assert.That(AnotherEnumeratedSubject.Parse("one"), Is.SameAs(AnotherEnumeratedSubject.One));
		}

		[Test]
		public void MembersWithSameName_InitializationException()
		{
			Assert.That(() => PoorlyDefinedEnumerated.One, Throws.InstanceOf<TypeInitializationException>());
		}
	}
}