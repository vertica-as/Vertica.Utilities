using System;
using NUnit.Framework;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections
{
	[TestFixture]
	public class PriorityQueueTester
	{
		[Test]
		public void Ctor_EmptyQueues()
		{
			var subject = new PriorityQueue<Priority, int>();
			Assert.That(subject.Count, Is.EqualTo(0));
		}

		[Test]
		public void Dequeue_EmptyQueue_Exception()
		{
			var subject = new PriorityQueue<Priority, int>();
			Assert.Throws<InvalidOperationException>(() => subject.Dequeue());
		}

		[TestCase(Priority.Low, -3)]
		[TestCase(Priority.Low, 5)]
		[TestCase(Priority.Normal, 1)]
		[TestCase(Priority.High, 1)]
		public void Dequeue_OneItem_ItemDequed(Priority priority, int value)
		{
			var subject = new PriorityQueue<Priority, int>();

			subject.Enqueue(priority, value);

			Assert.That(subject.Dequeue(), Is.EqualTo(value));
		}

		[Test]
		public void Dequeue_ItemsDequedByPriority()
		{
			var subject = new PriorityQueue<Priority, int>();
			int lowItem = 5, normalItem = -3, highItem = 0;

			subject.Enqueue(Priority.Normal, normalItem);
			subject.Enqueue(Priority.Low, lowItem);
			subject.Enqueue(Priority.High, highItem);

			Assert.That(subject.Dequeue(), Is.EqualTo(highItem));
			Assert.That(subject.Dequeue(), Is.EqualTo(normalItem));
			Assert.That(subject.Dequeue(), Is.EqualTo(lowItem));
		}

		[Test]
		public void Dequeue_CanEmptyQueue()
		{
			var subject = new PriorityQueue<Priority, int>();
			int lowItem = 5, normalItem = -3;

			subject.Enqueue(Priority.Normal, normalItem);
			subject.Enqueue(Priority.Low, lowItem);

			subject.Dequeue();
			subject.Dequeue();

			Assert.That(subject, Is.Empty);
		}

		[Test]
		public void Peek_ChosesHigherPriorityItem()
		{
			var subject = new PriorityQueue<Priority, int>();
			int lowItem = 5, normalItem = -3, highItem = 0;

			subject.Enqueue(Priority.Low, lowItem);
			Assert.That(subject.Peek(), Is.EqualTo(lowItem));

			subject.Enqueue(Priority.Normal, normalItem);
			Assert.That(subject.Peek(), Is.EqualTo(normalItem));

			subject.Enqueue(Priority.High, highItem);
			Assert.That(subject.Peek(), Is.EqualTo(highItem));

			subject.Dequeue();
			Assert.That(subject.Peek(), Is.EqualTo(normalItem));
		}

		[Test]
		public void Peek_IdempotentOperation()
		{
			var subject = new PriorityQueue<Priority, int>();
			int lowItem = 5;

			subject.Enqueue(Priority.Low, lowItem);
			int peeked1 = subject.Peek(), peeked2 = subject.Peek();
			Assert.That(peeked1, Is.EqualTo(peeked2));
		}

		[Test]
		public void Count_IncreasedByEnqueue()
		{
			var subject = new PriorityQueue<Priority, string>();
			Assert.That(subject.Count, Is.EqualTo(0));
			subject.Enqueue(Priority.Low, "s");
			Assert.That(subject.Count, Is.EqualTo(1));
			subject.Enqueue(Priority.High, "s");
			Assert.That(subject.Count, Is.EqualTo(2));
		}

		[Test]
		public void Count_DecreasedByDequeue()
		{
			var subject = new PriorityQueue<Priority, string>();
			Assert.That(subject.Count, Is.EqualTo(0));
			subject.Enqueue(Priority.Low, "s");
			subject.Enqueue(Priority.High, "s");
			Assert.That(subject.Count, Is.EqualTo(2));
			subject.Dequeue();
			Assert.That(subject.Count, Is.EqualTo(1));
			subject.Dequeue();
			Assert.That(subject.Count, Is.EqualTo(0));
		}

		[Test]
		public void Count_InvariantWithPeek()
		{
			var subject = new PriorityQueue<Priority, string>();
			Assert.That(subject.Count, Is.EqualTo(0));
			subject.Enqueue(Priority.Low, "s");
			subject.Enqueue(Priority.High, "s");

			Assert.That(subject.Count, Is.EqualTo(2));
			subject.Peek();
			Assert.That(subject.Count, Is.EqualTo(2));
			subject.Peek();
			Assert.That(subject.Count, Is.EqualTo(2));
		}

		[Test]
		public void GetEnumerator_HonorsPriority()
		{
			int lowItem = 1, normalItem = 2, highItem1 = 4, highItem2 = 3;
			var subject = new PriorityQueue<Priority, int>();

			subject.Enqueue(Priority.Low, lowItem);
			subject.Enqueue(Priority.Normal, normalItem);
			subject.Enqueue(Priority.High, highItem1);
			subject.Enqueue(Priority.High, highItem2);

			Assert.That(subject, Is.EqualTo(new[]
			{
				highItem1, highItem2, normalItem, lowItem
			}));
		}
	}
}