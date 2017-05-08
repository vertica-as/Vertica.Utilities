using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Collections;
using Testing.Commons.NUnit.Constraints;

namespace Vertica.Utilities_v4.Tests.Collections.Support
{
	public class SmartEntryConstraint<T> : DelegatingConstraint<SmartEntry<T>>
	{
		public SmartEntryConstraint(int index, T value, bool isFirst, bool isLast)
		{
			Delegate = Must.Have.Property<SmartEntry<T>>(e => e.Index, Is.EqualTo(index)) &
				Must.Have.Property<SmartEntry<T>>(e => e.Value, Is.EqualTo(value)) &
				Must.Have.Property<SmartEntry<T>>(e => e.IsFirst, Is.EqualTo(isFirst)) &
				Must.Have.Property<SmartEntry<T>>(e => e.IsLast, Is.EqualTo(isLast));
		}

		protected override bool matches(SmartEntry<T> current)
		{
			return Delegate.Matches(current);
		}
	}
}