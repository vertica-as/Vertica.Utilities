using System;
using System.Linq;
using Vertica.Utilities_v4.Extensions.EnumerableExt;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Web
{
	public class Qualified : IComparable<Qualified>
	{
		public Qualified(string value, Quality quality)
		{
			if (value.IsEmpty()) throw new ArgumentException("Must not be empty", nameof(value));

			Value = value;
			Quality = quality;
		}

		public string Value { get; }
		public Quality Quality { get; }

		public override string ToString()
		{
			return Value + Tokenizer + Quality;
		}

		public int CompareTo(Qualified other)
		{
			return other == null ? 1 : Quality.CompareTo(other.Quality);
		}

		public static readonly char Tokenizer = ';';

		class Tokenizers
		{
			public static readonly string Str = ";";
			public static readonly char[] Arr = {';'};
		}
		

		public static Qualified TryParse(string value)
		{
			Qualified parsed = null;
			if (value.IsNotEmpty())
			{
				string[] q = value.Split(Tokenizers.Arr, StringSplitOptions.RemoveEmptyEntries);
				// try to get quality from last segment
				Quality? quality = Quality.TryParse(q[q.Length - 1]);
				if (quality.HasValue)
				{
					string range = q.Take(q.Length - 1).ToDelimitedString(Tokenizers.Str);
					parsed = new Qualified(range, quality.Value);
				}
				else
				{
					string range = string.Join(Tokenizers.Str, q);
					parsed = new Qualified(range, Quality.Default);
				}
			}

			return parsed;
		}
	}
}