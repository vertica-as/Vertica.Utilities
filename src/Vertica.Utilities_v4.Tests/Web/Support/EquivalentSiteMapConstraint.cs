using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web.Support
{
	public class EquivalentSiteMapConstraint : DelegatingConstraint<SiteMapBuilder>
	{
		public EquivalentSiteMapConstraint(XDocument expected)
		{
			IEqualityComparer<string> ordinal = StringComparer.Ordinal;
			Delegate = new EqualConstraint(expected.ToString()).Using(ordinal);
		}

		protected override bool matches(SiteMapBuilder current)
		{
			var currentDoc = XDocument.Parse(current.RawXml);
			return Delegate.Matches(currentDoc.ToString());
		}
	}
}