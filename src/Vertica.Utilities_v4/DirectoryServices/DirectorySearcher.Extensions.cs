using System;
using System.DirectoryServices;
using System.Linq;
using Vertica.Utilities_v4.Extensions.EnumerableExt;

namespace Vertica.Utilities_v4.DirectoryServices
{
	[Obsolete(".NET Standard")]
	public static class DirectorySearcherExtensions
	{
		private static readonly TimeSpan twoSeconds = TimeSpan.FromSeconds(2);

		[Obsolete(".NET Standard")]
		public static DirectoryEntry GetOu(this DirectorySearcher ds, string ouName)
		{
			string ouFilter = "(&(objectCategory=organizationalUnit)(name={0}))";
			ds.SearchScope = SearchScope.OneLevel;
			ds.PropertiesToLoad.AddRange(new[] { AdProperty.ou.ToString(), AdProperty.name.ToString(),AdProperty.displayName.ToString()});
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, ouName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry FindOu(this DirectorySearcher ds, string ouName)
		{
			string ouFilter = "(&(objectCategory=organizationalUnit)(name={0}))";
			ds.SearchScope = SearchScope.Subtree;
			ds.PropertiesToLoad.AddRange(new[] { AdProperty.ou.ToString(), AdProperty.name.ToString(), AdProperty.displayName.ToString() });
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, ouName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry GetGroup(this DirectorySearcher ds, string groupName)
		{
			string ouFilter = "(&(objectCategory=group)(name={0}))";
			ds.SearchScope = SearchScope.OneLevel;
			ds.PropertiesToLoad.Add(AdProperty.name.ToString());
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, groupName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry FindGroup(this DirectorySearcher ds, string groupName)
		{
			string ouFilter = "(&(objectCategory=group)(name={0}))";
			ds.SearchScope = SearchScope.Subtree;
			ds.PropertiesToLoad.Add(AdProperty.name.ToString());
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, groupName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry GetUser(this DirectorySearcher ds, string samAccountName)
		{
			string ouFilter = "(&(objectCategory=user)(samAccountName={0}))";
			ds.SearchScope = SearchScope.OneLevel;
			ds.PropertiesToLoad.Add(AdProperty.samAccountName.ToString());
			ds.PropertiesToLoad.Add(AdProperty.displayName.ToString());
			ds.PropertiesToLoad.Add(AdProperty.mail.ToString());
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, samAccountName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry FindUser(this DirectorySearcher ds, string samAccountName)
		{
			string ouFilter = "(&(objectCategory=user)(samAccountName={0}))";
			ds.SearchScope = SearchScope.Subtree;
			ds.PropertiesToLoad.Add(AdProperty.samAccountName.ToString());
			ds.PageSize = 1;
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, samAccountName);

			SearchResult sr = ds.FindOne();
			return sr == null ? null : sr.GetDirectoryEntry();
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntryCollection GetAllOf(this DirectorySearcher ds, AdObjectCategory category)
		{
			string ouFilter = "(objectCategory={0})";
			ds.SearchScope = SearchScope.OneLevel;
			ds.PropertiesToLoad.AddRange(new [] { AdProperty.name.ToString(), AdProperty.displayName.ToString()});
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, category);
			SearchResultCollection results = ds.FindAll();
			var entries = new DirectoryEntryCollection(results.Count);

			results.Cast<SearchResult>().ForEach(result => entries.Add(result.GetDirectoryEntry()));
			return entries;
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntryCollection FindAllOf(this DirectorySearcher ds, AdObjectCategory category)
		{
			string ouFilter = "(objectCategory={0})";
			ds.SearchScope = SearchScope.Subtree;
			ds.PropertiesToLoad.Add(AdProperty.name.ToString());
			ds.ServerPageTimeLimit = twoSeconds;
			ds.Filter = string.Format(ouFilter, category);
			SearchResultCollection results = ds.FindAll();
			var entries = new DirectoryEntryCollection(results.Count);

			results.Cast<SearchResult>().ForEach(result => entries.Add(result.GetDirectoryEntry()));
			return entries;
		}
	}
}