using NSubstitute;
using NUnit.Framework;
using Vertica.Utilities.Tests.Patterns.Support;
using Vertica.Utilities.Patterns;

namespace Vertica.Utilities.Tests.Patterns
{
	[TestFixture]
	public class ReturningChainOfResponsibilityTester
	{
		#region documentation

		[Test, Category("Exploratory")]
		public void Handled_HandlerReturnsValue()
		{
			var chain = ChainOfResponsibility
				.Empty<int, string>()
				.Chain(new IntToStringLink(1))
				.Chain(new IntToStringLink(2))
				.Chain(new IntToStringLink(3));

			Assert.That(chain.Handle(2), Is.EqualTo("2"));
		}

		[Test, Category("Exploratory")]
		public void NotHandled_DefaultReturned()
		{
			var chain = ChainOfResponsibility
				.Empty<int, string>()
				.Chain(new IntToStringLink(1))
				.Chain(new IntToStringLink(2))
				.Chain(new IntToStringLink(3));

			Assert.That(chain.Handle(5), Is.Null);
		}

		[Test, Category("Exploratory")]
		public void TryHandle_True_AndHandlerReturnsValue()
		{
			var chain = ChainOfResponsibility
				.Empty<int, string>()
				.Chain(new IntToStringLink(1))
				.Chain(new IntToStringLink(2))
				.Chain(new IntToStringLink(3));

			string result;
			Assert.That(chain.TryHandle(2, out result), Is.True);
			Assert.That(result, Is.EqualTo("2"));
		}

		[Test, Category("Exploratory")]
		public void TryNotHandled_False_AndDefaultReturned()
		{
			var chain = ChainOfResponsibility
				.Empty<int, string>()
				.Chain(new IntToStringLink(1))
				.Chain(new IntToStringLink(2))
				.Chain(new IntToStringLink(3));

			string result;
			Assert.That(chain.TryHandle(5, out result), Is.False);
			Assert.That(result, Is.Null);
		}

		#endregion

		#region Chain

		[Test]
		public void Chain_FluentLinkingTwo_InnerChaining()
		{
			var l1 = new IntToStringLink(1);
			var l2 = new IntToStringLink(2);

			Assert.That(l1.Chain(l2), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.Null);
		}

		[Test]
		public void Chain_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.Chain(l2).Chain(l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Params_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

			Assert.That(l1.Chain(l2, l3), Is.SameAs(l1));

			Assert.That(l1.Next, Is.SameAs(l2));
			Assert.That(l2.Next, Is.SameAs(l3));
			Assert.That(l3.Next, Is.Null);
		}

		[Test]
		public void Chain_Enumerable_FluentLinkingThree_InnerChaining()
		{
			IntToStringLink l1 = new IntToStringLink(1), l2 = new IntToStringLink(2), l3 = new IntToStringLink(3);

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

		private ChainOfResponsibilityLink<int, string> initChainOfSubstitutes(
			out ChainOfResponsibilityLink<int, string> first,
			out ChainOfResponsibilityLink<int, string> second,
			out ChainOfResponsibilityLink<int, string> third
			)
		{
			first = Substitute.For<ChainOfResponsibilityLink<int, string>>();
			second = Substitute.For<ChainOfResponsibilityLink<int, string>>();
			third = Substitute.For<ChainOfResponsibilityLink<int, string>>();

			var chain = ChainOfResponsibility
				.Empty<int, string>()
				.Chain(first)
				.Chain(second)
				.Chain(third);

			return chain;
		}
	}
}