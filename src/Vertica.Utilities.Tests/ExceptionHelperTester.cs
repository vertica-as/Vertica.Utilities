using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class ExceptionHelperTester
	{
		[Test]
		public void Throw_Message_TypeAndMessageAsEntered()
		{
			Assert.That(() => ExceptionHelper.Throw<InvalidCastException>("template {0}", "value"), 
				Throws.InstanceOf<InvalidCastException>()
				.With.Message.EqualTo("template value"));
		}

		[Test]
		public void Throw_MessageWithNoPlaceHolders_NoSubstitutions()
		{
			string noPlaceholders = "template";
			Assert.That(() => ExceptionHelper.Throw<InvalidCastException>(noPlaceholders),
				Throws.InstanceOf<InvalidCastException>()
				.With.Message.EqualTo(noPlaceholders));
		}

		[Test]
		public void Throw_MessageWithNotEnoughFormatArguments_FormatException()
		{
			string twoPlaceholders = "template {0} {1}";
			Assert.That(() => ExceptionHelper.Throw<InvalidCastException>(twoPlaceholders, "one"),
				Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Throw_MessageWithTooManyFormatArguments_OnlyPlaceholdersSubsituted()
		{
			string onePlaceholder = "template {0}";
			Assert.That(() => ExceptionHelper.Throw<InvalidCastException>(onePlaceholder, "one", "two"),
				Throws.InstanceOf<InvalidCastException>());
		}

		[Test]
		public void Throw_MessageAndInnerException_MessageAndInnerSet()
		{
			string message = "template value", innerMessage = "inner message";
			var inner = new NullReferenceException(innerMessage); ;

			Assert.That(() => ExceptionHelper.Throw<ArgumentException>(inner, message),
				Throws.InstanceOf<ArgumentException>()
				.With.Message.EqualTo(message).And
				.InnerException.InstanceOf<NullReferenceException>().And
				.InnerException.With.Message.EqualTo(innerMessage));
		}

		[Test]
		public void ThrowArgumentException_Templated_ParamAndFormatterMessageSet()
		{
			string template = "template {0}", argument = "arg", param = "param";
			var ex = Assert.Throws<ArgumentException>(() => ExceptionHelper.ThrowArgumentException(param, template, argument));

			Assert.That(ex.Message, Is.StringStarting("template arg").And.StringContaining(param));
			Assert.That(ex.ParamName, Is.EqualTo(param));
			Assert.That(ex.InnerException, Is.Null);
		}

		[Test]
		public void ThrowArgumentException_NonPlaceholders_ArgumentsIgnored()
		{
			string template = "template", param = "param";
			var ex = Assert.Throws<ArgumentException>(() => ExceptionHelper.ThrowArgumentException(param, "template", "arg"));

			Assert.That(ex.Message, Is.StringStarting(template).And.StringContaining(param));
			Assert.That(ex.ParamName, Is.EqualTo(param));
			Assert.That(ex.InnerException, Is.Null);
		}

		[Test]
		public void ThrowArgumentException_NotEnoughFormatArguments_FormatException()
		{
			Assert.That(() => ExceptionHelper.ThrowArgumentException("param", "template {0} {1}", "value1"),
				Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void ThrowArgumentException_NoMessage_CorrectParamAndDefaultMessage()
		{
			string param = "param", defaultMessage = new ArgumentException(string.Empty, param).Message;
			var ex = Assert.Throws<ArgumentException>(() => ExceptionHelper.ThrowArgumentException(param));

			Assert.That(ex.Message, Is.EqualTo(defaultMessage));
			Assert.That(ex.ParamName, Is.EqualTo(param));
			Assert.That(ex.InnerException, Is.Null);
		}

		[Test]
		public void GenericThrowArgumentException_Templated_CorrectParamAndMessageSubstituted()
		{
			string template = "template {0}", argument = "arg",  param = "param";
			var ex = Assert.Throws<ArgumentNullException>(
				() => ExceptionHelper.ThrowArgumentException<ArgumentNullException>(param, template, argument));

			Assert.That(ex.Message, Is.StringStarting("template arg").And.StringContaining(param));
			Assert.That(ex.ParamName, Is.EqualTo(param));
			Assert.That(ex.InnerException, Is.Null);
		}

		[Test]
		public void GenericThrowArgumentException_NotTemplated_IgnoresParams()
		{
			string template = "template", param = "param";
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => ExceptionHelper.ThrowArgumentException<ArgumentOutOfRangeException>(param, "template", "arg"));

			Assert.That(ex.Message, Is.StringStarting(template).And.StringContaining(param));
			Assert.That(ex.ParamName, Is.EqualTo(param));
			Assert.That(ex.InnerException, Is.Null);
		}

		[Test]
		public void GenericThrowArgumentException_NotEnoughFormatArguments_FormatException()
		{
			Assert.Throws<FormatException>(
				() => ExceptionHelper.ThrowArgumentException<ArgumentNullException>("param", "template {0} {1}", "value1"));
		}

		[Test]
		public void ThrowArgumentException_NoMessage_DefaultExceptionMessage()
		{
			string param = "param", defaultMessage = new ArgumentOutOfRangeException(param).Message;
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => ExceptionHelper.ThrowArgumentException<ArgumentOutOfRangeException>(param));

			Assert.That(ex.Message, Is.EqualTo(defaultMessage));
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}
	}
}
