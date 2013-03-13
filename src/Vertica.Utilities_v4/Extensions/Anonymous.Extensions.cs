using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vertica.Utilities_v4.Extensions.Infrastructure;

namespace Vertica.Utilities_v4.Extensions.AnonymousExt
{
	public static class AnonymousExtensions
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

		public static T AsAnonymous<T>(this IDictionary<string, object> dict, T anonymousPrototype) where T : class
		{
			// get the sole constructor
			var ctor = anonymousPrototype.GetType().GetConstructors().Single();
			
			Func<IDictionary<string, object>, string, object> getValueOrDefault = (d, key) =>
			{
				object val;
				return d.TryGetValue(key, out val) ? val : null;
			};

			// conveniently named constructor parameters make this all possible...
			var args = ctor.GetParameters()
				.Select(p => new {p, val = getValueOrDefault(dict, p.Name)})
				.Select(a => a.val != null && a.p.ParameterType.IsInstanceOfType(a.val) ?
					a.val : null);

			return (T)ctor.Invoke(args.ToArray());
		}
	}


}