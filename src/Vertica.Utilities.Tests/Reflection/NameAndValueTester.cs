using System;
using NUnit.Framework;
using Vertica.Utilities.Tests.Reflection.Support;
using Vertica.Utilities.Reflection;

namespace Vertica.Utilities.Tests.Reflection
{
	[TestFixture]
	public class NameAndValueTester
	{
		#region subjects

		// ReSharper disable UnusedMember.Global
		// ReSharper disable UnusedAutoPropertyAccessor.Local
		// ReSharper disable UnusedAutoPropertyAccessor.Global
		internal StaticReflectionSubjectType ComplexProperty { get; private set; }
		internal static StaticReflectionSubjectType2 ComplexStaticProperty { get; set; }

		internal int SimpleProperty { get; private set; }
		
		private readonly StaticReflectionSubjectType _complexField = new StaticReflectionSubjectType();

		private static readonly StaticReflectionSubjectType _complexStaticField = new StaticReflectionSubjectType();
		private readonly int _simpleField = 3;

		private event EventHandler Event;
		// ReSharper restore UnusedAutoPropertyAccessor.Global
		// ReSharper restore UnusedAutoPropertyAccessor.Local
		// ReSharper restore UnusedMember.Global

		#endregion

		[Test]
		public void NameOf_OneGenericArgument_InstancePropertiesOrFields_GetsTheName()
		{
			var instance = new StaticReflectionSubjectType();

			Assert.That(Name.Of(() => instance.ComplexProperty), Is.EqualTo("ComplexProperty"));
			Assert.That(Name.Of(() => ComplexProperty), Is.EqualTo("ComplexProperty"));

			Assert.That(Name.Of(() => instance.SimpleField), Is.EqualTo("SimpleField"));
			Assert.That(Name.Of(() => _simpleField), Is.EqualTo("_simpleField"));

			Assert.That(Name.Of(() => instance.ComplexField), Is.EqualTo("ComplexField"));
			Assert.That(Name.Of(() => _complexField), Is.EqualTo("_complexField"));

			Assert.That(Name.Of(() => Event), Is.EqualTo("Event"));
		}

		[Test]
		public void NameOf_OneGenericArgument_StaticPropertiesOrFields_GetsTheName()
		{
			Assert.That(Name.Of(() => StaticReflectionSubjectType.ComplexStaticProperty), Is.EqualTo("ComplexStaticProperty"));
			Assert.That(Name.Of(() => ComplexStaticProperty), Is.EqualTo("ComplexStaticProperty"));

			Assert.That(Name.Of(() => StaticReflectionSubjectType.SimpleStaticField), Is.EqualTo("SimpleStaticField"));
			Assert.That(Name.Of(() => _complexStaticField), Is.EqualTo("_complexStaticField"));
		}

		[Test]
		public void NameOf_OneGenericArgument_LocalVariablesOrArguments(
			[Values("whatever")]string argument)
		{
			var local = new StaticReflectionSubjectType();

			Assert.That(Name.Of(() => local), Is.EqualTo("local"));
			Assert.That(Name.Of(() => argument), Is.EqualTo("argument"));
		}

		[Test]
		public void NameOf_TwoGenericArguments_InstancePropertiesOrFields_GetsTheName()
		{
			Assert.That(Name.Of<StaticReflectionSubjectType, StaticReflectionSubjectType2>(i => i.ComplexProperty), Is.EqualTo("ComplexProperty"));
			Assert.That(Name.Of<NameAndValueTester, StaticReflectionSubjectType>(_ => ComplexProperty), Is.EqualTo("ComplexProperty"));

			Assert.That(Name.Of<StaticReflectionSubjectType, int>(i => i.SimpleField), Is.EqualTo("SimpleField"));
			Assert.That(Name.Of<NameAndValueTester, int>(_ => _simpleField), Is.EqualTo("_simpleField"));

			Assert.That(Name.Of<StaticReflectionSubjectType, StaticReflectionSubjectType2>(i => i.ComplexField), Is.EqualTo("ComplexField"));
			Assert.That(Name.Of<NameAndValueTester, StaticReflectionSubjectType>(_ => _complexField), Is.EqualTo("_complexField"));

			Assert.That(Name.Of<NameAndValueTester, EventHandler>(_ => Event), Is.EqualTo("Event"));
		}
		[Test]
		public void ValueOf_InstancePropertiesOrFields_GetsTheName()
		{
			var instance = new StaticReflectionSubjectType
			{
				ComplexProperty = new StaticReflectionSubjectType2()
			};
			ComplexProperty = new StaticReflectionSubjectType();
			instance.ComplexField = new StaticReflectionSubjectType2();
			Event += (_, __) => { };

			Assert.That(Value.Of(() => instance.ComplexProperty), Is.InstanceOf<StaticReflectionSubjectType2>());
			Assert.That(Value.Of(() => ComplexProperty), Is.InstanceOf<StaticReflectionSubjectType>());

			Assert.That(Value.Of(() => instance.SimpleField), Is.EqualTo(0));
			Assert.That(Value.Of(() => _simpleField), Is.EqualTo(3));

			Assert.That(Value.Of(() => instance.ComplexField), Is.InstanceOf<StaticReflectionSubjectType2>());
			Assert.That(Value.Of(() => _complexField), Is.InstanceOf<StaticReflectionSubjectType>());

			Assert.That(Value.Of(() => Event), Is.Not.Null);
		}

		[Test]
		public void ValueOf_LocalVariablesOrArguments(
			[Values("whatever")]string argument)
		{
			var local = new StaticReflectionSubjectType();

			Assert.That(Value.Of(() => local), Is.InstanceOf<StaticReflectionSubjectType>());
			Assert.That(Value.Of(() => argument), Is.EqualTo("whatever"));
		}
	}
}
