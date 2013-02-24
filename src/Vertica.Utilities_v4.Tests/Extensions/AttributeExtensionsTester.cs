using NUnit.Framework;
using Vertica.Utilities_v4.Extensions.AttributeExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class AttributeExtensionsTester
	{
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
	}
}