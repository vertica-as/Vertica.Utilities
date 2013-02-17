﻿using System;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;

namespace Vertica.Utilities_v4.Tests.Collections
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


		[TestCase(0U, 0U, 0U, ExpectedException = typeof(DivideByZeroException))]
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
	}
}