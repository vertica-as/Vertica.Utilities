using System;

namespace Vertica.Utilities.Security
{
	public interface IPasswordGenerator
	{
		string Generate(int length, Charsets charsets);
	}

	[Flags]
	public enum Charsets
	{
		Digits = 1,
		Letters = 2,
		AlphaNumeric = 3,
		SpecialCharacters = 4,
		All = 7
	}
}