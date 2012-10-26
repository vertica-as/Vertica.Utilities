using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class OptionTester
	{
		#region Some

		[Test]
		public void Some_ReferenceType_HasValue()
		{
			var subject = Option<string>.Some("something");
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.EqualTo("something"));
			Assert.That(subject.ValueOrDefault, Is.EqualTo("something"));
		}

		[Test]
		public void Some_ValueType_HasValue()
		{
			var subject = Option<decimal>.Some(3m);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.EqualTo(3m));
			Assert.That(subject.ValueOrDefault, Is.EqualTo(3m));
		}

		[Test]
		public void Some_NullableType_HasValue()
		{
			var subject = Option<int?>.Some(null);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.Null);
			Assert.That(subject.ValueOrDefault, Is.Null);
		}

		#endregion

		#region None

		[Test]
		public void None_ReferenceType_NoValue()
		{
			var subject = Option<string>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Null);
		}

		[Test]
		public void None_ValueType_NoValue()
		{
			var subject = Option<decimal>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(decimal.Zero));
		}

		[Test]
		public void None_NullableType_NoValue()
		{
			var subject = Option<int?>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Null);
		}

		[Test]
		public void None_IsSingleton()
		{
			Assert.That(Option<int?>.None, Is.SameAs(Option<int?>.None));
		}

		#region defaults

		[Test]
		public void None_WithDefaultReferenceType_NoValue()
		{
			string defaultValue = "default";
			var subject = Option<string>.NoneWithDefault(defaultValue);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(defaultValue));
		}

		[Test]
		public void None_WithDefaultValueType_NoValue()
		{
			decimal defaultValue = 5m;
			var subject = Option<decimal>.NoneWithDefault(5m);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(defaultValue));
		}
		[Test]
		public void None_WithDefaultNullableType_NoValue()
		{
			var subject = Option<int?>.NoneWithDefault(5);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(5));
		}

		[Test]
		public void None_WithDefault_IsNone()
		{
			var subject = Option<int>.NoneWithDefault(1);
			Assert.That(subject.IsNone);
		}

		[Test]
		public void None_WithDefault_IsNotSingleton()
		{
			Assert.That(Option<int>.NoneWithDefault(1), Is.Not.SameAs(Option<int>.NoneWithDefault(1)));
		}

		[Test]
		public void Some_WithTypeInference()
		{
			Option<string> some = Option.Some("dry");
			Assert.That(some.IsSome);

			some = Option<string>.Some("notDryAtAll");
			Assert.That(some.IsSome);
		}

		[Test]
		public void None_WithTypeInference()
		{
			Option<string> none = Option.None("dry");
			Assert.That(none.IsNone);

			none = Option<string>.NoneWithDefault("notDryAtAll");
			Assert.That(none.IsNone);
		}

		#endregion

		#endregion
	}
}
