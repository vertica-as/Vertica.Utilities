﻿using System;

namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	public class SafeSubject
	{
		public Exception ReferenceProperty { get; set; } 
		public int ValueProperty { get; set; }
		public decimal? NullableProperty { get; set; }
	}
}