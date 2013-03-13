using System;
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


		[Test]
		public void IChainOfResponsibility_Works_JustFine()
		{
			var chain = ChainOfResponsibility.Empty<int, string>()
				.Chain(new ResponsibleLink<int, string>(new MultiLink(2)))
				.Chain(new MultiLink(1));

			var anotherchain = ChainOfResponsibility
				.Empty<Exception>()
				.Chain(new ResponsibleLink<Exception>(new MultiLink()))
				.Chain(new MultiLink());

			Assert.That(chain.Handle(1), Is.EqualTo("1"));
			Exception ex = new Exception();
			anotherchain.Handle(ex);
			Assert.That(ex.Data, Is.Not.Empty);
		}
	}
}