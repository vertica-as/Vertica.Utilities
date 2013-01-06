using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class StaticClassMapperTester
	{
		#region single

		[Test]
		public void Map_SingleNotNull_Transformation()
		{
			string message = "message";
			var notNull = new InvalidOperationException(message);
			ArgumentException to = ClassMapper.MapIfNotNull(notNull,
				() => new ArgumentException(notNull.Message));

			Assert.That(to, Is.Not.Null.And.With.Message.EqualTo(message));
		}

		[Test]
		public void Map_SingleNotNullWithDefault_Transformation()
		{
			string message = "message";
			var notNull = new InvalidOperationException(message);
			var @default = new ArgumentException(string.Empty);

			ArgumentException to = ClassMapper.MapIfNotNull(notNull,
				() => new ArgumentException(notNull.Message),
				@default);

			Assert.That(to, Is.Not.Null.And.With.Message.EqualTo(message));
		}

		[Test]
		public void Map_NullSingle_Null()
		{
			InvalidOperationException @null = null;
			ArgumentException to = ClassMapper.MapIfNotNull(@null,
				() => new ArgumentException(@null.Message));

			Assert.That(to, Is.Null);
		}

		[Test]
		public void Map_NullSingleWithDefault_Default()
		{
			InvalidOperationException @null = null;
			string message = "message";
			var @default = new ArgumentException(message);

			ArgumentException to = ClassMapper.MapIfNotNull(@null,
				() => new ArgumentException(@null.Message),
				@default);

			Assert.That(to, Is.SameAs(@default));
		}

		#endregion

		#region collection

		[Test]
		public void Map_SeveralNotNull_Transformation()
		{
			var from = new[] { new InvalidOperationException("1"), new InvalidOperationException("2") };
			IEnumerable<ArgumentException> to = ClassMapper.MapIfNotNull(from,
				each => new ArgumentException(each.Message));

			Assert.That(to, Must.Be.Constrained(Has.Message.EqualTo("1"), Has.Message.EqualTo("2")));
		}

		[Test]
		public void Map_NullSeveral_Empty()
		{
			IEnumerable<InvalidOperationException> from = null;
			IEnumerable<ArgumentException> to = ClassMapper.MapIfNotNull(from,
				each => new ArgumentException(each.Message));

			Assert.That(to, Is.InstanceOf<IEnumerable<ArgumentException>>().And.Empty);
		}

		[Test]
		public void Map_EmptySeveral_Empty()
		{
			IEnumerable<InvalidOperationException> from = Enumerable.Empty<InvalidOperationException>();
			IEnumerable<ArgumentException> to = ClassMapper.MapIfNotNull(from,
				each => new ArgumentException(each.Message));

			Assert.That(to, Is.InstanceOf<IEnumerable<ArgumentException>>().And.Empty);
		}

		[Test]
		public void Map_SeveralWithNulls_NullsIgnored()
		{
			var from = new[] { new InvalidOperationException("1"), null, new InvalidOperationException("2") };
			IEnumerable<ArgumentException> to = ClassMapper.MapIfNotNull(from,
				each => new ArgumentException(each.Message));

			Assert.That(to, Must.Be.Constrained(Has.Message.EqualTo("1"), Has.Message.EqualTo("2")));
		}

		#endregion
	}
}
