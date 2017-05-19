using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities.Tests.Collections.Support;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Extensions.EnumerableExt;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class TreeTester
	{
		private static readonly Category 
            c1 = new Category { Id = 1 },
			c2 = new Category { Id = 2 },
			c3 = new Category { Id = 3, ParentId = 1 },
			c4_orphan = new Category { Id = 4, ParentId = 5 };

		[Test]
		public void ToTree_BuildsATree_OfWrappedModels()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			// roots
			Assert.That(tree, Must.Be.Constrained(
				Must.Have.Model(c1),
				Must.Have.Model(c2)));
			// children of c1
			Assert.That(tree[0], Must.Be.Constrained(
				Must.Have.Model(c3)));
		}

        [Test]
        public void ToTree_MultipleParentOfSameItem_BothParentsHaveSameItem()
        {
            Dictionary<int, Category> categories = new[] { c1, c2, c3 }
                .ToDictionary(x => x.Id, x => x);

            Category[] references = 
            {
                new Category { Id = c1.Id },
                new Category { Id = c2.Id },
                new Category { Id = c3.Id, ParentId = c1.Id },
                new Category { Id = c3.Id, ParentId = c2.Id } // same ID as previous, different parent
            };

            Tree<Category, Category, int> tree = references.ToTree(
                c => c.Id,
                (c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None,
                c => categories[c.Id]);

            TreeNode<Category> parent1 = tree.Get(c1.Id);
            TreeNode<Category> parent2 = tree.Get(c2.Id);
            TreeNode<Category> child = tree.Get(c3.Id);

            TreeNode<Category>[] childrenOfParent1 = parent1.ToArray();
            TreeNode<Category>[] childrenOfParent2 = parent2.ToArray();
            TreeNode<Category>[] parentsOfChild = child.Parents.ToArray();

            Assert.That(childrenOfParent1.Length, Is.EqualTo(1));
            Assert.That(childrenOfParent1[0].Model, Is.SameAs(child.Model));
            
            Assert.That(childrenOfParent2.Length, Is.EqualTo(1));
            Assert.That(childrenOfParent2[0].Model, Is.SameAs(child.Model));

            CollectionAssert.AreEqual(
                parentsOfChild.Select(x => x.Model), 
                new[] { parent1, parent2 }.Select(x => x.Model));
        }

        [Test]
		public void Orphans_ModelsWithoutParent_GoToAnotherStructure()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.That(tree.Orphans(), Must.Be.Constrained(Is.SameAs(c4_orphan)));
		}

		[Test]
		public void Get_NodeKeysInTree_DoesNotThrowAndNodeReference()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.DoesNotThrow(() => tree.Get(1));
			Assert.That(tree.Get(1).Model, Is.SameAs(c1));
		}

		[Test]
		public void Get_NodeKeysNotInTree_Throws()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(c => c.Id, (c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			KeyNotFoundException exception = Assert.Throws<KeyNotFoundException>(() => tree.Get(4), "c4 is an orphan");
			Assert.That(exception.Message, Does.Contain("4"), "contains the key in text");

			Assert.That(() => tree.Get(42), Throws.InstanceOf<KeyNotFoundException>(),
				"c42 does not even exist");
		}

		[Test]
		public void TryGet_NodeKeysInTree_TrueAndNodeReference()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			TreeNode<Category> node;
			Assert.That(tree.TryGet(3, out node), Is.True);
			Assert.That(node.Model, Is.SameAs(c3));
		}

		[Test]
		public void TryGet_NodeKeysNotInTree_FalseAndNull()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			TreeNode<Category> node;
			Assert.That(tree.TryGet(4, out node), Is.False, "c4 is an orphan");
			Assert.That(tree.TryGet(42, out node), Is.False, "c42 does not even exist");
		}

		[Test]
		public void ToTree_ModesCanBeProjectedWhileBuildingStructure()
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

			Assert.That(tree, Must.Be.Constrained(Must.Have.Model("Grand Dad")));
			Assert.That(tree[0], Must.Be.Constrained(Must.Have.Model("Dad")));
			Assert.That(tree[0][0], Must.Be.Constrained(Must.Have.Model("Son")));
		}

		[Test]
		public void Parent_MaintainsUpstreamStructure()
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

			Assert.That(tree[0][0][0].Parent, Must.Have.Model("Dad"));
			Assert.That(tree[0][0][0].Parent.Parent, Must.Have.Model("Grand Dad"));
		}

		[Test]
		public void Breadcrumb_MaintainsADownstreamPath()
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

			Assert.That(tree.Get("Grand Dad").Breadcrumb(), Is.EqualTo(new[] { "Grand Dad" }));
			Assert.That(tree.Get("dad").Breadcrumb(), Is.EqualTo(new[] { "Grand Dad", "Dad" }));
			Assert.That(tree.Get("Son").Breadcrumb(), Is.EqualTo(new[] { "Grand Dad", "Dad", "Son" }));
		}

		[Test]
		public void Indexer_Node_ThrowsOutOfRange()
		{
			var categories = new[] { c1 };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.That(() => tree[0][1], Throws.InstanceOf<ArgumentOutOfRangeException>(),
				"should not exist any node at index 1");
		}

		[Test]
		public void Indexer_Tree_ThrowsOutOfRange()
		{
			var categories = new[] { c1 };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.That(() => tree[1], Throws.InstanceOf<ArgumentOutOfRangeException>(),
				"should not exist any at index 1");
		}
        
		[Test]
		public void Count_No_Orphans()
		{
			var categories = new[] { c1, c2, c3 };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			Assert.That(tree.Count, Is.EqualTo(categories.Length));
		}

		[Test]
		public void Count_C4_Is_Orphan()
		{
			var categories = new[] { c1, c2, c3, c4_orphan };

			Tree<Category, int> tree = categories.ToTree(
				c => c.Id,
				(c, p) => c.ParentId.HasValue ? p.Value(c.ParentId.Value) : p.None);

			// c4 references a parent thats not there
			Assert.That(tree.Count, Is.EqualTo(3));
		}
	}
}