using System;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Linq;

namespace Vertica.Utilities_v4.DirectoryServices
{
	[Obsolete(".NET Standard")]
	public static class DirectoryEntryExtensions
	{
		[Obsolete(".NET Standard")]
		public static void SetPassword(this DirectoryEntry user, string password)
		{
			user.Invoke(AdEntryMethod.SetPassword.ToString(), new object[] { password });
			user.CommitChanges();
		}

		[Obsolete(".NET Standard")]
		public static T GetProperty<T>(this DirectoryEntry entry, AdProperty property)
		{
			return (T)entry.Properties[property.ToString()][0];
		}

		[Obsolete(".NET Standard")]
		public static T SetProperty<T>(this DirectoryEntry entry, AdProperty property, T value)
		{
			return (T)(entry.Properties[property.ToString()].Value = value);
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry AddOu(this DirectoryEntry container, string ouName)
		{
			DirectoryEntry ou = container.Children.Add("OU=" + ouName, AdObjectCategory.organizationalUnit.ToString());
			ou.CommitChanges();
			return ou;
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry AddGroup(this DirectoryEntry container, string groupName)
		{
			DirectoryEntry group = container.Children.Add("CN=" + groupName, AdObjectCategory.group.ToString());
			group.SetProperty(AdProperty.samAccountName, groupName);
			group.CommitChanges();
			return group;
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry AddUser(this DirectoryEntry container, string userName, string password, string displayName)
		{
			DirectoryEntry user = container.Children.Add("CN=" + userName, AdObjectCategory.user.ToString());
			user.CommitChanges();
			user.SetProperty(AdProperty.samAccountName, userName);
			user.SetProperty(AdProperty.displayName, displayName);
			user.CommitChanges();
			SetPassword(user, password);
			return user;
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry AddMember(this DirectoryEntry group, DirectoryEntry member)
		{
			group.Invoke(AdEntryMethod.Add.ToString(), new object[] { member.Path });
			group.CommitChanges();
			return group;
		}

		[Obsolete(".NET Standard")]
		public static DirectoryEntry EnableAccount(this DirectoryEntry user)
		{
			//UF_DONT_EXPIRE_PASSWD 0x10000
			int exp = (int)user.Properties[AdProperty.userAccountControl.ToString()].Value;
			user.Properties[AdProperty.userAccountControl.ToString()].Value = exp | 0x0001;
			user.CommitChanges();
			//UF_ACCOUNTDISABLE 0x0002
			int val = (int)user.Properties[AdProperty.userAccountControl.ToString()].Value;
			user.Properties[AdProperty.userAccountControl.ToString()].Value = val & ~0x0002;
			user.CommitChanges();
			return user;
		}

		[Obsolete(".NET Standard")]
		internal static void DelegateOuToGroup(this DirectoryEntry ou, string domainName, string groupName)
		{
			IdentityReference trustee = new NTAccount(domainName, groupName);
			var aceType = AccessControlType.Allow;
			Guid groupObjectType = new Guid("{BF967A9C-0DE6-11D0-A285-00AA003049E2}"),
			     ouObjectType = new Guid("{BF967AA5-0DE6-11D0-A285-00AA003049E2}"),
			     userObjectType = new Guid("{BF967ABA-0DE6-11D0-A285-00AA003049E2}");

			ActiveDirectoryRights createAndDeleteAccessMask = ActiveDirectoryRights.CreateChild | ActiveDirectoryRights.DeleteChild;
			var inheritAllFlags = ActiveDirectorySecurityInheritance.All;
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, createAndDeleteAccessMask, aceType, groupObjectType, inheritAllFlags));
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, createAndDeleteAccessMask, aceType, ouObjectType, inheritAllFlags));
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, createAndDeleteAccessMask, aceType, userObjectType, inheritAllFlags));

			var genericAccessMask = ActiveDirectoryRights.GenericAll;
			var inheritDescendetsFlags = ActiveDirectorySecurityInheritance.Descendents;
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, genericAccessMask, aceType, inheritDescendetsFlags, groupObjectType));
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, genericAccessMask, aceType, inheritDescendetsFlags, ouObjectType));
			ou.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(trustee, genericAccessMask, aceType, inheritDescendetsFlags, userObjectType));

			ou.CommitChanges();
		}
	}
}