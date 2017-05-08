using System;
using System.Configuration;
using NUnit.Framework;
using Vertica.Utilities_v4.Configuration;
using Vertica.Utilities_v4.Tests.Configuration.Support;

namespace Vertica.Utilities_v4.Tests.Configuration
{
	[TestFixture]
	public class CollectionCountValidatorTester
	{
		[Test]
		public void CanValidate_TrueWhenSublassOfCollection()
		{
			var subject = new CollectionCountValidator(uint.MinValue);
			Assert.That(subject.CanValidate(typeof(CollectionCountSubject)), Is.True);
			Assert.That(subject.CanValidate(typeof(CollectionCountElementSubject)), Is.False);
		}

		[Test]
		public void Validate_NotCollection_Exception()
		{
			var subject = new CollectionCountValidator(uint.MinValue);
			Assert.That(() => subject.Validate(new CollectionCountElementSubject()),
				Throws.InstanceOf<InvalidCastException>());
		}

		[Test]
		public void Validate_MinimumMet_NoException()
		{
			var subject = new CollectionCountValidator(2);
			// ReSharper disable AccessToModifiedClosure
			Assert.That(() => subject.Validate(CollectionCountSubject.Build(1, 2)), Throws.Nothing);

			Assert.That(() => subject.Validate(CollectionCountSubject.Build(1, 2, 3)), Throws.Nothing);

			subject = new CollectionCountValidator(1);
			Assert.That(() => subject.Validate(CollectionCountSubject.Build(1)), Throws.Nothing);

			subject = new CollectionCountValidator(0);
			Assert.That(() => subject.Validate(CollectionCountSubject.Build()), Throws.Nothing);
			// ReSharper restore AccessToModifiedClosure
		}

		[Test]
		public void Validate_MinimumNotMet_Exception()
		{
			var subject = new CollectionCountValidator(2);

			Assert.That(() => subject.Validate(CollectionCountSubject.Build(1)), Throws.InstanceOf<ConfigurationErrorsException>()
				.With.Message.StringContaining(typeof(CollectionCountSubject).Name).And
				.Message.StringContaining("1").And
				.Message.StringContaining("2"));
		}
	}
}