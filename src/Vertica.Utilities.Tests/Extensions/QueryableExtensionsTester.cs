using System;
using System.Linq;
using NUnit.Framework;
using Vertica.Utilities.Tests.Extensions.Support;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Comparisons;
using Vertica.Utilities.Extensions.QueryableExt;

namespace Vertica.Utilities.Tests.Extensions
{
	[TestFixture]
	public class QueryableExtensionsTester
	{
		#region Paginate

		[Test]
		public void Paginate_NullInput_Exception()
		{
			IQueryable<int> nullQuery = null;
			Assert.That(() => nullQuery.Paginate(new Pagination(1, 1)), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Paginate_EmptyInput_Empty()
		{
			IQueryable<int> emptyQuery = new int[0].AsQueryable();
			Assert.That(emptyQuery.Paginate(new Pagination(1, 1)), Is.Empty);
		}

		[Test]
		public void Paginate_WithinBounds_PageOfData()
		{
			IQueryable<int> oneToTen = Enumerable.Range(1, 10).AsQueryable();

			Assert.That(oneToTen.Paginate(new Pagination(3, 2)), Is.EqualTo(new[] { 4, 5, 6 }));
		}

		[Test]
		public void Paginate_OutsideBounds_Empty()
		{
			IQueryable<int> oneToTen = Enumerable.Range(1, 10).AsQueryable();

			Assert.That(oneToTen.Paginate(new Pagination(5, 3)), Is.Empty);
		}

		#endregion

		#region SortBy

		[Test]
		public void SortBy_NoDirection_Ascending()
		{
			var query = new[] { 2, 5, 1 }.AsQueryable();
			Assert.That(query.SortBy(), Is.EqualTo(new[] { 1, 2, 5 }));
		}

		[Test]
		public void SortBy_NullDirection_Unordered()
		{
			var query = new[] { 2, 5, 1 }.AsQueryable();
			Direction? nullDirection = null;
			Assert.That(query.SortBy(nullDirection), Is.EqualTo(query));
		}

		[Test]
		public void SortBy_AscendingDirection_Ascending()
		{
			var query = new[] { 2, 5, 1 }.AsQueryable();
			Assert.That(query.SortBy(Direction.Ascending), Is.EqualTo(new[] { 1, 2, 5 }));
		}

		[Test]
		public void SortBy_DescendingDirection_Descending()
		{
			var query = new[] { 2, 5, 1 }.AsQueryable();
			Assert.That(query.SortBy(Direction.Descending), Is.EqualTo(new[] { 5, 2, 1 }));
		}

		[Test]
		public void SortBy_SelectorNullDirection_Unordered()
		{
			OrderSubject s2_10 = new OrderSubject(2, 10), s1_7 = new OrderSubject(1, 7), s1_8 = new OrderSubject(1, 8);
			var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
			Direction? nullDirection = null;
			Assert.That(query.SortBy(s => s.I2, nullDirection), Is.EqualTo(query));
		}

		[Test]
		public void SortBy_SelectorAscendingDirection_Ascending()
		{
			OrderSubject s2_10 = new OrderSubject(2, 10), s1_7 = new OrderSubject(1, 7), s1_8 = new OrderSubject(1, 8);
			var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
			Assert.That(query.SortBy(s => s.I2, Direction.Ascending), Is.EqualTo(new[] { s1_7, s1_8, s2_10 }));
		}

		[Test]
		public void SortBy_SelectorDescendingDirection_Descending()
		{
			OrderSubject s2_10 = new OrderSubject(2, 10), s1_7 = new OrderSubject(1, 7), s1_8 = new OrderSubject(1, 8);
			var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
			Assert.That(query.SortBy(s => s.I2, Direction.Descending), Is.EqualTo(new[] { s2_10, s1_8, s1_7 }));
		}

		#endregion
	}
}