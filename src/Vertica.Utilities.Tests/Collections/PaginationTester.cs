using NUnit.Framework;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class PaginationTester
	{
		[TestCase(5u, 0u, 1u, 5u)]
		[TestCase(5u, 1u, 1u, 5u)]
		[TestCase(5u, 2u, 6u, 10u)]
		[TestCase(1u, 1u, 1u, 1u)]
		[TestCase(0u, 0u, 1u, 1u)]
		public void Pagination_FirstRecordAndLastRecord(uint pageSize, uint pageNumber, uint firstRecord, uint lastRecord)
		{
			var subject = new Pagination(pageSize, pageNumber);
			Assert.That(subject.PageSize, Is.EqualTo(pageSize));
			Assert.That(subject.PageNumber, Is.EqualTo(pageNumber));
			Assert.That(subject.FirstRecord, Is.EqualTo(firstRecord));
			Assert.That(subject.LastRecord, Is.EqualTo(lastRecord));
		}

		[TestCase(0U, 1U, 0U)]
		[TestCase(1U, 1U, 1U)]
		[TestCase(1U, 5U, 1U)]
		[TestCase(4U, 1U, 4U)]
		[TestCase(4U, 2U, 2U)]
		[TestCase(4U, 3U, 2U)]
		[TestCase(4U, 4U, 1U)]
		[TestCase(4U, 6U, 1U)]
		public void PageCount_uint_AsExpected(uint totalCount, uint pageSize, uint expectedNumberOfPages)
		{
			var subject = new Pagination(pageSize, default(uint));
			Assert.That(subject.PageCount(totalCount), Is.EqualTo(expectedNumberOfPages));
		}

		[Test]
		public void PageCount_ZeroPageSize_ZeroRegardlessOfTotalCount(
			[Values(0u, 1u, 2u)]
			uint totalCount)
		{
			var subject = new Pagination(0, default(uint));

			Assert.That(subject.PageCount(totalCount), Is.EqualTo(0));
		}

		[Test]
		public void PageCount_Zero_Zero()
		{
			var subject = new Pagination(1u, 1u);

			Assert.That(subject.PageCount(0), Is.EqualTo(0));
		}

		[Test]
		public void Next_IncreasePageNumber_DefaultIncrementOfOne()
		{
			var subject = new Pagination(42, 3);

			Pagination next = subject.Next();
			Assert.That(next, Has.Property(nameof(Pagination.PageSize)).EqualTo(subject.PageSize).And
				.Property(nameof(Pagination.PageNumber)).EqualTo(4));
		}

		[Test]
		public void Next_CustomIncreasePageNumber_CustomIncrement()
		{
			var subject = new Pagination(42, 3);

			Pagination next = subject.Next(2);
			Assert.That(next.PageNumber, Is.EqualTo(5));
		}
	}
}