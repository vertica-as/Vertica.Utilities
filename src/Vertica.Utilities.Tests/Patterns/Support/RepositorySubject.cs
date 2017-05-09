using System.Globalization;
using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns.Support
{
	internal class RepositorySubject : IIdentifiable<int>
	{
		public RepositorySubject(int id) : this(id, id.ToString(CultureInfo.InvariantCulture)) { }
		public RepositorySubject(int id, string property)
		{
			Id = id;
			Property = property;
		}

		public int Id { get; set; }
		public string Property { get; private set; }

		public static implicit operator RepositorySubject(int i)
		{
			return new RepositorySubject(i);
		}
	}
}
