using System;
using System.IO;
using NUnit.Framework;

namespace Vertica.Utilities.Tests
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

			Assert.That(() => Guard.Against<NotSupportedException>(3 > 2), Throws.InstanceOf<NotSupportedException>()
				.With.Message.EqualTo(defaultMessage));
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

			Assert.That(() => Guard.Against<ArgumentNullException>(true, template, argument),
				Throws.InstanceOf<ArgumentNullException>()
					.With.Property("ParamName").EqualTo("message arg"));
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

			Assert.That(() => Guard.AgainstNullArgument(paramName, param), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.EqualTo(actualMessage).And
				.With.Property("ParamName").EqualTo(paramName));

			Assert.That(() => new GuardSubject().Method(null), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.EqualTo(actualMessage).And
				.With.Property("ParamName").EqualTo(paramName));
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

			Assert.That(() => Guard.AgainstArgument(param, trueCondition, message), Throws.ArgumentException
				.With.Message.StartWith(message).And
				.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void AgainstArgument_TrueConditionFormatMessageNoArguments_ExceptionMustProvideArguments()
		{
			string message = "message{0}";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument(string.Empty, trueCondition, message),
				Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void AgainstArgument_TrueConditionFormatMessageArguments_FormattedMessageException()
		{
			string message = "message{0}", argument = "argument", param = "param";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument(param, trueCondition, message, argument), Throws.ArgumentException
				.With.Message.StartsWith(string.Format(message, argument)).And
				.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void AgainstArgumentNoMessage_FalseCondition_NoException()
		{
			Assert.That(() => Guard.AgainstArgument("param", "asd" == null), Throws.Nothing);
		}

		[Test]
		public void AgainstArgumentNoMessage_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = "param";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument(param, trueCondition), Throws.ArgumentException
				.With.Message.EqualTo(new ArgumentException(string.Empty, param).Message).And
				.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgument_FalseCondition_NoException()
		{
			Assert.That(() => Guard.AgainstArgument<ArgumentNullException>("param", "asd" == null, "no Exception"),
				Throws.Nothing);
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionNoFormatMessage_ExceptionWithMessageAndParam()
		{
			string message = "message", param = "param";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument<ArgumentNullException>(param, trueCondition, message),
				Throws.InstanceOf<ArgumentNullException>().With.Message.StartWith(message).And
				.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionFormatMessageNoArguments_ExceptionMustProvideArguments()
		{
			string message = "message{0}";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument<ArgumentException>(string.Empty, trueCondition, message),
				Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void GenericAgainstArgument_TrueConditionFormatMessageArguments_FormattedMessageException()
		{
			string message = "message{0}", argument = "argument", param = "param";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument<ArgumentOutOfRangeException>(param, trueCondition, message, argument),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
					.With.Message.StartWith(string.Format(message, argument)).And
					.With.Property("ParamName").EqualTo(param));
		}

		[Test]
		public void GenericAgainstArgumentNoMessage_FalseCondition_NoException()
		{
			Assert.That(() => Guard.AgainstArgument<ArgumentNullException>("param", "asd" == null), Throws.Nothing);
		}

		[Test]
		public void GenericAgainstArgumentNoMessage_TrueCondition_ExceptionWithDefaultMessageAndParam()
		{
			string param = "param";
			bool trueCondition = 3 > 2;
			Assert.That(() => Guard.AgainstArgument<ArgumentNullException>(param, trueCondition),
				Throws.InstanceOf<ArgumentNullException>()
					.With.Message.EqualTo(new ArgumentNullException(param).Message).And
					.With.Property("ParamName").EqualTo(param));
		}

		internal class GuardSubject
		{
			public void Method(string param)
			{
				Guard.AgainstNullArgument("param", param);
			}

			public void Method(string x, char? y)
			{
				Guard.AgainstNullArgument(new { x, y });
			}
		}

		#region anonymous null argument checking

		[Test]
		public void AgainstNullArguments_NullContainer_Exception()
		{
			// Use an initial assignment to get the right anonymous type. Ick!
			var nullContainer = new { x = "hi" };
			nullContainer = null;

			Assert.That(() => Guard.AgainstNullArgument(nullContainer), Throws.InstanceOf<ArgumentNullException>()
				.With.Property("ParamName").EqualTo("container"));
		}

		[Test]
		public void AgainstNullArgument_ValueTypeArgument_NoException()
		{
			var withValueType = new { i = 5 };
			Assert.That(() => Guard.AgainstNullArgument(withValueType), Throws.Nothing);
		}

		[Test]
		public void AgainstNullArgument_NoNulls_NoException()
		{
			var notNulls = new { x = "hello", y = new object() };
			Assert.That(() => Guard.AgainstNullArgument(notNulls), Throws.Nothing);
		}

		[Test]
		public void AgainstNullArgument_SingleNullValue_ExceptionWithArgumentName()
		{
			var singleNull = new { x = (Stream)null };
			Assert.That(() => Guard.AgainstNullArgument(singleNull), Throws.InstanceOf<ArgumentNullException>()
				.With.Property("ParamName").EqualTo("x"));
		}

		[Test]
		public void AgainstNullArgument_MultipleNullValues_ExceptionWithOneName()
		{
			var multipleNulls = new { x = "hello", y = (string)null, z = (string)null };

			Assert.That(() => Guard.AgainstNullArgument(multipleNulls), Throws.InstanceOf<ArgumentNullException>()
				.With.Property("ParamName").EqualTo("y").Or
				.With.Property("ParamName").EqualTo("z"));
		}

		[Test]
		public void AgainstNullArgument_NullableNull_Exception()
		{
			var withNullableNull = new { x = default(int?) };

			Assert.That(() => Guard.AgainstNullArgument(withNullableNull), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AgainstNullArgument_NullableNotNull_NoException()
		{
			// ReSharper disable once RedundantExplicitNullableCreation
			// ReSharper disable once ConvertNullableToShortForm
			var withNullableNull = new { x = new Nullable<int>(4) };

			Assert.That(() => Guard.AgainstNullArgument(withNullableNull), Throws.Nothing);
		}

		[Test]
		public void AgainstNullArgument_NoNullArguments_NoException()
		{
			Assert.That(() => new GuardSubject().Method("notNull", 'A'), Throws.Nothing);
		}

		// ReSharper disable ExpressionIsAlwaysNull
		[Test]
		public void AgainstNullArgument_SomeNullArguments_ExceptionWithDefaultMessageAndParam()
		{
			string paramName = "x", actualMessage = new ArgumentNullException(paramName).Message;

			Assert.That(() => new GuardSubject().Method(null, 'B'), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.EqualTo(actualMessage).And
				.With.Property("ParamName").EqualTo(paramName));
		}
		// ReSharper restore ExpressionIsAlwaysNull

		#endregion
	}
	// ReSharper restore ConditionIsAlwaysTrueOrFalse
}
