using NUnit.Framework;

namespace Vertica.Utilities.Tests
{
	[TestFixture]
	public class ICloneableTester
	{
		class Complex
		{
			public string S { get; set; }
			public decimal D { get; set; }
		}

		class CanBeShallowCloned : IShallowCloneable<CanBeShallowCloned>
		{
			public int Simple { get; set; }
			public Complex Complex { get; set; }

			public CanBeShallowCloned ShallowClone()
			{
				return new CanBeShallowCloned
				{
					Simple = Simple
				};
			}
		}

		class CanBeDeeplyCloned : IDeepCloneable<CanBeDeeplyCloned>
		{
			public int Simple { get; set; }
			public Complex Complex { get; set; }
			
			public CanBeDeeplyCloned DeepClone()
			{
				return new CanBeDeeplyCloned
				{
					Simple = Simple,
					Complex = Complex != null ?
						new Complex { D = Complex.D, S = Complex.S } :
						default(Complex)
				};
			}
		}

		private class ImplementsBothCloneSemantics : ICloneable<ImplementsBothCloneSemantics>
		{
			public int Simple { get; set; }
			public Complex Complex { get; set; }

			public ImplementsBothCloneSemantics ShallowClone()
			{
				return new ImplementsBothCloneSemantics
				{
					Simple = Simple
				};
			}

			public ImplementsBothCloneSemantics DeepClone()
			{
				return new ImplementsBothCloneSemantics
				{
					Simple = Simple,
					Complex = Complex != null ?
						new Complex { D = Complex.D, S = Complex.S } :
						default(Complex)
				};
			}
		}
	}
}