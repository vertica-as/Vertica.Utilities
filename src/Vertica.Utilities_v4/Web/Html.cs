using System;
using System.Web.UI;

namespace Vertica.Utilities_v4.Web
{
	[Obsolete(".NET Standard")]
	public static class Html
	{
		[Obsolete(".NET Standard")]
		public static string Write(this HtmlTextWriterTag tag)
		{
			Enumeration.AssertDefined(tag);
			return tag.ToString().ToLowerInvariant();
		}

		[Obsolete(".NET Standard")]
		public static string Write(this HtmlTextWriterAttribute attr)
		{
			Enumeration.AssertDefined(attr);
			return attr.ToString().ToLowerInvariant();
		}
	}
}