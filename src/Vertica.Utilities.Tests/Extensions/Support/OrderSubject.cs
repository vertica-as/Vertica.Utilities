namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	internal class OrderSubject
	{
		public OrderSubject(int i1, int i2)
		{
			I1 = i1;
			I2 = i2;
		}

		public int I1 { get; private set; }
		public int I2 { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", I1, I2);
		}
	}
}