using System;
using System.Linq;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Extensions.EnumerableExt;

namespace Vertica.Utilities_v4.Tests.Collections
{
	[TestFixture]
	public class TreeTester
	{
		[Test]
		public void Build_Category_Tree()
		{
			var categories = new[]
			{
				new Category { Id = 1},
				new Category { Id = 2},
				new Category { Id = 3, ParentId = 1 },
				new Category { Id = 4, ParentId = 5 }
			};

			Tree<Category, int> tree = categories.ToTree(c => c.Id, (c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.That(tree.Count(), Is.EqualTo(2));
			Assert.That(tree.ElementAt(0).Model, Is.SameAs(categories[0]));
			Assert.That(tree.ElementAt(1).Model, Is.SameAs(categories[1]));

			Assert.That(tree.ElementAt(0).Count(), Is.EqualTo(1));
			Assert.That(tree.ElementAt(0).ElementAt(0).Model, Is.SameAs(categories[2]));

			Assert.That(tree.Orphans().Count(), Is.EqualTo(1));
			Assert.That(tree.Orphans().ElementAt(0), Is.EqualTo(categories[3]));

			Assert.That(tree[1], Is.Not.Null);
			Assert.That(tree[1].Model, Is.SameAs(categories[0]));
			Assert.That(tree[4], Is.Null);

			TreeNode<Category> node;
			Assert.That(tree.TryGet(5, out node), Is.False);
			Assert.That(tree.TryGet(3, out node), Is.True);
			Assert.That(node.Model, Is.SameAs(categories[2]));
		}

		public class Category
		{
			public int Id { get; set; }
			public int? ParentId { get; set; }
		}

		[Test]
		public void Build_Family_Tree_Of_Men_With_Projection()
		{
			var members = new[]
			{
				new FamilyMember { Name = "Son", ParentName = "Dad"},
				new FamilyMember { Name = "Grand Dad" },
				new FamilyMember { Name = "Dad", ParentName = "Grand dad" }
			};

			Tree<FamilyMember, string, string> tree = members.ToTree(
				x => x.Name, 
				(x, p) => x.ParentName != null ? p.Value(x.ParentName) : p.None, 
				x => x.Name, 
				StringComparer.OrdinalIgnoreCase);

			Assert.That(tree.Count(), Is.EqualTo(1));
			Assert.That(tree.ElementAt(0).Model, Is.EqualTo("Grand Dad"));

			Assert.That(tree.ElementAt(0).Count(), Is.EqualTo(1));
			Assert.That(tree.ElementAt(0).ElementAt(0).Model, Is.EqualTo("Dad"));

			Assert.That(tree.ElementAt(0).ElementAt(0).Count(), Is.EqualTo(1));
			Assert.That(tree.ElementAt(0).ElementAt(0).ElementAt(0).Model, Is.EqualTo("Son"));
			Assert.That(tree.ElementAt(0).ElementAt(0).ElementAt(0).Parent.Model, Is.EqualTo("Dad"));
			Assert.That(tree.ElementAt(0).ElementAt(0).ElementAt(0).Parent.Parent.Model, Is.EqualTo("Grand Dad"));

			CollectionAssert.AreEqual(tree["Grand Dad"].Breadcrumb(), new[] { "Grand Dad" });
			CollectionAssert.AreEqual(tree["dad"].Breadcrumb(), new[] { "Grand Dad", "Dad" });
			CollectionAssert.AreEqual(tree["Son"].Breadcrumb(), new[] {"Grand Dad", "Dad", "Son"});
		}

		public class FamilyMember
		{
			public string Name { get; set; }
			public string ParentName { get; set; }
		}
	}
}