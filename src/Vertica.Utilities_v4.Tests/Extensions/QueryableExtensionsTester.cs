using System;
using System.Linq;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Extensions.QueryableExt;

namespace Vertica.Utilities_v4.Tests.Extensions
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
	}
}