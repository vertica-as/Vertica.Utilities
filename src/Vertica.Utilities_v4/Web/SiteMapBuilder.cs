using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Vertica.Utilities_v4.Web
{
	public class SiteMapBuilder
	{
		#region schema internals

		internal static readonly string SiteMapSchema = "http://schemas.microsoft.com/AspNet/SiteMap-File-1.0";
		internal static readonly string SiteMap = "siteMap";
		internal static readonly string SiteMapNode = "siteMapNode";
		internal static readonly string Url = "url";
		internal static readonly string Title = "title";

		#endregion

		private XmlDocument _dom;
		private XmlNamespaceManager _nsManager;

		public XPathNavigator Navigator { get { return _dom.CreateNavigator(); } }

		public string RawXml { get { return _dom.OuterXml; } }
		
		public MapNode Create(string rootUrl, string rootTitle)
		{
			setRoot();

			XmlNode rootXmlNode = setRootNode(rootUrl, rootTitle);
			var rootNode = new MapNode(rootUrl, rootTitle, rootXmlNode);

			return rootNode;
		}

		public MapNode AppendNode(MapNode parent, string url, string title)
		{
			if (parent.InnerNode == null)
			{
				ExceptionHelper.ThrowArgumentException("parent",
					Resources.Exceptions.SiteMapBuilder_NoContextParentTemplate,
					typeof(SiteMapBuilder).Name);
			}

			XmlNode xmlNode = createNode(url, title);
			parent.InnerNode.AppendChild(xmlNode);
			return new MapNode(url, title, xmlNode);
		}

		public void Save(string fileName, bool performBackup, string backupExtension)
		{
			checkBeforeSave();
			if (performBackup)
			{
				backupExistingFile(fileName, backupExtension);
			}

			_dom.Save(fileName);
		}

		public void Save(string fileName, bool performBackup)
		{
			Save(fileName, performBackup, DefaultBackupExtension);
		}

		public void Save(string fileName)
		{
			Save(fileName, false, DefaultBackupExtension);
		}

		public static readonly string DefaultBackupExtension = ".old";

		private string ensureSingledot(string extension)
		{
			return extension.StartsWith(".") ? extension : "." + extension;
		}

		private void backupExistingFile(string fileName, string backupExtension)
		{
			if (File.Exists(fileName))
			{
				string backupFileName = fileName + ensureSingledot(backupExtension);
				File.Copy(fileName, backupFileName, true);
			}
		}

		private void setRoot()
		{
			_dom = new XmlDocument();

			_dom.AppendChild(_dom.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null));
			_dom.AppendChild(_dom.CreateElement(SiteMap, SiteMapSchema));

			_nsManager = new XmlNamespaceManager(_dom.NameTable);
			_nsManager.AddNamespace("s", SiteMapSchema);
		}

		private XmlNode setRootNode(string url, string title)
		{
			XmlNode rootNode = createNode(url, title);
			_dom.DocumentElement.AppendChild(rootNode);
			return rootNode;
		}

		private XmlElement createNode(string url, string title)
		{
			XmlElement node = _dom.CreateElement(SiteMapNode, SiteMapSchema);
			appendAttribute(node, Url, url);
			appendAttribute(node, Title, title);
			return node;
		}

		private void appendAttribute(XmlNode element, string name, string value)
		{
			XmlAttribute attribute = _dom.CreateAttribute(name);
			attribute.Value = value;
			element.Attributes.Append(attribute);
		}

		private void checkBeforeSave()
		{
			if (_dom == null)
			{
				throw new NotSupportedException(Resources.Exceptions.SiteMapBuilder_SaveBeforeCreate);
			}
		}

		internal static readonly XNamespace Ns = SiteMapSchema;
		public static XDocument Build(params XElement[] nodes)
		{
			var doc = new XDocument(
				new XDeclaration("1.0", "utf-8", null),
				new XElement(Ns + "siteMap", nodes));
			return doc;
		}
	}
}