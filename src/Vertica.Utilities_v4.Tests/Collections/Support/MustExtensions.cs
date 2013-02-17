using Testing.Commons;

namespace Vertica.Utilities_v4.Tests.Collections.Support
{
	internal static partial class MustExtensions
	{
		public static SmartEntryConstraint<T> Entry<T>(this Must.BeEntryPoint entry, int index, T value, bool isFirst, bool isLast)
		{
			return new SmartEntryConstraint<T>(index, value, isFirst, isLast);
		}
	}
}