using System.ComponentModel;
using System.Dynamic;
using System.Xml;
using System.Xml.Linq;

namespace Vertica.Utilities_v4.Web
{
	public class MapNode : DynamicObject
	{
		public MapNode(string url, string title)
		{
			_url = url;
			_title = title;
		}

		internal MapNode(string url, string title, XmlNode innerNode)
			: this(url, title)
		{
			_innerNode = innerNode;
		}

		private readonly string _url;
		public string Url
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

		public static XElement Build(string url, string title, params XElement[] nodes)
		{
			return new XElement(SiteMapBuilder.Ns + SiteMapBuilder.SiteMapNode,
				new XAttribute(SiteMapBuilder.Url, url ?? ""),
				new XAttribute(SiteMapBuilder.Title, title ?? ""),
				nodes);
		}

		public static XElement Build(string url, string title, object extraAttributes, params XElement[] nodes)
		{
			var ele = new XElement(SiteMapBuilder.Ns + SiteMapBuilder.SiteMapNode,
				new XAttribute(SiteMapBuilder.Url, url ?? ""),
				new XAttribute(SiteMapBuilder.Title, title ?? ""),
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