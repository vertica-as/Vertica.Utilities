using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using NUnit.Framework;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web
{
	[TestFixture]
	public class HtmlTester
	{
		[Test]
		public void Tags_AreLowercased()
		{
			Assert.That(HtmlTextWriterTag.Table.Write(), Is.EqualTo("table"));
		}

		[Test]
		public void Attributes_AreLowercased()
		{
			Assert.That(HtmlTextWriterAttribute.Href.Write(), Is.EqualTo("href"));
		}

		private int maxValue<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return Enumeration.GetValues<TEnum>().Cast<int>().Max();
		}

		private TEnum undefined<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			dynamic undefinedValue = maxValue<TEnum>() + 1;
			return (TEnum) undefinedValue;
		}

		[Test]
		public void UndefinedValues_Exception()
		{
			Assert.That(() => undefined<HtmlTextWriterTag>().Write(),
				Throws.InstanceOf<InvalidEnumArgumentException>());

			Assert.That(() => undefined<HtmlTextWriterAttribute>().Write(),
				Throws.InstanceOf<InvalidEnumArgumentException>());
		}
	}
}