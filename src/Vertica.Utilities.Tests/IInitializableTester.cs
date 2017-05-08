using System;
using NUnit.Framework;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class IInitializableTester
	{
		class SimplestInit : IInitializable
		{
			public void Initialize() { }
		}

		class SimpleInit : IInitializable<ArgumentException>
		{
			public void Initialize(ArgumentException t0) { }
		}
	}
}