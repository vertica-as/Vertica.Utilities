using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities_v4.Patterns;
using Vertica.Utilities_v4.Tests.Patterns.Support;

namespace Vertica.Utilities_v4.Tests.Patterns
{
	[TestFixture]
	public class ChainOfResponsibilityTester
	{
		#region documentation

		[Test, Category("Exploratory")]
		public void Handled_ContextModifed()
		{
			var chain = ChainOfResponsibility
				.Empty<Context>()
				.Chain(new ToUpperIfStartsWith("1"))
				.Chain(new ToUpperIfStartsWith("2"))
				.Chain(new ToUpperIfStartsWith("3"));

			var context = new Context("2_a");
			chain.Handle(context);
			Assert.That(context.S, Is.EqualTo("2_A"));
		}

		[Test, Category("Exploratory")]
		public void NotHandled_ContextNotModified()
		{
			var chain = ChainOfResponsibility
				.Empty<Context>()
				.Chain(new ToUpperIfStartsWith("1"))
				.Chain(new ToUpperIfStartsWith("2"))
				.Chain(new ToUpperIfStartsWith("3"));

			var context = new Context("5_a");
			chain.Handle(context);
			Assert.That(context.S, Is.EqualTo("5_a"));
		}

		[Test, Category("Exploratory")]
		public void TryHandled_ContextModified()
		{
			var chain = ChainOfResponsibility
				.Empty<Context>()
				.Chain(new ToUpperIfStartsWith("1"))
				.Chain(new ToUpperIfStartsWith("2"))
				.Chain(new ToUpperIfStartsWith("3"));

			var context = new Context("2_a");
			Assert.That(chain.TryHandle(context), Is.True);
			Assert.That(context.S, Is.EqualTo("2_A"));
		}

		[Test, Category("Exploratory")]
		public void TryNotHandled_ContextNotModified()
		{
			var chain = ChainOfResponsibility
				.Empty<Context>()
				.Chain(new ToUpperIfStartsWith("1"))
				.Chain(new ToUpperIfStartsWith("2"))
				.Chain(new ToUpperIfStartsWith("3"));

			var context = new Context("5_a");
			Assert.That(chain.TryHandle(context), Is.False);
			Assert.That(context.S, Is.EqualTo("5_a"));
		}

		#endregion

		#region Chain

		[Test]
		public void Chain_LinkingTwo_InnerChaining()
		{
			var l1 = new ToUpperIfStartsWith("1");
			var l2 = new ToUpperIfStartsWith("2");

			Assert.That(l1.Chain(l2), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}

		[Test]
		public void Chain_LinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.Chain(l2).Chain(l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Params_LinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.Chain(l2, l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Enumerable_LinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.Chain(new[] { l2, l3 }), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		#endregion

		#region Handle

		[Test]
		public void Handle_ContextHandledByFirstInChain_RestOfMembersNotEvenAsked()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			first.CanHandle("str").Returns(true);

			chain.Handle("str");

			second.DidNotReceiveWithAnyArgs().CanHandle(null);
			third.DidNotReceiveWithAnyArgs().CanHandle(null);
		}

		[Test]
		public void Handle_ContextHandledBySecondInChain_FirstAskedAndThirdNot()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle("str").Returns(true);

			chain.Handle("str");

			first.Received().CanHandle("str");
			third.DidNotReceiveWithAnyArgs().CanHandle(null);
		}

		[Test]
		public void Handle_ContextHandledByLastInChain_AllAskedAndLastHandles()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			third.CanHandle("str").Returns(true);

			chain.Handle("str");

			first.Received().CanHandle("str");
			second.Received().CanHandle("str");
		}

		[Test]
		public void Handle_ContextNotHandled_AllAskedNoneHandled()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			chain.Handle("str");

			first.Received().CanHandle("str");
			second.Received().CanHandle("str");
			third.Received().CanHandle("str");
		}

		#endregion

