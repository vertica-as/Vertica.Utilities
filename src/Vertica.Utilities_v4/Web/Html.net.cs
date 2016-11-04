using System;
using System.Web.UI;

namespace Vertica.Utilities_v4.Web
{
	public static class Html
	{
		public static string Write(this HtmlTextWriterTag tag)
		{
			Enumeration.AssertDefined(tag);
			return tag.ToString().ToLowerInvariant();
		}

		public static string Write(this HtmlTextWriterAttribute attr)
		{
			Enumeration.AssertDefined(attr);
			return attr.ToString().ToLowerInvariant();
		}
	}
}