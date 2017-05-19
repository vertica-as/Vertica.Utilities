using NUnit.Framework;
using Vertica.Utilities.Security;

namespace Vertica.Utilities.Tests.Security
{
	[TestFixture]
	public class SimplePasswordGeneratorTester
	{
		[Test]
		public void Generate_DigitsOnly_Verify()
		{
			var subject = new SimplePasswordGenerator();

			string actual = subject.Generate(10, Charsets.Digits);

			Assert.That(actual, Has.Length.EqualTo(10)
				.With.All.Matches<char>(char.IsNumber),
				"ten digits");
		}

		[Test]
		public void Generate_LettersOnly_Verify()
		{
			var subject = new SimplePasswordGenerator();

			string actual = subject.Generate(10, Charsets.Letters);

			Assert.That(actual, Has.Length.EqualTo(10)
				.With.All.Matches<char>(char.IsLetter),
				"ten letters");
		}

		[Test]
		public void Generate_AlphaNumericOnly_Verify()
		{
			var subject = new SimplePasswordGenerator();

			string actual = subject.Generate(10, Charsets.AlphaNumeric);

			Assert.That(actual, Has.Length.EqualTo(10)
				.With.All.Matches<char>(char.IsLetterOrDigit),
				"ten alphanumeric characters");
		}

		[Test]
		public void Generate_SpecialCharactersOnly_Verify()
		{
			var subject = new SimplePasswordGenerator();

			string actual = subject.Generate(10, Charsets.SpecialCharacters);

			Assert.That(actual, Has.Length.EqualTo(10)
				.With.None.Matches<char>(char.IsLetterOrDigit),
				"ten special characters");
		}

		[Test]
		public void Generate_AllCharacters_VerifyLength()
		{
			var subject = new SimplePasswordGenerator();

			string actual = subject.Generate(10, Charsets.All);

			Assert.That(actual, Has.Length.EqualTo(10));
		}
	}
}