		#region TryHandle

		[Test]
		public void TryHandle_ContextHandledByFirstInChain_RestOfMembersNotEvenAsked()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			first.CanHandle("str").Returns(true);

			Assert.That(chain.TryHandle("str"), Is.True);

			second.DidNotReceiveWithAnyArgs().CanHandle(null);
			third.DidNotReceiveWithAnyArgs().CanHandle(null);
		}

		[Test]
		public void TryHandle_ContextHandledBySecondInChain_FirstAskedAndThirdNot()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle("str").Returns(true);

			Assert.That(chain.TryHandle("str"), Is.True);

			first.Received().CanHandle("str");
			third.DidNotReceiveWithAnyArgs().CanHandle(null);
		}

		[Test]
		public void TryHandle_ContextHandledByLastInChain_AllAskedAndLastHandles()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			third.CanHandle("str").Returns(true);

			Assert.That(chain.TryHandle("str"), Is.True);

			first.Received().CanHandle("str");
			second.Received().CanHandle("str");
		}

		[Test]
		public void TryHandle_ContextNotHandled_AllAskedNoneHandled()
		{
			ChainOfResponsibilityLink<string> first, second, third;
			ChainOfResponsibilityLink<string> chain = initChainOfSubstitutes(out first, out second, out third);

			Assert.That(chain.TryHandle("str"), Is.False);

			first.Received().CanHandle("str");
			second.Received().CanHandle("str");
			third.Received().CanHandle("str");
		}

		#endregion

		private ChainOfResponsibilityLink<string> initChainOfSubstitutes(
			out ChainOfResponsibilityLink<string> first,
			out ChainOfResponsibilityLink<string> second,
			out ChainOfResponsibilityLink<string> third
			)
		{
			first = Substitute.For<ChainOfResponsibilityLink<string>>();
			second = Substitute.For<ChainOfResponsibilityLink<string>>();
			third = Substitute.For<ChainOfResponsibilityLink<string>>();

			var chain = ChainOfResponsibility
				.Empty<string>()
				.Chain(first)
				.Chain(second)
				.Chain(third);

			return chain;
		}
	}

	[TestFixture]
	public class ReturningChainOfResponsibilityTester
	{
		#region Chain

		[Test]
		public void Chain_NotSoFluentLinkingTwo_InnerChaining()
		{
			var l1 = new IntToStringLink(1);
			var l2 = new IntToStringLink(2);

			Assert.That(l1.Chain(l2), Is.SameAs(l2));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}
		
		[Test]
		public void Chain_NotSoFluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.Chain(l2).Chain(l3), Is.SameAs(l3));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Params_NotSoFluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.Chain(l2, l3), Is.SameAs(l3));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Enumerable_NotSoFluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.Chain(new[] { l2, l3 }), Is.SameAs(l3));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		#endregion
		
		#region Handle

		[Test]
		public void Handle_ContextHandledByFirstInChain_RestOfMembersNotEvenAsked()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			first.CanHandle(1).Returns(true);

			chain.Handle(1);

			second.DidNotReceiveWithAnyArgs().CanHandle(0);
			third.DidNotReceiveWithAnyArgs().CanHandle(0);
		}
		
		[Test]
		public void Handle_ContextHandledBySecondInChain_FirstAskedAndThirdNot()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle(1).Returns(true);

			chain.Handle(1);

			first.Received().CanHandle(1);
			third.DidNotReceiveWithAnyArgs().CanHandle(0);
		}

		[Test]
		public void Handle_ContextHandledByLastInChain_AllAskedAndLastHandles()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			third.CanHandle(1).Returns(true);

			chain.Handle(1);

			first.Received().CanHandle(1);
			second.Received().CanHandle(1);
		}

		[Test]
		public void Handle_ContextHandled_ReturnValueComesFromHandler()
		{
			string returnValue = "handled by second";

			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle(1).Returns(true);
			second.Handle(1).Returns(returnValue);

			Assert.That(chain.Handle(1), Is.EqualTo(returnValue));
		}
		
		[Test]
		public void Handle_ContextNotHandled_AllAskedNoneHandled()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			Assert.That(chain.Handle(1), Is.Null);

			first.Received().CanHandle(1);
			second.Received().CanHandle(1);
			third.Received().CanHandle(1);
		}

		#endregion

		#region TryHandle

		[Test]
		public void TryHandle_ContextHandledByFirstInChain_RestOfMembersNotEvenAsked()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			first.CanHandle(1).Returns(true);

			string result;
			chain.TryHandle(1, out result);

			second.DidNotReceiveWithAnyArgs().CanHandle(0);
			third.DidNotReceiveWithAnyArgs().CanHandle(0);
		}

		[Test]
		public void TryHandle_ContextHandledBySecondInChain_FirstAskedAndThirdNot()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle(1).Returns(true);

			string result;
			Assert.That(chain.TryHandle(1, out result), Is.True);

			first.Received().CanHandle(1);
			third.DidNotReceiveWithAnyArgs().CanHandle(0);
		}

		[Test]
		public void TryHandle_ContextHandledByLastInChain_AllAskedAndLastHandles()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			third.CanHandle(1).Returns(true);

			string result;
			Assert.That(chain.TryHandle(1, out result), Is.True);

			first.Received().CanHandle(1);
			second.Received().CanHandle(1);
		}

		[Test]
		public void TryHandle_ContextHandled_ReturnValueComesFromHandler()
		{
			string returnValue = "handled by second";

			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			second.CanHandle(1).Returns(true);
			second.Handle(1).Returns(returnValue);

			string result;
			Assert.That(chain.TryHandle(1, out result), Is.True);
			Assert.That(result, Is.EqualTo(returnValue));
		}

		[Test]
		public void TryHandle_ContextNotHandled_AllAskedNoneHandled()
		{
			ChainOfResponsibilityLink<int, string> first, second, third;
			ChainOfResponsibilityLink<int, string> chain = initChainOfSubstitutes(out first, out second, out third);

			string result;
			Assert.That(chain.TryHandle(1, out result), Is.False);
			Assert.That(result, Is.Null);

			first.Received().CanHandle(1);
			second.Received().CanHandle(1);
			third.Received().CanHandle(1);
		}

		#endregion
		
		#region FluentChain

		[Test]
		public void FluentChain_FluentLinkingTwo_InnerChaining()
		{
			var l1 = new IntToStringLink(1);
			var l2 = new IntToStringLink(2);

			Assert.That(l1.FluentChain(l2), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}

		[Test]
		public void FluentChain_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.FluentChain(l2).FluentChain(l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void FluentChain_Params_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.FluentChain(l2, l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void FluentChain_Enumerable_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.FluentChain(new[] { l2, l3 }), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void FluentChain_SampleScenario()
		{
			var chain = ChainOfResponsibilityLink<int, string>
				.Empty()
				.FluentChain(new IntToStringLink(1))
				.FluentChain(new IntToStringLink(2))
				.FluentChain(new IntToStringLink(3));

			Assert.That(chain.Handle(2), Is.EqualTo("2"));
		}

		#endregion

		private ChainOfResponsibilityLink<int, string> initChainOfSubstitutes(
			out ChainOfResponsibilityLink<int, string> first,
			out ChainOfResponsibilityLink<int, string> second,
			out ChainOfResponsibilityLink<int, string> third
			)
		{
			first = Substitute.For<ChainOfResponsibilityLink<int, string>>();
			second = Substitute.For<ChainOfResponsibilityLink<int, string>>();
			third = Substitute.For<ChainOfResponsibilityLink<int, string>>();

			var chain = ChainOfResponsibilityLink<int, string>
				.Empty()
				.FluentChain(first)
				.FluentChain(second)
				.FluentChain(third);

			return chain;
		}
	}
}