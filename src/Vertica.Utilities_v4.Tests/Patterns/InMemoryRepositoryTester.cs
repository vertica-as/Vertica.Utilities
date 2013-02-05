using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertica.Utilities_v4.Collections;
using Vertica.Utilities_v4.Patterns;
using Vertica.Utilities_v4.Tests.Patterns.Support;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	[TestFixture]
	public class InMemoryRepositoryTester
	{
		[Test]
		public void Ctor_NoInitialData_Empty()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>();

			Assert.That(subject.Count(), Is.EqualTo(0));
			Assert.That(subject.Count(s => true), Is.EqualTo(0));
			Assert.That(subject.Find(), Is.Empty);
			Assert.That(subject.Find(s => true), Is.Empty);
			Assert.That(subject.Find(0), Is.Empty);
			Assert.That(subject.FindAll(), Is.Empty);
			Assert.That(subject.FindAll(s => true), Is.Empty);

			Assert.That(() => subject.FindOne(0), Throws.InvalidOperationException);
			Assert.That(() => subject.FindOne(s => true), Throws.InvalidOperationException);

			RepositorySubject found;
			Assert.That(subject.TryFindOne(s => true, out found), Is.False);
			Assert.That(found, Is.Null);
		}

		#region Count

		[Test]
		public void Count_ReturnsNumberOfElements()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			Assert.That(subject.Count(), Is.EqualTo(2));
			Assert.That(subject.Count(e => true), Is.EqualTo(2));
		}

		[Test]
		public void Count_PerformsFiltering()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			Assert.That(subject.Count(e => e.Property.Equals("2")), Is.EqualTo(1));
		}

		#endregion

		#region Add

		[Test]
		public void Add_AddsItems()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			subject.Add(3);
			Assert.That(subject.Count(), Is.EqualTo(3));
			Assert.That(subject.FindOne(e => e.Id == 3), Is.Not.Null);
		}

		[Test]
		public void Add_AlreadyExisting_AddThemNonetheless()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			subject.Add(1);
			Assert.That(subject.Count(), Is.EqualTo(3));
			Assert.That(subject.FindAll(e => e.Id == 1).Count, Is.EqualTo(2));
		}

		#endregion

		#region Remove

		[Test]
		public void Remove_AlreadyAdded_RemovesThem()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			var existingEntity = subject.FindOne(1);
			subject.Remove(existingEntity);
			Assert.That(subject.Count(), Is.EqualTo(1));
			Assert.That(subject.Find(1), Is.Empty);
		}

		[Test]
		public void Remove_NotAddedButSameId_RemovesThem()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			var existingEntity = new RepositorySubject(1);
			subject.Remove(existingEntity);
			Assert.That(subject.Count(), Is.EqualTo(1));
			Assert.That(subject.Find(1), Is.Empty);
		}

		[Test]
		public void Remove_NotAdded_NoOp()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			var nonExistingEntity = new RepositorySubject(3);
			subject.Remove(nonExistingEntity);
			Assert.That(subject.Count(), Is.EqualTo(2));
		}

		#endregion

		#region FindOne

		[Test]
		public void FindOne_ExistingId_FindsTheElementsWithSameId()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			var entity = subject.FindOne(1);
			Assert.That(entity, Is.Not.Null);
			Assert.That(entity.Id, Is.EqualTo(1));
			Assert.That(entity.Property, Is.EqualTo("1"));
		}

		[Test]
		public void FindOne_NonExistingId_Exception()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			Assert.Throws<InvalidOperationException>(() => subject.FindOne(3));
		}

		[Test]
		public void FindOne_ExistingDuplicatedId_Exception()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2, new RepositorySubject(1, string.Empty));
			Assert.Throws<InvalidOperationException>(() => subject.FindOne(1));
		}

		[Test]
		public void FindOne_MatchingExpression_ElementFound()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			var entity = subject.FindOne(e => e.Property.StartsWith("1"));
			Assert.That(entity, Is.Not.Null);
			Assert.That(entity.Id, Is.EqualTo(1));
			Assert.That(entity.Property, Is.EqualTo("1"));
		}

		[Test]
		public void FindOne_NonMatchingExpression_Exception()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			Assert.Throws<InvalidOperationException>(() => subject.FindOne(e => string.IsNullOrEmpty(e.Property)));
		}

		[Test]
		public void FindOne_MultipleResults_Exception()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(new RepositorySubject(1, string.Empty), 2);
			subject.Add(new RepositorySubject(1, "one"));
			Assert.Throws<InvalidOperationException>(() => subject.FindOne(e => e.Id % 2 != 0));
		}

		#endregion

		#region TryFindOne

		[Test]
		public void TryFindOne_MatchingExpression_ElementFound()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			RepositorySubject entity;
			Assert.That(subject.TryFindOne(e => e.Property.StartsWith("1"), out entity), Is.True);
			Assert.That(entity, Is.Not.Null);
			Assert.That(entity.Id, Is.EqualTo(1));
			Assert.That(entity.Property, Is.EqualTo("1"));
		}

		[Test]
		public void TryFindOne_NonMatchingExpression_Null()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);

			RepositorySubject entity;

			Assert.That(subject.TryFindOne(e => string.IsNullOrEmpty(e.Property), out entity), Is.False);
			Assert.That(entity, Is.Null);
		}

		[Test]
		public void TryFindOne_MultipleResults_Null()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(new RepositorySubject(1, string.Empty), 2);
			subject.Add(new RepositorySubject(1, "one"));
			RepositorySubject entity;
			Assert.That(subject.TryFindOne(e => e.Id % 2 != 0, out entity), Is.False);
			Assert.That(entity, Is.Null);
		}

		#endregion

		#region FindAll

		[Test]
		public void FindAll_AllReturned()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			IReadOnlyList<RepositorySubject> all = subject.FindAll();
			Assert.That(all.Count, Is.EqualTo(2));
			Assert.That(all[0].Id, Is.EqualTo(1));
			Assert.That(all[0].Property, Is.EqualTo("1"));
			Assert.That(all[1].Id, Is.EqualTo(2));
			Assert.That(all[1].Property, Is.EqualTo("2"));
		}

		[Test]
		public void Findall_Empty_EmptyReturned()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>();
			IReadOnlyList<RepositorySubject> all = subject.FindAll();
			Assert.That(all, Is.Empty);
		}

		[Test]
		public void FindAll_MatchingExpression_MatchesReturned()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2, new RepositorySubject(3, null));
			IReadOnlyList<RepositorySubject> all = subject.FindAll(e => !string.IsNullOrEmpty(e.Property));
			Assert.That(all.Count, Is.EqualTo(2));
			Assert.That(all[0].Id, Is.EqualTo(1));
			Assert.That(all[0].Property, Is.EqualTo("1"));
			Assert.That(all[1].Id, Is.EqualTo(2));
			Assert.That(all[1].Property, Is.EqualTo("2"));
		}

		[Test]
		public void FindAll_NonMatchingExpression_EmptyReturned()
		{
			var subject = new InMemoryRepository<RepositorySubject, int>(1, 2);
			IReadOnlyList<RepositorySubject> all = subject.FindAll(e => string.IsNullOrEmpty(e.Property));
			Assert.That(all, Is.Empty);
		}

		#endregion
	}
}