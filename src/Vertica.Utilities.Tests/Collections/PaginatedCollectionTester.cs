using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;

namespace Vertica.Utilities_v4.Tests.Collections
{
	[TestFixture]
	public class PaginatedCollectionTester
	{
		[Test]
		public void CurrentPage_SamesAsProvidedInPagination()
		{
			uint pageSize = default(uint), pageNumber = 7;

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				Substitute.For<IEnumerable<int>>());

			Assert.That(subject.CurrentPage, Is.EqualTo(pageNumber));
		}

		[Test]
		public void PageSize_SamesAsProvidedInPagination()
		{
			uint pageSize = 5, pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				Substitute.For<IEnumerable<int>>());

			Assert.That(subject.Pagesize, Is.EqualTo(pageSize));
		}

		#region NumberOfPages

		[Test]
		public void NumberOfPages_NullCollection_0()
		{
			uint pageSize = 5, pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				null);

			Assert.That(subject.NumberOfPages, Is.EqualTo(0));
		}

		[Test]
		public void NumberOfPages_EmptyCollection_0()
		{
			uint pageSize = 5, pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				Enumerable.Empty<int>());

			Assert.That(subject.NumberOfPages, Is.EqualTo(0));
		}


		[TestCase(new[] { 1 }, 1U, 1U)]
		[TestCase(new[] { 1 }, 5U, 1U)]
		[TestCase(new[] { 1, 2, 3, 4 }, 1U, 4U)]
		[TestCase(new[] { 1, 2, 3, 4 }, 2U, 2U)]
		[TestCase(new[] { 1, 2, 3, 4 }, 3U, 2U)]
		[TestCase(new[] { 1, 2, 3, 4 }, 4U, 1U)]
		[TestCase(new[] { 1, 2, 3, 4 }, 6U, 1U)]
		public void NumberOfPages_PopulatedCollection(IEnumerable<int> collection, uint pageSize, uint expectedNumberOfPages)
		{
			uint pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				collection);

			Assert.That(subject.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
		}

		#endregion

		#region Collection

		[Test]
		public void Collection_NullCollection_Null()
		{
			uint pageSize = 5, pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				null);

			Assert.That(subject.Collection, Is.Null);
		}

		[Test]
		public void Collection_EmptyCollection_Empty()
		{
			uint pageSize = 5, pageNumber = default(uint);

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				Enumerable.Empty<int>());

			Assert.That(subject.Collection, Is.Empty);
		}

		[Test]
		public void Collection_PageNumberGreaterThanNumberOfPages_Empty()
		{
			uint pageSize = 5, pageNumber = 6;

			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				new[]{1, 2, 3});

			Assert.That(subject.NumberOfPages, Is.LessThan(pageNumber));
			Assert.That(subject.Collection, Is.Empty);
		}


		[TestCase(new[] { 1 }, 1U, 1U, new[] { 1 })]
		[TestCase(new[] { 1 }, 1U, 5U, new[] { 1 })]
		[TestCase(new[] { 1, 2, 3, 4 }, 1U, 4U, new[] { 1, 2, 3, 4 })]
		[TestCase(new[] { 1, 2, 3, 4 }, 2U, 2U, new[] { 3, 4 })]
		[TestCase(new[] { 1, 2, 3, 4 }, 2U, 3U, new[] { 4 })]
		[TestCase(new[] { 1, 2, 3, 4 }, 1U, 6U, new[] { 1, 2, 3, 4 })]
		public void Collection_PopulatedCollection(IEnumerable<int> collection, uint pageNumber, uint pageSize, IEnumerable<int> expected)
		{
			var subject = new PaginatedCollection<int>(
				new Pagination(pageSize, pageNumber),
				collection);

			Assert.That(subject.Collection, Is.EqualTo(expected));
		}

		#endregion

		#region IEnumerable<T> implementation

		[Test]
		public void PaginatedCollection_Is_ACollectionItseld()
		{
			var pagination = new Pagination(2, 1);
			var subject = new PaginatedCollection<int>(pagination,
				new[]{1, 2, 3, 4});

			Assert.That(subject, Is.EqualTo(new[]{1, 2}));

		}

		[Test]
		public void GetEnumerator_EnumeratesOverCollection()
		{
			var pagination = new Pagination(2, 1);

			var subject = new PaginatedCollection<int>(pagination,
				new[]{1, 2, 3, 4});

			IEnumerable<int> expected = new[]{1, 2};

			IEnumerator<int> subjectEnumerator = subject.GetEnumerator();
			IEnumerator<int> expectedEnumerator = expected.GetEnumerator();
			while (subjectEnumerator.MoveNext())
			{
				expectedEnumerator.MoveNext();
				Assert.That(subjectEnumerator.Current, Is.EqualTo(expectedEnumerator.Current));
			}
		}

		#endregion
	}
}