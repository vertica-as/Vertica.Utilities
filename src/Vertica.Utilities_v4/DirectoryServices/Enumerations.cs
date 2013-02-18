namespace Vertica.Utilities_v4.DirectoryServices
{
	public enum AdObjectCategory
	{
		organizationalUnit,
		group,
		user
	}

	public enum AdProperty
	{
		name,
		userAccountControl,
		samAccountName,
		dc,
		displayName,
		mail,
		ou
	}

	public enum AdEntryMethod
	{
		Add,
		SetPassword
	}

	public static class EnumExtensions
	{
		public static string ToAd(this AdObjectCategory category)
		{
			return category.ToString();
		}
		public static string ToAd(this AdEntryMethod method)
		{
			return method.ToString();
		}
		public static string ToAd(this AdProperty property)
		{
			return property.ToString();
		}
	}
}