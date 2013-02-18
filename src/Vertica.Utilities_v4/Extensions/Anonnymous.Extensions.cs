using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Extensions.AnonymousExt
{
	public static class AnonnymousExtensions
	{
		public static T ByExample<T>(this CastExtensionPoint<object> obj, T example) where T : class 
		{
			return (T)obj.ExtendedValue;
		}

		public static IEnumerable<Tuple<string, object>> AsTuples<T>(this T anonymousObject) where T : class
		{
			return anonymousAs(anonymousObject, Tuple.Create);
		}

		public static IEnumerable<KeyValuePair<string, object>> AsKeyValuePairs<T>(this T anonymousObject) where T : class
		{
			return anonymousAs(anonymousObject, (propName, val) => new KeyValuePair<string, object>(propName, val));
		}

		public static IDictionary<string, object> AsDictionary<T>(this T anonymousObject) where T : class
		{
			return AsKeyValuePairs(anonymousObject)
				.ToDictionary(p => p.Key, p => p.Value);
		}

		private static IEnumerable<TResult> anonymousAs<TAnonymous, TResult>(TAnonymous anonymous, Func<string, object, TResult> result) where TAnonymous : class
		{
			if (anonymous != null)
			{
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(anonymous);
				foreach (PropertyDescriptor prop in props)
				{
					object val = prop.GetValue(anonymous);
					if (val != null)
					{
						yield return result(prop.Name, val);
					}
				}
			}
		}
	}


}