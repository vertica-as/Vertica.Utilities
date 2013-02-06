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
			var chain = ChainOfResponsibilityLink<Context>
				.Empty()
				.FluentChain(new ToUpperIfStartsWith("1"))
				.FluentChain(new ToUpperIfStartsWith("2"))
				.FluentChain(new ToUpperIfStartsWith("3"));

			var context = new Context("2_a");
			chain.Handle(context);
			Assert.That(context.S, Is.EqualTo("2_A"));
		}

		[Test, Category("Exploratory")]
		public void NotHandled_ContextNotModified()
		{
			var chain = ChainOfResponsibilityLink<Context>
				.Empty()
				.FluentChain(new ToUpperIfStartsWith("1"))
				.FluentChain(new ToUpperIfStartsWith("2"))
				.FluentChain(new ToUpperIfStartsWith("3"));

			var context = new Context("5_a");
			chain.Handle(context);
			Assert.That(context.S, Is.EqualTo("5_a"));
		}

		[Test, Category("Exploratory")]
		public void TryHandled_ContextModified()
		{
			var chain = ChainOfResponsibilityLink<Context>
				.Empty()
				.FluentChain(new ToUpperIfStartsWith("1"))
				.FluentChain(new ToUpperIfStartsWith("2"))
				.FluentChain(new ToUpperIfStartsWith("3"));

			var context = new Context("2_a");
			Assert.That(chain.TryHandle(context), Is.True);
			Assert.That(context.S, Is.EqualTo("2_A"));
		}

		[Test, Category("Exploratory")]
		public void TryNotHandled_ContextNotModified()
		{
			var chain = ChainOfResponsibilityLink<Context>
				.Empty()
				.FluentChain(new ToUpperIfStartsWith("1"))
				.FluentChain(new ToUpperIfStartsWith("2"))
				.FluentChain(new ToUpperIfStartsWith("3"));

			var context = new Context("5_a");
			Assert.That(chain.TryHandle(context), Is.False);
			Assert.That(context.S, Is.EqualTo("5_a"));
		}

		#endregion

		#region Chain

		[Test]
		public void Chain_NotSoFluentLinkingTwo_InnerChaining()
		{
			var l1 = new ToUpperIfStartsWith("1");
			var l2 = new ToUpperIfStartsWith("2");

			Assert.That(l1.Chain(l2), Is.SameAs(l2));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}

		[Test]
		public void Chain_NotSoFluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.Chain(l2).Chain(l3), Is.SameAs(l3));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Params_NotSoFluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.Chain(l2, l3), Is.SameAs(l3));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Enumerable_NotSoFluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

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

		#region FluentChain

		[Test]
		public void FluentChain_FluentLinkingTwo_InnerChaining()
		{
			var l1 = new ToUpperIfStartsWith("1");
			var l2 = new ToUpperIfStartsWith("2");

			Assert.That(l1.FluentChain(l2), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}

		[Test]
		public void FluentChain_FluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.FluentChain(l2).FluentChain(l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void FluentChain_Params_FluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.FluentChain(l2, l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void FluentChain_Enumerable_FluentLinkingThree_InnerChaining()
		{
			ToUpperIfStartsWith l1 = new ToUpperIfStartsWith("1"),
				l2 = new ToUpperIfStartsWith("2"),
				l3 = new ToUpperIfStartsWith("3");

			Assert.That(l1.FluentChain(new[] { l2, l3 }), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
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

			var chain = ChainOfResponsibilityLink<string>
				.Empty()
				.FluentChain(first)
				.FluentChain(second)
				.FluentChain(third);

			return chain;
		}
	}
}