using System;
using NUnit.Framework;
using Vertica.Utilities.Collections;
using Vertica.Utilities.Eventing;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class NotifyingListTester
	{
		#region Insert

		[Test]
		public void Insert_EventsRaised()
		{
			bool insertingRaised = false, insertedRaised = false;

			var subject = new NotifyingList<int>();
			subject.Inserting += (sender, args) => insertingRaised = true;
			subject.Inserted += (sender, args) => insertedRaised = true;

			subject.Insert(0, 3);

			Assert.That(insertingRaised, Is.True);
			Assert.That(insertedRaised, Is.True);
		}

		[Test]
		public void Insert_EventsRaisedWithCorrectArgumentsAndItemAdded()
		{
			int index = 0, item = 3;

			var subject = new NotifyingList<int>();
			subject.Inserting += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
				Assert.That(e.IsCancelled, Is.False);
			};

			subject.Inserted += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
			};

			subject.Insert(index, item);

			Assert.That(subject.Count, Is.EqualTo(1));
			Assert.That(subject[index], Is.EqualTo(item));
		}

		[Test]
		public void Insert_CanCancelInsertion()
		{
			bool insertedRaised = false;

			var subject = new NotifyingList<int>();

			subject.Inserting += ((sender, e) => e.Cancel());
			subject.Inserted += (sender, args) => insertedRaised = true;

			subject.Insert(0, 3);

			Assert.That(insertedRaised, Is.False);
		}

		#endregion

		#region RemoveAt

		[Test]
		public void RemoveAt_OutOfBounds_Exception()
		{
			var subject = new NotifyingList<int> { 3 };

			Assert.That(() => subject.RemoveAt(2), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void RemoveAt_EventsRaised()
		{
			bool removingRaised = false, removedRaised = false;

			var subject = new NotifyingList<int> { 3 };
			subject.Removing += (sender, args) => removingRaised = true;
			subject.Removed += (sender, args) => removedRaised = true;

			subject.RemoveAt(0);

			Assert.That(removingRaised, Is.True);
			Assert.That(removedRaised, Is.True);
		}

		[Test]
		public void RemoveAt_EventsRaisedWithCorrectArgumentsAndItemRemoved()
		{
			int index = 0, item = 3;
			
			var subject = new NotifyingList<int> { 3 };
			subject.Removing += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
				Assert.That(e.IsCancelled, Is.False);
			};

			subject.Removed += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
			};

			subject.RemoveAt(0);

			Assert.That(subject.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveAt_CanCancelDeletion()
		{
			bool removedRaised = false;

			var subject = new NotifyingList<int> { 3 };
			subject.Removing += ((sender, e) => e.Cancel());

			subject.Removed += delegate
			{
				removedRaised = true;
			};

			subject.RemoveAt(0);
			Assert.That(removedRaised, Is.False);
			Assert.That(subject[0], Is.EqualTo(3));
		}

		#endregion

		#region Setter

		[Test]
		public void Set_OutOfBounds_Exception()
		{
			var subject = new NotifyingList<int> { 1 };

			Assert.That(() => subject[2] = 3, Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Set_EventsRaised()
		{
			bool settingRaised = false, setRaised = false;

			var subject = new NotifyingList<int>();

			subject.Setting += (sender, args) => settingRaised = true;
			subject.Set += (sender, args) => setRaised = true;

			subject.Add(1);
			subject[0] = 3;

			Assert.That(settingRaised, Is.True);
			Assert.That(setRaised, Is.True);

		}

		[Test]
		public void Set_EventsRaisedWithCorrectArgumentsAndItemSet()
		{
			int index = 0, newValue = 3, previousValue = 1;

			var subject = new NotifyingList<int>();

			subject.Setting += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(previousValue));
				Assert.That(e.NewValue, Is.EqualTo(newValue));
				Assert.That(e.IsCancelled, Is.False);
			};

			subject.Set += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.OldValue, Is.EqualTo(previousValue));
				Assert.That(e.Value, Is.EqualTo(newValue));
			};

			subject.Add(previousValue);
			subject[index] = newValue;

			Assert.That(subject.Count, Is.EqualTo(1));
			Assert.That(subject[index], Is.EqualTo(newValue));
		}

		[Test]
		public void Set_CanCancelSetting()
		{
			bool setRaised = false;

			var subject = new NotifyingList<int>();
			subject.Setting += ((sender, e) => e.Cancel());

			subject.Set += (sender, args) => setRaised = true;

			subject.Insert(0, 1);
			subject[0] = 3;

			Assert.That(setRaised, Is.False);
		}

		#endregion

		#region Add

		[Test]
		public void Add_EventsRaised()
		{
			bool insertingRaised = false, insertedRaised = false;

			var subject = new NotifyingList<int>();
			subject.Inserting += (sender, args) => insertingRaised = true;
			subject.Inserted += (sender, args) => insertedRaised = true;

			subject.Add(3);
			Assert.That(insertingRaised, Is.True);
			Assert.That(insertedRaised, Is.True);
		}

		[Test]
		public void Add_EventsRaisedWithCorrectArgumentsAndItemAdded()
		{
			int index = 0, item = 3;

			var subject = new NotifyingList<int>();
			subject.Inserting += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
				Assert.That(e.IsCancelled, Is.False);
			};

			subject.Inserted += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
			};

			subject.Add(item);
			Assert.That(subject.Count, Is.EqualTo(1));
			Assert.That(subject[index], Is.EqualTo(item));
		}

		[Test]
		public void Add_CanCancelAddition()
		{
			bool insertedRaised = false;

			var subject = new NotifyingList<int>();
			subject.Inserting += ((sender, e) => e.Cancel());
			subject.Inserted += (sender, args) => insertedRaised = true;

			subject.Add(3);

			Assert.That(insertedRaised, Is.False);
		}

		#endregion

		#region Clear

		[Test]
		public void Clear_EventsRaised()
		{
			bool clearingRaised = false, clearedRaised = false;

			var subject = new NotifyingList<int>();
			subject.Clearing += (sender, args) => clearingRaised = true;
			subject.Cleared += (sender, args) => clearedRaised = true;

			subject.Clear();
			Assert.That(clearingRaised, Is.True);
			Assert.That(clearedRaised, Is.True);
		}

		[Test]
		public void Clear_EventsRaisedWithCorrectArgumentsAndItemAdded()
		{
			var subject = new NotifyingList<int>();
			subject.Clearing += ((sender, e) => Assert.That(e.IsCancelled, Is.False));

			subject.Cleared += (sender, args) => { };

			subject.Clear();
			Assert.That(subject.Count, Is.EqualTo(0));
		}

		[Test]
		public void Clear_CanCancelClearance()
		{
			bool clearedRaised = false;

			var subject = new NotifyingList<int>();
			subject.Clearing += ((sender, e) => e.Cancel());

			subject.Cleared += (sender, args) => clearedRaised = true;

			subject.Clear();
			Assert.That(clearedRaised, Is.False);
		}

		#endregion

		#region Remove

		[Test]
		public void RemoveAt_OutOfBounds_ReturnsFalse()
		{
			var subject = new NotifyingList<int> { 3 };

			Assert.That(subject.Remove(2), Is.False);
		}

		[Test]
		public void Remove_EventsRaised()
		{
			bool removingRaised = false, removedRaised = false;

			var subject = new NotifyingList<int> { 3 };
			subject.Removing += (sender, args) => removingRaised = true;
			subject.Removed += (sender, args) => removedRaised = true;

			Assert.That(subject.Remove(3), Is.True);

			Assert.That(removingRaised, Is.True);
			Assert.That(removedRaised, Is.True);
		}

		[Test]
		public void Remove_EventsRaisedWithCorrectArgumentsAndItemRemoved()
		{
			int index = 0, item = 3;

			var subject = new NotifyingList<int> { 3 };

			subject.Removing += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
				Assert.That(e.IsCancelled, Is.False);
			};

			subject.Removed += (sender, e) =>
			{
				Assert.That(e.Index, Is.EqualTo(index));
				Assert.That(e.Value, Is.EqualTo(item));
			};

			subject.Remove(3);
			Assert.That(subject.Count, Is.EqualTo(0));
		}

		[Test]
		public void Remove_CanCancelDeletion()
		{
			bool removedRaised = false;

			var subject = new NotifyingList<int> { 3 };

			subject.Removing += ((sender, e) => e.Cancel());

			subject.Removed += (sender, args) => removedRaised = true;

			Assert.That(subject.Remove(3), Is.False);
			Assert.That(removedRaised, Is.False);
			Assert.That(subject[0], Is.EqualTo(3));
		}

		#endregion
	}
}