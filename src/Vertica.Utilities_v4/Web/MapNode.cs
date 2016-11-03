using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using Vertica.Utilities_v4.Extensions.ObjectExt;

namespace Vertica.Utilities_v4.Web
{
	[Obsolete(".NET Standard")]
	public class MapNode
	{
		public MapNode(Uri url, string title)
		{
			_url = url;
			_title = title;
		}

		internal MapNode(Uri url, string title, XmlNode innerNode)
			: this(url, title)
		{
			_innerNode = innerNode;
		}

		private readonly Uri _url;
		public Uri Url
		{
			get { return _url; }
		}

		private readonly string _title;
		public string Title
		{
			get { return _title; }
		}

		private readonly XmlNode _innerNode;
		internal XmlNode InnerNode
		{
			get { return _innerNode; }
		}

		public void AppendAttribute(string name, string value)
		{
			if (_innerNode == null)
				return;

			var attribute = _innerNode.OwnerDocument.CreateAttribute(name);
			attribute.Value = value;

			_innerNode.Attributes.Append(attribute);
		}

		public static XElement Build(Uri url, string title, params XElement[] nodes)
		{
			return new XElement(SiteMapBuilder.Ns + SiteMapBuilder.SiteMapNode,
				new XAttribute(SiteMapBuilder.Url, url.SafeToString(string.Empty)),
				new XAttribute(SiteMapBuilder.Title, title ?? string.Empty),
				nodes);
		}

		public static XElement Build(Uri url, string title, object extraAttributes, params XElement[] nodes)
		{
			var ele = new XElement(SiteMapBuilder.Ns + SiteMapBuilder.SiteMapNode,
				new XAttribute(SiteMapBuilder.Url, url.SafeToString(string.Empty)),
				new XAttribute(SiteMapBuilder.Title, title ?? string.Empty),
				nodes);
			if (extraAttributes != null)
			{
				foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(extraAttributes))
				{
					ele.SetAttributeValue(property.Name, property.GetValue(extraAttributes));
				}
			}

			return ele;
		}
	}
}