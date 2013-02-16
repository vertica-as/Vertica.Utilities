using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vertica.Utilities_v4.Security
{
	public class SimplePasswordGenerator
	{
		private static readonly Random _random;
		private static readonly Lazy<IEnumerable<char>> _digits, _letters, _special;
		static SimplePasswordGenerator()
		{
			_random = new Random(Guid.NewGuid().GetHashCode());
			_digits = new Lazy<IEnumerable<char>>(() => getRange('0', '9'));
			_letters = new Lazy<IEnumerable<char>>(() => getRange('a', 'z'));
			_special = new Lazy<IEnumerable<char>>(() =>
				getRange(':', '@')
				.Concat(getRange('[', ']'))
				.Concat(getRange('[', ']'))
				.Concat(getRange('{', '}'))
				.Concat(new[]{'_'}));
		}

		public string Generate(uint length, Charsets charsets)
		{
			var characters = new List<char>();

			if ((charsets & Charsets.Digits) == Charsets.Digits)
				characters.AddRange(_digits.Value);
			if ((charsets & Charsets.Letters) == Charsets.Letters)
				characters.AddRange(_letters.Value);
			if ((charsets & Charsets.SpecialCharacters) == Charsets.SpecialCharacters)
				characters.AddRange(_special.Value);

			var sb = new StringBuilder();

			for (int i = 1; i <= length; i++)
			{
				sb.Append(characters[_random.Next(0, characters.Count)]);
			}

			return sb.ToString();
		}
		
		private static IEnumerable<char> getRange(char start, char end)
		{
			if (start > end) throw new ArgumentException("start must be less than or equal to end.");

			for (var i = (byte)start; i <= (byte)end; i++)
			{
				yield return Convert.ToChar(i);
			}
		}
	}
}