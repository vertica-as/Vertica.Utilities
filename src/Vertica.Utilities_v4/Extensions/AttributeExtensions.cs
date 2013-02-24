using System;
using System.Reflection;

namespace Vertica.Utilities_v4.Extensions.AttributeExt
{
	public static class AttributeExtensions
	{
		#region HasAttribute

		public static bool HasAttribute<T>(this object element, bool inherit = false) where T : Attribute
		{
			return element != null && Attribute.IsDefined(element.GetType(), typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this Assembly element, bool inherit = false) where T : Attribute
		{
			return Attribute.IsDefined(element, typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this MemberInfo element, bool inherit = false) where T : Attribute
		{
			return Attribute.IsDefined(element, typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this Module element, bool inherit = false) where T : Attribute
		{
			return Attribute.IsDefined(element, typeof(T), inherit);
		}

		public static bool HasAttribute<T>(this ParameterInfo element, bool inherit = false) where T : Attribute
		{
			return Attribute.IsDefined(element, typeof(T), inherit);
		}

		#endregion
	}
}