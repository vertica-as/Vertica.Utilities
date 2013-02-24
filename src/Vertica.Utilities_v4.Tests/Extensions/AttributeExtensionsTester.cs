using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.AttributeExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class AttributeExtensionsTester
	{
		#region HasAttribute

		[Test]
		public void HasAttributeOnInstance_DecoratedWithAttribute_True()
		{
			Assert.That(this.HasAttribute<TestFixtureAttribute>(), Is.True);
		}

		[Test]
		public void HasAttributeOnInstance_NotDecoratedWithAttribute_False()
		{
			Assert.That(this.HasAttribute<DescriptionAttribute>(), Is.False);
		}

		[Test]
		public void HasAttributeOnInstace_ParentDecoratedWithInheritableAttribute_NoInheritance_False()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.HasAttribute<CategoryAttribute>(), Is.False);
			Assert.That(inheritor.HasAttribute<CategoryAttribute>(false), Is.False);
		}

		[Test]
		public void HasAttributeOnInstace_ParentDecoratedWithInheritableAttribute_Inheritance_True()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.HasAttribute<CategoryAttribute>(true), Is.True);
		}

		[Test]
		public void HasAttributeOnInstace_ParentDecoratedWithNonInheritableAttribute_Inheritance_False()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.HasAttribute<DescriptionAttribute>(true), Is.False);
		}

		[Test]
		public void HasAttributeOnInstace_ParentNotDecoratedWithAttribute_False()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.HasAttribute<TestAttribute>(true), Is.False);
		}

		#endregion

		#region GetAttribute

		[Test]
		public void GetAttributeOnInstance_DecoratedWithAttribute_Instance()
		{
			Assert.That(this.GetAttribute<TestFixtureAttribute>(), Is.InstanceOf<TestFixtureAttribute>());
		}

		[Test]
		public void GetAttributeOnInstance_NotDecoratedWithAttribute_Null()
		{
			Assert.That(this.GetAttribute<DescriptionAttribute>(), Is.Null);
		}

		[Test]
		public void GetAttributeOnInstace_ParentDecoratedWithInheritableAttribute_NoInheritance_Null()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.GetAttribute<CategoryAttribute>(), Is.Null);
			Assert.That(inheritor.GetAttribute<CategoryAttribute>(false), Is.Null);
		}

		[Test]
		public void GetAttributeOnInstace_ParentDecoratedWithInheritableAttribute_Inheritance_Instance()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.GetAttribute<CategoryAttribute>(true), Is.InstanceOf<CategoryAttribute>()
				.With.Property("Name").EqualTo("cat"));
		}

		[Test]
		public void GetAttributeOnInstace_ParentDecoratedWithNonInheritableAttribute_Inheritance_Null()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.GetAttribute<DescriptionAttribute>(true), Is.Null);
		}

		[Test]
		public void GetAttributeOnInstace_ParentNotDecoratedWithAttribute_Null()
		{
			var inheritor = new ParentDecoratedWithCategoryAndDecription();
			Assert.That(inheritor.GetAttribute<TestAttribute>(true), Is.Null);
		}

		#endregion

	}
}