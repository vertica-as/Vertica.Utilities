using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class OptionTester
	{
		#region documentation

		[Test, Category("Exploratory")]
		public void Explore()
		{
			Option<Exception> nonMissing = Option<Exception>.Some(new Exception());
			Option<Exception> inferredNonMissing = Option.Some(new Exception());
			Assert.That(nonMissing.IsSome, Is.EqualTo(inferredNonMissing.IsSome).And.True);
			Assert.That(nonMissing.IsNone, Is.EqualTo(inferredNonMissing.IsNone).And.False);
			
			Option<Exception> missingWithoutDefault = Option<Exception>.None;
			Assert.That(missingWithoutDefault.IsSome, Is.False);
			Assert.That(missingWithoutDefault.IsNone, Is.True);

			var some = Option.Some("something");
			Assert.That(some.Value, Is
				.EqualTo(some.ValueOrDefault).And
				.EqualTo(some.GetValueOrDefault("default")).And
				.EqualTo("something"));

			var none = Option<string>.None;
			Assert.That(()=> none.Value, Throws.InvalidOperationException);
			Assert.That(none.ValueOrDefault, Is.Null);
			Assert.That(none.GetValueOrDefault("default"), Is.EqualTo("default"));

			var missingWithDefault = Option<Exception>.NoneWithDefault(new Exception());
			var inferredMissing = Option.None("default");
			Assert.That(() => inferredMissing.Value, Throws.InvalidOperationException);
			Assert.That(inferredMissing.ValueOrDefault, Is.EqualTo("default"));
			Assert.That(inferredMissing.GetValueOrDefault("overriden"), Is.EqualTo("overriden"));

			Uri url = null;
			Option<Uri> verboseCreation = url != null ?
				Option.Some(url) :
				Option.None(new Uri("#", UriKind.Relative));
			Option<Uri> lessVerboseCreation = Option.Maybe(url);	// null default
			lessVerboseCreation = Option.Maybe(url, () => new Uri("#", UriKind.Relative));

			string s = null;
			Option<string> missingString = Option.Maybe(s);
			Assert.That(missingString.IsNone, Is.True);
			Assert.That(missingString.ValueOrDefault, Is.Empty);

			Option<int> missingNullable = Option.Maybe(default(int?));
			Assert.That(missingNullable.IsNone, Is.True);
			Assert.That(missingNullable.ValueOrDefault, Is.EqualTo(default(int)));

			var missingCollection = Option.Maybe(new int[0]);
			Assert.That(missingCollection.IsNone, Is.True);
			Assert.That(missingCollection.ValueOrDefault, Is.Empty);
		}

		#endregion

		#region Value, ValueOrDefault, GetValueOrDefault

		[Test]
		public void Some_ReferenceType_HasValue()
		{
			var subject = Option<string>.Some("something");
			
			Assert.That(subject.Value, Is.EqualTo("something"));
			Assert.That(subject.ValueOrDefault, Is.EqualTo("something"));
			Assert.That(subject.GetValueOrDefault("something else"), Is.EqualTo("something"));
		}

		[Test]
		public void Some_ValueType_HasValue()
		{
			var subject = Option<decimal>.Some(3m);
			
			Assert.That(subject.Value, Is.EqualTo(3m));
			Assert.That(subject.ValueOrDefault, Is.EqualTo(3m));
			Assert.That(subject.GetValueOrDefault(5m), Is.EqualTo(3m));
		}

		[Test]
		public void None_ReferenceType_NoValue()
		{
			var subject = Option<string>.None;
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(default(string)));
			Assert.That(subject.GetValueOrDefault("5m"), Is.EqualTo("5m"));
		}

		[Test]
		public void None_ValueType_NoValue()
		{
			var subject = Option<decimal>.None;
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(default(decimal)));
			Assert.That(subject.GetValueOrDefault(10m), Is.EqualTo(10m));
		}

		[Test]
		public void None_NullableType_NoValue()
		{
			var subject = Option<int?>.None;
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(default(int?)));
			Assert.That(subject.GetValueOrDefault(12), Is.EqualTo(12));
		}

		[Test]
		public void None_WithDefaultReferenceType_NoValue()
		{
			string defaultValue = "default";
			var subject = Option<string>.NoneWithDefault(defaultValue);
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(defaultValue));
			Assert.That(subject.GetValueOrDefault("not the creation default"), Is.EqualTo("not the creation default"));
		}

		[Test]
		public void None_WithDefaultValueType_NoValue()
		{
			decimal defaultValue = 5m;
			var subject = Option<decimal>.NoneWithDefault(defaultValue);
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(defaultValue));
			Assert.That(subject.GetValueOrDefault(10m), Is.EqualTo(10m));
		}

		[Test]
		public void None_WithDefaultNullableType_NoValue()
		{
			var subject = Option<int?>.NoneWithDefault(5);
			
			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(5));
			Assert.That(subject.GetValueOrDefault(null), Is.Null);
		}

		#endregion

		#region Some

		[Test]
		public void Some_ReferenceType()
		{
			var subject = Option<string>.Some("something");
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);
		}

		[Test]
		public void Some_ValueType()
		{
			var subject = Option<decimal>.Some(3m);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);
		}

		[Test]
		public void Some_NullableType()
		{
			var subject = Option<int?>.Some(null);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);
		}

		[Test]
		public void Some_WithTypeInference()
		{
			Option<string> some = Option.Some("dry");
			Assert.That(some.IsSome);

			some = Option<string>.Some("notDryAtAll");
			Assert.That(some.IsSome);
		}

		#endregion

		#region None

		[Test]
		public void None_ReferenceType()
		{
			var subject = Option<string>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
		}

		[Test]
		public void None_ValueType()
		{
			var subject = Option<decimal>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
		}

		[Test]
		public void None_NullableType()
		{
			var subject = Option<int?>.None;
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
		}

		[Test]
		public void None_IsSingleton()
		{
			Assert.That(Option<int?>.None, Is.SameAs(Option<int?>.None));
		}

		#endregion

		#region NoneWithDefault

		[Test]
		public void None_WithDefaultReferenceType()
		{
			var subject = Option<string>.NoneWithDefault("default");
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
		}

		[Test]
		public void None_WithDefaultValueType()
		{
			var subject = Option<decimal>.NoneWithDefault(5m);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
		}

		[Test]
		public void None_WithDefaultNullableType()
		{
			var subject = Option<int?>.NoneWithDefault(5);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);
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
		public void None_WithTypeInference()
		{
			Option<string> none = Option.None("dry");
			Assert.That(none.IsNone);

			none = Option<string>.NoneWithDefault("notDryAtAll");
			Assert.That(none.IsNone);
		}

		#endregion

		#region Maybe

		[Test]
		public void Maybe_NullReferenceType_NoneWithoutDefault()
		{
			Exception reference = null;
			Option<Exception> subject = Option.Maybe(reference);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Null);
		}

		[Test]
		public void Maybe_NullReferenceType_NoneWithDefault()
		{
			Uri reference = null;
			Option<Uri> subject = Option.Maybe(reference, ()=> new Uri("#", UriKind.Relative));
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Not.Null);
		}

		[Test]
		public void Maybe_WithDefault_LazyEvaluation()
		{
			bool used = false;
			Func<Uri> nullObj =()=>
			{
				used = true;
				return new Uri("#", UriKind.Relative);
			};
			Option.Maybe(new Uri("http://localhost"), nullObj);
			Assert.That(used, Is.False);

			Option.Maybe(null, nullObj);
			Assert.That(used, Is.True);
		}

		[Test]
		public void Maybe_NullReferenceType_SomeWithValue()
		{
			var reference = new Exception("msg");
			Option<Exception> subject = Option.Maybe(reference);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.SameAs(reference));
			Assert.That(subject.ValueOrDefault, Is.SameAs(reference));
		}

		[Test]
		public void Maybe_NullNullable_NoneWithDefault()
		{
			int? @null = default(int?);
			Option<int> subject = Option.Maybe(@null);

			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.EqualTo(default(int)));
		}

		[Test]
		public void Maybe_ValuedNullable_SomeWithValue()
		{
			int? valued = 3;
			Option<int> subject = Option.Maybe(valued);

			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.EqualTo(3));
			Assert.That(subject.ValueOrDefault, Is.EqualTo(3));
			
		}

		[Test]
		public void Maybe_NullString_NoneWithDefault()
		{
			string str = null;
			Option<string> subject = Option.Maybe(str);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Empty);
		}

		[Test]
		public void Maybe_EmptyString_NoneWithDefault()
		{
			string str = null;
			Option<string> subject = Option.Maybe(str);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Empty);
		}

		[Test]
		public void Maybe_SomeString_SomeWithValue()
		{
			string str = "str";
			Option<string> subject = Option.Maybe(str);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.EqualTo(str));
			Assert.That(subject.ValueOrDefault, Is.EqualTo(str));
		}

		[Test]
		public void Maybe_NullCollection_NoneWithDefault()
		{
			IEnumerable<int> coll = null;
			Option<IEnumerable<int>> subject = Option.Maybe(coll);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Empty);
		}

		[Test]
		public void Maybe_EmptyCollection_NoneWithDefault()
		{
			IEnumerable<int> coll = new int[0];
			Option<IEnumerable<int>> subject = Option.Maybe(coll);
			Assert.That(subject.IsSome, Is.False);
			Assert.That(subject.IsNone, Is.True);

			Assert.That(() => subject.Value, Throws.InvalidOperationException);
			Assert.That(subject.ValueOrDefault, Is.Empty);
		}

		[Test]
		public void Maybe_SomeCollection_SomeWithValue()
		{
			IEnumerable<int> coll = new[] { 1, 2, 3 };
			var subject = Option.Maybe(coll);
			Assert.That(subject.IsSome, Is.True);
			Assert.That(subject.IsNone, Is.False);

			Assert.That(subject.Value, Is.EqualTo(coll));
			Assert.That(subject.ValueOrDefault, Is.EqualTo(coll));
		}

		[Test]
		public void Maybe_EmptyEnumerations_NoneWithDefault()
		{
			int[] emptyArray = new int[0];
			Option<IEnumerable<int>> subject = Option.Maybe(emptyArray);
			assertNoneCollection(subject);

			IList<int> emptyList =  new List<int>();
			subject = Option.Maybe(emptyList);
			assertNoneCollection(subject);

			ICollection<int> emptyCollection = new Collection<int>();
			subject = Option.Maybe(emptyCollection);
			assertNoneCollection(subject);
		}

		private void assertNoneCollection<T>(Option<IEnumerable<T>> none)
		{
			Assert.That(none.IsSome, Is.False);
			Assert.That(none.IsNone, Is.True);

			Assert.That(() => none.Value, Throws.InvalidOperationException);
			Assert.That(none.ValueOrDefault, Is.Empty);
		}

		#endregion

		#region equality

		[Test]
		public void Equals_SomethingAndNull_False()
		{
			Option<string> @null = null;
			Assert.That(Option.Some("value").Equals(@null), Is.False);
			Assert.That(Option<string>.None.Equals(@null), Is.False);
			Assert.That(Option.None((string)null).Equals(@null), Is.False);
		}

		[Test]
		public void Equals_SomeAndNone_False()
		{
			Assert.That(Option.Some("some").Equals(Option.None((string)null)), Is.False);
			Assert.That(Option.Some("some").Equals(Option.None("some")), Is.False);
			Assert.That(Option.Some((string)null).Equals(Option.None((string)null)), Is.False);
			Assert.That(Option.Some((string)null).Equals(Option<string>.None), Is.False);
			Assert.That(Option.Some(string.Empty).Equals(Option.None(string.Empty)), Is.False);
		}

		[Test]
		public void Equals_NoneAndSome_False()
		{
			Assert.That(Option.None("some").Equals(Option.Some((string)null)), Is.False);
			Assert.That(Option.None("some").Equals(Option.Some("some")), Is.False);
			Assert.That(Option.None((string)null).Equals(Option.Some((string)null)), Is.False);
			Assert.That(Option<string>.None.Equals(Option.Some((string)null)), Is.False);
			Assert.That(Option.None(string.Empty).Equals(Option.Some(string.Empty)), Is.False);
		}

		[Test]
		public void Equals_SomeAndSome_EqualsIfValuesAreEqual()
		{
			Uri localhost = new Uri("http://localhost/"),
				localhostIndex = new Uri("http://localhost/index.htm");

			Assert.That(Option.Some(localhost).Equals(Option.Some(localhost)), Is.True);
			Assert.That(Option.Some(localhost).Equals(Option.Some(localhostIndex)), Is.False);
		}

		[Test]
		public void Equals_NoneAndNone_EqualsIfDeaultsAreEqual()
		{
			Uri localhost = new Uri("http://localhost/"),
				localhostIndex = new Uri("http://localhost/index.htm");

			Assert.That(Option.None(localhost).Equals(Option.None(localhost)), Is.True);
			Assert.That(Option.None(localhost).Equals(Option.None(localhostIndex)), Is.False);

			Assert.That(Option.None((Uri)null).Equals(Option<Uri>.None), Is.True);
		}

		#endregion

		[Test]
		public void NullIsAlsoOption_ButPlease_Dont()
		{
			Option<int> nullable = null;
			Assert.That(()=>nullable.IsSome, Throws.InstanceOf<NullReferenceException>());
		}
	}
}
