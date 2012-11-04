using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	// ReSharper disable ConditionIsAlwaysTrueOrFalse
	[TestFixture]
	public class GuardTester
	{
		[Test]
		public void Against_FalseCondition_NoException()
		{
			Assert.That(() => Guard.Against(false, "no Exception"), Throws.Nothing);
			Assert.That(() => Guard.Against(false), Throws.Nothing);
		}

		[Test]
		public void Against_TrueCondition_ExceptionWithDefaultMessage()
		{
			string defaultMessage = new InvalidOperationException().Message;
			Assert.That(() => Guard.Against(3 > 2), Throws.InstanceOf<InvalidOperationException>()
				.With.Message.EqualTo(defaultMessage));
		}

		[Test]
		public void Against_TrueConditionNotTemplated_ExceptionWithTemplate()
		{
			string notTemplated = "message";
			Assert.That(() => Guard.Against(true, notTemplated), Throws.InstanceOf<InvalidOperationException>()
				.With.Message.EqualTo(notTemplated));
		}

		[Test]
		public void Against_TrueConditionWithNoArgumentsForTemplate_FormatException()
		{
			string template = "message{0}";
			Assert.That(() => Guard.Against(true, template), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Against_TrueConditionWithArgumentsForTemplate_FormattedMessage()
		{
			string template = "message {0}", argument = "arg";
			Assert.That(() => Guard.Against(true, template, argument), Throws.InstanceOf<InvalidOperationException>()
				.With.Message.EqualTo("message arg"));
		}

		[Test]
		public void GenericAgainst_FalseCondition_NoException()
		{
			Assert.That(() => Guard.Against<NotSupportedException>(false, "no Exception"), Throws.Nothing);
			Assert.That(() => Guard.Against<NotSupportedException>(false), Throws.Nothing);
		}

		[Test]
		public void GenericAgainst_TrueCondition_DefaultMessage()
		{
			string defaultMessage = new NotSupportedException().Message;
			var ex = Assert.Throws<NotSupportedException>(() => Guard.Against<NotSupportedException>(3 > 2));
			Assert.That(ex.Message, Is.EqualTo(defaultMessage));
		}

		[Test]
		public void GenericAgainst_TrueConditionNotTemplated_FormattedMessage()
		{
			string notTemplated = "message";
			Assert.That(() => Guard.Against<NotSupportedException>(true, notTemplated), Throws.InstanceOf<NotSupportedException>()
				.With.Message.EqualTo(notTemplated));
		}

		[Test]
		public void GenericAgainst_TrueConditionWithNoArgumentsForTemplate_FormatException()
		{
			string template = "message {0}";
			Assert.That(() => Guard.Against<NotSupportedException>(true, template), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void GenericAgainst_TrueConditionWithArgumentsForTemplate_FormattedMessage()
		{
			string template = "message {0}", argument = "arg";
			Assert.That(() => Guard.Against<NotSupportedException>(true, template, argument), 
				Throws.InstanceOf<NotSupportedException>().With.Message.EqualTo("message arg"));
		}

		[Test]
		public void GenericAgainst_TrueConditionArgumentException_ParamNameIsWrong()
		{
			string template = "message {0}", argument = "arg";
			var ex = Assert.Throws<ArgumentNullException>(() => Guard.Against<ArgumentNullException>(true, template, argument));

			Assert.That(ex.ParamName, Is.EqualTo("message arg"));
		}

		[Test]
		public void AgainstNullArgument_FalseCondition_NoException()
		{
			string notNull = "notNull";
			Assert.That(() => Guard.AgainstNullArgument("param", notNull), Throws.Nothing);
			Assert.That(() => new GuardSubject().Method("notNull"), Throws.Nothing);
		}

		// ReSharper disable ExpressionIsAlwaysNull
		[Test]
		public void AgainstNullArgument_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = null, paramName = "param", actualMessage = new ArgumentNullException(paramName).Message;
			var ex = Assert.Throws<ArgumentNullException>(

				() => Guard.AgainstNullArgument(paramName, param));
			Assert.That(ex.Message, Is.EqualTo(actualMessage).IgnoreCase);
			Assert.That(ex.ParamName, Is.EqualTo(paramName));

			ex = Assert.Throws<ArgumentNullException>(() => new GuardSubject().Method(null));
			Assert.That(ex.Message, Is.EqualTo(actualMessage).IgnoreCase);
			Assert.That(ex.ParamName, Is.EqualTo(paramName));
		}
		// ReSharper restore ExpressionIsAlwaysNull

		[Test]
		public void AgainstArgument_FalseCondition_NoException()
		{
			Assert.DoesNotThrow(() => Guard.AgainstArgument("param", "asd" == null, "no Exception"));
		}

		[Test]
		public void AgainstArgument_TrueConditionNoFormatMessage_ExceptionWithMessageAndParam()
		{
			string message = "message", param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentException>(
				() => Guard.AgainstArgument(param, trueCondition, message));
			StringAssert.StartsWith(message, ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		[Test]
		public void AgainstArgument_TrueConditionFormatMessageNoArguments_ExceptionMustProvideArguments()
		{
			string message = "message{0}";
			bool trueCondition = 3 > 2;
			Assert.Throws<FormatException>(() => Guard.AgainstArgument(string.Empty, trueCondition, message));
		}

		[Test]
		public void AgainstArgument_TrueConditionFormatMessageArguments_FormattedMessageException()
		{
			string message = "message{0}", argument = "argument", param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentException>(
				() => Guard.AgainstArgument(param, trueCondition, message, argument));
			StringAssert.StartsWith(string.Format(message, argument), ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		[Test]
		public void AgainstArgumentNoMessage_FalseCondition_NoException()
		{
			Assert.DoesNotThrow(() => Guard.AgainstArgument("param", "asd" == null));
		}

		[Test]
		public void AgainstArgumentNoMessage_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentException>(
				() => Guard.AgainstArgument(param, trueCondition));
			StringAssert.AreEqualIgnoringCase(new ArgumentException(string.Empty, param).Message, ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgument_FalseCondition_NoException()
		{
			Assert.DoesNotThrow(() => Guard.AgainstArgument<ArgumentNullException>("param", "asd" == null, "no Exception"));
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionNoFormatMessage_ExceptionWithMessageAndParam()
		{
			string message = "message", param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentNullException>(
				() => Guard.AgainstArgument<ArgumentNullException>(param, trueCondition, message));
			StringAssert.StartsWith(message, ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionFormatMessageNoArguments_ExceptionMustProvideArguments()
		{
			string message = "message{0}";
			bool trueCondition = 3 > 2;
			Assert.Throws<FormatException>(
				() => Guard.AgainstArgument<ArgumentException>(string.Empty, trueCondition, message));
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionFormatMessageArguments_FormattedMessageException()
		{
			string message = "message{0}", argument = "argument", param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Guard.AgainstArgument<ArgumentOutOfRangeException>(param, trueCondition, message, argument));
			StringAssert.StartsWith(string.Format(message, argument), ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgumentNoMessage_FalseCondition_NoException()
		{
			Assert.DoesNotThrow(() => Guard.AgainstArgument<ArgumentNullException>("param", "asd" == null));
		}

		[Test]
		public void GenericAgainstArgumentNoMessage_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = "param";
			bool trueCondition = 3 > 2;
			var ex = Assert.Throws<ArgumentNullException>(

				() => Guard.AgainstArgument<ArgumentNullException>(param, trueCondition));
			StringAssert.AreEqualIgnoringCase(new ArgumentNullException(param).Message, ex.Message);
			Assert.That(ex.ParamName, Is.EqualTo(param));
		}

		internal class GuardSubject
		{
			public void Method(string param)
			{
				Guard.AgainstNullArgument("param", param);
			}
		}
	}
	// ReSharper restore ConditionIsAlwaysTrueOrFalse
}
