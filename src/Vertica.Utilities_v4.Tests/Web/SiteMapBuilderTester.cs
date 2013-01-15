using System.Xml;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Tests.Web.Support;
using Vertica.Utilities_v4.Web;

namespace Vertica.Utilities_v4.Tests.Web
{
	[TestFixture]
	public class SiteMapBuilderTester
	{
		#region MapNode

		[Test]
		public void MapNode_ExternalCreation_PropertiesSetAndNullInnerNode()
		{
			var subject = new MapNode("url", "title");
			Assert.That(subject.Url, Is.EqualTo("url"));
			Assert.That(subject.Title, Is.EqualTo("title"));
			Assert.That(subject.InnerNode, Is.Null);
		}

		[Test]
		public void MapNode_InternalCreation_PropertiesSetAndNotNullInnerNode()
		{
			var dom = new XmlDocument();
			XmlNode xmlNode = dom.CreateElement("name");

			var node = new MapNode("url", "title", xmlNode);
			Assert.That(node.Url, Is.EqualTo("url"));
			Assert.That(node.Title, Is.EqualTo("title"));
			Assert.That(node.InnerNode, Is.Not.Null);
		}

		#endregion

		#region Create

		[Test]
		public void Create_BothValued_RootNodeCreated()
		{
			string url = "url", title = "title";

			var subject = new SiteMapBuilder();
			MapNode node = subject.Create(url, title);

			Assert.That(node.Title, Is.EqualTo(title));
			Assert.That(node.Url, Is.EqualTo(url));
			Assert.That(node.InnerNode, Is.Not.Null);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(url, title))));
		}

		[Test]
		public void Create_EmptyUrl_RootNodeWithEmptyUrl()
		{
			string url = string.Empty, title = "title";

			var subject = new SiteMapBuilder();
			MapNode node = subject.Create(url, title);

			Assert.That(node.Title, Is.EqualTo(title));
			Assert.That(node.Url, Is.Empty);
			Assert.That(node.InnerNode, Is.Not.Null);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(url, title))));
		}

		[Test]
		public void Create_EmptyTitle_RootNodeWithEmptyTitle()
		{
			string url = "url", title = string.Empty;

			var subject = new SiteMapBuilder();
			MapNode node = subject.Create(url, title);

			Assert.That(node.Title, Is.Empty);
			Assert.That(node.Url, Is.EqualTo(url));
			Assert.That(node.InnerNode, Is.Not.Null);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(url, title))));
		}

		[Test]
		public void Create_NullUrl_RootNodeWithNullUrl()
		{
			string url = null, title = "title";

			var subject = new SiteMapBuilder();
			MapNode node = subject.Create(url, title);

			Assert.That(node.Title, Is.EqualTo(title));
			Assert.That(node.Url, Is.Null);
			Assert.That(node.InnerNode, Is.Not.Null);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(url, title))));
		}

		[Test]
		public void Create_NullTitle_RootNodeWithNullTitle()
		{
			string url = "url", title = null;

			var subject = new SiteMapBuilder();
			MapNode node = subject.Create(url, title);

			Assert.That(node.Title, Is.Null);
			Assert.That(node.Url, Is.EqualTo(url));
			Assert.That(node.InnerNode, Is.Not.Null);

			subject.Save(PersistentSitemap.FilePath);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(url, title))));
		}

		#endregion

		#region AppendNode

		[Test]
		public void AppendNode_NodeNotInContext_Exception()
		{
			string url = "url", title = "title";

			var outOfContext = new MapNode(url, title);

			var subject = new SiteMapBuilder();
			subject.Create(url, title);
			Assert.That(() => subject.AppendNode(outOfContext, url, title), Throws.ArgumentException);
		}

		[Test]
		public void AppendNode_ToRoot_AddedNodeToRoot()
		{
			string url = "url", title = "title";
			var subject = new SiteMapBuilder();
			MapNode root = subject.Create(string.Empty, string.Empty);

			MapNode node = subject.AppendNode(root, url, title);

			Assert.That(node.Title, Is.EqualTo(title));
			Assert.That(node.Url, Is.EqualTo(url));
			Assert.That(node.InnerNode, Is.Not.Null);

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
					MapNode.Build(string.Empty, string.Empty,
						MapNode.Build(url, title))
				)));
		}

		[Test]
		public void AppendNode_TwoNodesToRoot_AddedNodeToRoot()
		{
			var subject = new SiteMapBuilder();
			MapNode root = subject.Create(string.Empty, string.Empty);

			subject.AppendNode(root, "url1", "title1");
			subject.AppendNode(root, "url2", "title2");

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
				MapNode.Build(null, null,
					MapNode.Build("url1", "title1"),
					MapNode.Build("url2", "title2"))
				)));
		}

		[Test]
		public void AppendNode_OneToRootOtherToAppended_AddedNodeToRoot()
		{
			var subject = new SiteMapBuilder();
			MapNode root = subject.Create(string.Empty, string.Empty);

			MapNode node = subject.AppendNode(root, "url1", "title1");
			subject.AppendNode(node, "url2", "title2");

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
				MapNode.Build(null, null,
					MapNode.Build("url1", "title1",
						MapNode.Build("url2", "title2")))
				)));
		}

		[Test]
		public void AppendNode_OneToRootOtherToAppendedAnotherToRoot_AddedNodeToRoot()
		{
			var subject = new SiteMapBuilder();
			MapNode root = subject.Create(string.Empty, string.Empty);

			MapNode node = subject.AppendNode(root, "url1", "title1");
			subject.AppendNode(node, "url2", "title2");
			subject.AppendNode(root, "url3", "title3");

			Assert.That(subject, Must.Be.EquivalentTo(SiteMapBuilder.Build(
				MapNode.Build(null, null,
					MapNode.Build("url1", "title1",
						MapNode.Build("url2", "title2")),
					MapNode.Build("url3", "title3"))
				)));
		}

		#endregion
	}
}