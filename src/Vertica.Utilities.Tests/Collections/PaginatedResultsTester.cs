using NUnit.Framework;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class PaginatedResultsTester
	{
		[Test]
		public void Records_FirstPage_OneToPageSize()
		{
			var pagination = new Pagination(3, 1);
			var subject = new PaginatedResults<int>(new[] { 10, 20, 30 }, 7, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(1));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(3));
		}

		[Test]
		public void Records_CompletePage_FirstRecordPlusPageSize()
		{
			var pagination = new Pagination(3, 2);
			var subject = new PaginatedResults<int>(new[] { 40, 50, 60 }, 7, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(4));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(6));
		}

		[Test]
		public void Records_IncompletePage_FirstRecordPlusResultsLength()
		{
			var pagination = new Pagination(3, 3);
			var subject = new PaginatedResults<int>(new[] { 70 }, 7, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(7));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(7));

			subject = new PaginatedResults<int>(new[] { 70, 80 }, 8, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(7));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(8));
		}

		[Test]
		public void Records_LessThanOnePage_OneToLength()
		{
			var pagination = new Pagination(3, 1);
			var subject = new PaginatedResults<int>(new[] { 10 }, 1, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(1));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(1));

			subject = new PaginatedResults<int>(new[] { 10, 20 }, 2, pagination);

			Assert.That(subject.RecordNumbers.LowerBound, Is.EqualTo(1));
			Assert.That(subject.RecordNumbers.UpperBound, Is.EqualTo(2));
		}

		[Test]
		public void Project_MapsToNewForm_KeepsSameMeta()
		{
			var pagination = new Pagination(3, 1);
			int totalResults = 5;
			var subject = new PaginatedResults<int>(new[] { 10, 20 }, totalResults, pagination);

			PaginatedResults<string> projection = subject.Project(x => x.ToString());

			Assert.That(projection.Pagination, Is.EqualTo(subject.Pagination));
			Assert.That(projection.Pagination.PageNumber, Is.EqualTo(subject.Pagination.PageNumber));
			Assert.That(projection.Pagination.PageSize, Is.EqualTo(subject.Pagination.PageSize));

			Assert.That(projection.TotalResults, Is.EqualTo(subject.TotalResults).And.EqualTo(5));

			Assert.That(projection.PageOfResults, Is.EqualTo(new[] { "10", "20" }));
		}
	}
}