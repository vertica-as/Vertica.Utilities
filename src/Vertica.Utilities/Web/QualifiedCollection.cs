using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vertica.Utilities.Extensions.EnumerableExt;
using Vertica.Utilities.Extensions.StringExt;

namespace Vertica.Utilities.Web
{
	public class QualifiedCollection : IEnumerable<Qualified>
	{
		private readonly IEnumerable<Qualified> _collection;

		public QualifiedCollection(IEnumerable<Qualified> collection) : this(collection, Comparer<Qualified>.Default) { }

		public QualifiedCollection(IEnumerable<Qualified> collection, IComparer<Qualified> customComparer)
		{
			_collection = collection.EmptyIfNull()
				.SkipNulls()
				.OrderByDescending(q => q, customComparer);
		}

		public IEnumerator<Qualified> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static QualifiedCollection TryParse(string headerValue)
		{
			return TryParse(headerValue, Comparer<Qualified>.Default);
		}

		public static QualifiedCollection TryParse(string headerValue, IComparer<Qualified> customComparer)
		{
			IEnumerable<string> splitted = headerValue.EmptyIfNull()
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.Trim());

			return new QualifiedCollection(splitted.Select(Qualified.TryParse), customComparer);
		}
	}
}