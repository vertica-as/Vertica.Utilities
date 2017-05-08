using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework.Constraints;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web.Support
{
	public class EquivalentSiteMapConstraint : Constraint
	{
		private readonly Constraint _delegate;
		public EquivalentSiteMapConstraint(XDocument expected)
		{
			IEqualityComparer<string> ordinal = StringComparer.Ordinal;
			_delegate = new EqualConstraint(expected.ToString()).Using(ordinal);
		}

		public override bool Matches(object current)
		{
			actual = current;

			var xml = current as string;
			if (xml == null)
			{
				var builder = current as SiteMapBuilder;
				if (builder == null) throw new Exception("actual must be either a string or a SiteBuilder");
				xml = builder.RawXml;
			}
			return _delegate.Matches(XDocument.Parse(xml).ToString());
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			_delegate.WriteDescriptionTo(writer);
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			_delegate.WriteActualValueTo(writer);
		}

		public override void WriteMessageTo(MessageWriter writer)
		{
			_delegate.WriteMessageTo(writer);
		}
	}
}