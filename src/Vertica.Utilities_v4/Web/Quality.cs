using System;
using System.Globalization;
using Vertica.Utilities_v4.Extensions.StringExt;

namespace Vertica.Utilities_v4.Web
{
	public struct Quality : IComparable<Quality>
	{
		// a quality is like a probability: between 0 and 1
		private static readonly Range<float> _zeroToOne = Range.Closed(0f, 1f);

		public Quality(float factor)
		{
			_zeroToOne.AssertArgument(nameof(factor), factor);
			Factor = factor;
		}

		public float Factor { get; }

		public int CompareTo(Quality other)
		{
			return Factor.CompareTo(other.Factor);
		}

		public static readonly string Qualifier = "q=";

		public override string ToString()
		{
			return Qualifier + Factor.ToString(CultureInfo.InvariantCulture);
		}

		public static readonly Quality Default = new Quality(1f);
		public static readonly Quality Min = new Quality(0f);

		public static Quality? TryParse(string q)
		{

			Quality? parsed = q.IsEmpty() ?
				default(Quality?) :
				parseNumberOrDefault(trimTokenizer(q.Strip(' ')));

			return parsed;
		}

		private static string trimTokenizer(string withoutSpaces)
		{
			return withoutSpaces.TrimStart(Qualifier.ToCharArray());
		}

		private static Quality? parseNumberOrDefault(string factor)
		{
			float quality;
			return parse(factor, out quality) && _zeroToOne.Contains(quality) ?
				new Quality(quality) :
				default(Quality?);
		}

		private static bool parse(string factor, out float quality)
		{
			return float.TryParse(factor, NumberStyles.Float, CultureInfo.InvariantCulture, out quality);
		}
	}
}