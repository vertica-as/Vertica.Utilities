using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Patterns;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	[TestFixture]
	public class GenericCommandTester
	{
		public interface CommandSubject1
		{
			void Op1();
			void Op2();
		}

		public interface CommandSubject2
		{
			int Op1();
			string Op2();
		}

		public interface CommandSubject3
		{
			int Op1(string s);
			string Op2(decimal d);
		}

		[Test]
		public void Execute_Subject1Methods_CommandsInvokeMethod()
		{
			var subject = Substitute.For<CommandSubject1>();

			var op1Command = new GenericCommand<CommandSubject1>(
				subject, obj => obj.Op1());

			op1Command.Execute();

			subject.Received().Op1();
			subject.DidNotReceive().Op2();

			subject.ClearReceivedCalls();
			var op2Command = new GenericCommand<CommandSubject1>(
				subject, obj => obj.Op2());

			op2Command.Execute();

			subject.Received().Op2();
			subject.DidNotReceive().Op1();
		}

		[Test]
		public void Execute_Subject2Methods_CommandsInvokeMethod()
		{
			var subject = Substitute.For<CommandSubject2>();

			var op1Command = new GenericCommand<CommandSubject2, int>(
				subject, obj => obj.Op1());

			op1Command.Execute();

			subject.Received().Op1();
			subject.DidNotReceive().Op2();

			subject.ClearReceivedCalls();
			var op2Command = new GenericCommand<CommandSubject2, string>(
				subject, obj => obj.Op2());

			op2Command.Execute();

			subject.Received().Op2();
			subject.DidNotReceive().Op1();
		}

		[Test]
		public void Execute_Subject3Methods_CommandsInvokeMethod()
		{
			var subject = Substitute.For<CommandSubject3>();

			var op1Command = new GenericCommand<CommandSubject3, string, int>(
				subject, (obj, input) => obj.Op1(input));

			op1Command.Execute("s");
			
			subject.Received().Op1("s");

			var op2Command = new GenericCommand<CommandSubject3, decimal, string>(
				subject, (obj, input) => obj.Op2(input));

			op2Command.Execute(3m);
			subject.Received().Op2(3m);
		}
	}
}