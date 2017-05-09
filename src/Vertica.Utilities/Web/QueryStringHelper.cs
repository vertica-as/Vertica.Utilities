using System;
using System.Linq;
using Vertica.Utilities.Collections;
using System.Net;
using System.Text;

namespace Vertica.Utilities.Web
{
	public static class QueryStringHelper
	{
		private const string EQ = "=", AMP = "&", Q = "?";

		static class ValueEncoding
		{
			internal static readonly Func<string, string> None = _ => _;
			internal static readonly Func<string, string> Url = str => WebUtility.UrlEncode(str);
		}
		static class Prepend
		{
			internal static readonly Action<StringBuilder> Nothing = _ => { };
			internal static readonly Action<StringBuilder> Q = sb => sb.Insert(0, QueryStringHelper.Q);
		}

		public static string ToQueryString(this MutableLookup<string, string> collection)
		{
			return buildString(collection,
				ValueEncoding.Url,
				Prepend.Q);
		}

		public static string ToQuery(this MutableLookup<string, string> collection)
		{
			return buildString(collection,
				ValueEncoding.Url,
				Prepend.Nothing);
		}

		public static string ToDecodedQueryString(this MutableLookup<string, string> collection)
		{
			return buildString(collection,
				ValueEncoding.None,
				Prepend.Q);
		}

		public static string ToDecodedQuery(this MutableLookup<string, string> collection)
		{
			return buildString(collection,
				ValueEncoding.None,
				Prepend.Nothing);
		}

		private static string buildString(MutableLookup<string, string> collection,
			Func<string, string> valueEncoding,
			Action<StringBuilder> prependAction)
		{
			Guard.AgainstNullArgument("collection", collection);

			var sb = new StringBuilder();
			foreach (string key in collection.Keys)
			{
			    if (key != null)
			    {
                    string[] values = collection[key].ToArray();

                    if (values != null)
                    {
                        foreach (var value in values)
                        {
                            sb.Append(valueEncoding(key));
                            sb.Append(EQ);
                            sb.Append(valueEncoding(value));
                            sb.Append(AMP);
                        }
                    }
                    else
                    {
                        sb.Append(valueEncoding(key));
                        sb.Append(EQ);
                        sb.Append(AMP);
                    }
                }
			}
			// maybe insert first ? and remove last &
			if (sb.Length > 0)
			{
				prependAction(sb);
				sb.Remove(sb.Length - 1, 1);
			}
			return sb.ToString();
		}
	}
}
