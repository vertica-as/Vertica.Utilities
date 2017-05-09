using System;
using System.Linq;
using System.Reflection;

namespace Vertica.Utilities.Extensions.AttributeExt
{
	public static class AttributeExtensions
	{
		#region HasAttribute

		public static bool HasAttribute<T>(this object element, bool inherit = false) where T : Attribute
		{
			return element != null && 
				element.GetType().GetTypeInfo().IsDefined(typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this Assembly element) where T : Attribute
		{
			return element.IsDefined(typeof(T));
		}

		public static bool HasAttribute<T>(this MemberInfo element, bool inherit = false) where T : Attribute
		{
			return element.IsDefined(typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this Module element) where T : Attribute
		{
			return element.IsDefined(typeof(T));
		}

		public static bool HasAttribute<T>(this ParameterInfo element, bool inherit = false) where T : Attribute
		{
			return element.IsDefined(typeof(T), inherit);
		}

		#endregion

		#region GetAttribute

		public static T GetAttribute<T>(this object element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		public static T GetAttribute<T>(this Assembly element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		public static T GetAttribute<T>(this MemberInfo element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		public static T GetAttribute<T>(this Module element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		public static T GetAttribute<T>(this ParameterInfo element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		#endregion

		#region GetAttributes

		public static T[] GetAttributes<T>(this object element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttributes<T>().ToArray();
		}

		public static T[] GetAttributes<T>(this Assembly element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttributes<T>().ToArray();
		}

		public static T[] GetAttributes<T>(this MemberInfo element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttributes<T>().ToArray();
		}

		public static T[] GetAttributes<T>(this Module element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttributes<T>().ToArray();
		}

		public static T[] GetAttributes<T>(this ParameterInfo element, bool inherit = false) where T : Attribute
		{
			Guard.AgainstNullArgument(nameof(element), element);
			return element.GetType().GetTypeInfo().GetCustomAttributes<T>().ToArray();
		}

		#endregion
	}
}