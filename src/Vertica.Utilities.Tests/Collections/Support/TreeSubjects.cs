namespace Vertica.Utilities_v4.Tests.Collections.Support
{
	public class Category
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }
	}

	public class FamilyMember
	{
		public string Name { get; set; }
		public string ParentName { get; set; }
	}
}