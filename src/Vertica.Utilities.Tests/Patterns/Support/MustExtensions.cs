using NUnit.Framework.Constraints;
using Testing.Commons;

namespace Vertica.Utilities.Tests.Patterns.Support
{
	public static class MustExtensions
	{
		public static SpecificationConstraint<T> SatisfiedBy<T>(this Must.BeEntryPoint entry, T value)
		 {
			 return new SpecificationConstraint<T>(value, true);
		 }

		 public static SpecificationConstraint<T> SatisfiedBy<T>(this Must.NotBeEntryPoint entry, T value)
		 {
			 return new SpecificationConstraint<T>(value, false);
		 }
	}
}