using System;

namespace Vertica.Utilities_v4.DirectoryServices
{
	[Obsolete(".NET Standard")] 

	public enum AdObjectCategory
	{
		organizationalUnit,
		group,
		user
	}

	[Obsolete(".NET Standard")]
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

	[Obsolete(".NET Standard")]
	public enum AdEntryMethod
	{
		Add,
		SetPassword
	}

	[Obsolete(".NET Standard")]
	public static class EnumExtensions
	{
		[Obsolete(".NET Standard")]
		public static string ToAd(this AdObjectCategory category)
		{
			return category.ToString();
		}

		[Obsolete(".NET Standard")]
		public static string ToAd(this AdEntryMethod method)
		{
			return method.ToString();
		}

		[Obsolete(".NET Standard")]
		public static string ToAd(this AdProperty property)
		{
			return property.ToString();
		}
	}
}