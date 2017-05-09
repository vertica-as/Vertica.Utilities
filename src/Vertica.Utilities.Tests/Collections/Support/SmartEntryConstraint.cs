using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Vertica.Utilities.Collections;
using Testing.Commons.NUnit.Constraints;

namespace Vertica.Utilities.Tests.Collections.Support
{
	public class SmartEntryConstraint<T> : DelegatingConstraint
	{
		public SmartEntryConstraint(int index, T value, bool isFirst, bool isLast)
		{
			Delegate = Must.Have.Property<SmartEntry<T>>(e => e.Index, Is.EqualTo(index)) &
				Must.Have.Property<SmartEntry<T>>(e => e.Value, Is.EqualTo(value)) &
				Must.Have.Property<SmartEntry<T>>(e => e.IsFirst, Is.EqualTo(isFirst)) &
				Must.Have.Property<SmartEntry<T>>(e => e.IsLast, Is.EqualTo(isLast));
		}

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}