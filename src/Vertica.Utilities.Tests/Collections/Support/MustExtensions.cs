using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Vertica.Utilities.Collections;

namespace Vertica.Utilities.Tests.Collections.Support
{
	internal static partial class MustExtensions
	{
		public static SmartEntryConstraint<T> Entry<T>(this Must.BeEntryPoint entry, int index, T value, bool isFirst, bool isLast)
		{
			return new SmartEntryConstraint<T>(index, value, isFirst, isLast);
		}

		public static Constraint Model<T>(this Must.HaveEntryPoint entry, T model)
		{
			return Must.Have.Property<TreeNode<T>>(n => n.Model, Is.SameAs(model));
		}
	}
}