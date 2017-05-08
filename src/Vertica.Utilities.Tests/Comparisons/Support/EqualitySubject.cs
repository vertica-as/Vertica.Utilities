namespace Vertica.Utilities_v4.Tests.Comparisons.Support
{
	internal class EqualitySubject
	{
		public EqualitySubject() { }

		public EqualitySubject(string s, int i, decimal d)
		{
			S = s;
			I = i;
			D = d;
		}

		public int I { get; set; }
		public string S { get; set; }
		public decimal D { get; set; }

		public override string ToString()
		{
			return "[" + S + " " + I + " " + D + "]";
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = I;
				hashCode = (hashCode * 397) ^ D.GetHashCode();
				hashCode = (hashCode * 397) ^ (S != null ? S.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}
