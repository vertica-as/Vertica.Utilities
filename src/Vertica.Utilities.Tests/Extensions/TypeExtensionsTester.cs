using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.TypeExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class TypeExtensionsTester
	{
		[TestCase(typeof(DateTime), "DateTime")]
		[TestCase(typeof(Func<int>), "Func<Int32>", Description = "close generic types")]
		[TestCase(typeof(Func<int, string>), "Func<Int32, String>", Description = "multiple close generic types")]
		[TestCase(typeof(Func<>), "Func<>", Description = "open generic types")]
		[TestCase(typeof(Func<,>), "Func<,>", Description = "multiple open generic types")]
		public void LongName_NoNamespace_Spec(Type t, string longName)
		{
			Assert.That(t.LongName(), Is.EqualTo(longName));
			Assert.That(t.LongName(includeNamespace: false), Is.EqualTo(longName));
		}

		[TestCase(typeof(DateTime), "System.DateTime")]
		[TestCase(typeof(Func<int>), "System.Func<System.Int32>")]
		[TestCase(typeof(Func<int, string>), "System.Func<System.Int32, System.String>")]
		[TestCase(typeof(Func<>), "System.Func<>")]
		[TestCase(typeof(Func<,>), "System.Func<,>")]
		public void LongName_WithNamespace_Spec(Type t, string longName)
		{
			Assert.That(t.LongName(includeNamespace: true), Is.EqualTo(longName));
		}

		[Test]
		public void GetDefault_SameResultAsDefaultOperator()
		{
			Assert.That(typeof(int).GetDefault<int>(), Is.EqualTo(default(int)));
			Assert.That(typeof(string).GetDefault<string>(), Is.EqualTo(default(string)));
			Assert.That(typeof(NonZeroEnum).GetDefault<NonZeroEnum>(), Is.EqualTo(default(NonZeroEnum)));

			Assert.That(typeof(int).GetDefault(), Is.EqualTo(default(int)));
			Assert.That(typeof(string).GetDefault(), Is.EqualTo(default(string)));
			Assert.That(typeof(NonZeroEnum).GetDefault(), Is.EqualTo(default(NonZeroEnum)));
		}

		[TestCase(typeof(int), false)]
		[TestCase(typeof(string), false)]
		[TestCase(typeof(NonZeroEnum), false)]
		[TestCase(typeof(Action<>), false)]
		[TestCase(typeof(Action<int>), false)]
		[TestCase(typeof(Action<string>), false)]
		[TestCase(typeof(IServiceProvider), false)]
		[TestCase(typeof(int?), true)]
		[TestCase(typeof(NonZeroEnum?), true)]
		public void IsNullable_TrueWhenNullableValueType(Type t, bool value)
		{
			Assert.That(t.IsNullable(), Is.EqualTo(value));
		}

		[TestCase(typeof(int), false)]
		[TestCase(typeof(string), true)]
		[TestCase(typeof(NonZeroEnum), false)]
		[TestCase(typeof(Action<>), true)]
		[TestCase(typeof(Action<int>), true)]
		[TestCase(typeof(Action<string>), true)]
		[TestCase(typeof(IServiceProvider), true)]
		[TestCase(typeof(int?), true)]
		[TestCase(typeof(NonZeroEnum?), true)]
		public void CanBeNulled_TrueWhenNullableOrReference(Type t, bool value)
		{
			Assert.That(t.CanBeNulled(), Is.EqualTo(value));
		}
	}
}