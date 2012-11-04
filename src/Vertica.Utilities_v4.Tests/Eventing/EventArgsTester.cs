using NUnit.Framework;
using Vertica.Utilities_v4.Eventing;

namespace Vertica.Utilities_v4.Tests.Eventing
{
	[TestFixture]
	public class EventArgsTester
	{
		[Test, Category("Exploratory")]
		public void Explore()
		{
			IValueEventArgs<int> value = new ValueEventArgs<int>(1);
			value = Args.Value(1);

			var multi = new MultiEventArgs<int, string>(1, "1");
			multi = Args.Value(1, "1");

			IMutableValueEventArgs<decimal> mutable = new MutableValueEventArgs<decimal> { Value = 1m };
			mutable = Args.Mutable(1m);

			var changed = new PropertyValueChangedEventArgs<int>("prop", 2, 3);
			changed = Args.Changed("prop", 2, 3);

			var indexed = new ValueIndexChangedEventArgs<string>(2, "old", "new");
			indexed = Args.Changed(2, "old", "new");

			var changing = new PropertyValueChangingEventArgs<int>("prop", 2, 3);
			changing = Args.Changing("prop", 2, 3);

			var indexing = new ValueIndexChangingEventArgs<string>(2, "old", "new");
			indexing = Args.Changing(2, "old", "new");

			ICancelEventArgs cancel = new ValueIndexCancelEventArgs<string>(2, "value");
			cancel = Args.Cancel(2, "value");

			IIndexEventArgs index = new ValueIndexEventArgs<string>(2, "value");
			index = Args.Index(2, "value");

		}
	}
}
