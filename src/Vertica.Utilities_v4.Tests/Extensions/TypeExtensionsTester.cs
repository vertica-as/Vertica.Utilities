using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.TypeExt;

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
	}
}