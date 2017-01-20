using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Vertica.Utilities_v4.Web
{
	public static class QueryStringHelper
	{
		private const string EQ = "=", AMP = "&", Q = "?";

		static class ValueEncoding
		{
			internal static readonly Func<string, string> None = _ => _;
			internal static readonly Func<string, string> Url = str => HttpUtility.UrlEncode(str);
		}
		static class Prepend
		{
			internal static readonly Action<StringBuilder> Nothing = _ => { };
			internal static readonly Action<StringBuilder> Q = sb => sb.Insert(0, QueryStringHelper.Q);
		}

		public static string ToQueryString(this NameValueCollection collection)
		{
			return buildString(collection,
				ValueEncoding.Url,
				Prepend.Q);
		}

		public static string ToQuery(this NameValueCollection collection)
		{
			return buildString(collection,
				ValueEncoding.Url,
				Prepend.Nothing);
		}

		public static string ToDecodedQueryString(this NameValueCollection collection)
		{
			return buildString(collection,
				ValueEncoding.None,
				Prepend.Q);
		}

		public static string ToDecodedQuery(this NameValueCollection collection)
		{
			return buildString(collection,
				ValueEncoding.None,
				Prepend.Nothing);
		}

		private static string buildString(NameValueCollection collection,
			Func<string, string> valueEncoding,
			Action<StringBuilder> prependAction)
		{
			Guard.AgainstNullArgument("collection", collection);

			var sb = new StringBuilder();
			foreach (string key in collection.Keys)
			{
			    if (key != null)
			    {
                    string[] values = collection.GetValues(key);

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

		public static NameValueCollection QueryString(this Uri uri)
		{
			string queryText = HttpUtility.UrlDecode(uri.Query);
			return HttpUtility.ParseQueryString(queryText);
		}
	}
}
