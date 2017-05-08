using System;

namespace Vertica.Utilities_v4.Tests.Reflection.Support
{
	internal class StaticReflectionSubjectType
	{
		public int Func3(int i, int j) { return 0; }

		public StaticReflectionSubjectType2 ComplexProperty { get; set; }

		public StaticReflectionSubjectType2 ComplexField;

		public int SimpleField;

		public static int SimpleStaticField = 3;
		public static StaticReflectionSubjectType2 ComplexStaticProperty { get; set; }

		public event EventHandler Event;

		public void VoidMethod() { }
		public void NonVoidMethod(int i) { }
		public int VoidFunction() { return 0; }
		public int NonVoidFunction(int i) { return 0; }
	}

	internal class StaticReflectionSubjectType2
	{
		public StaticReflectionSubjectType3 ComplexProperty { get; set; }
	}

	internal class StaticReflectionSubjectType3
	{
		public int SimpleProperty { get; set; }
	}
}
