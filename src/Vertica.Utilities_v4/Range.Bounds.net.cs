using System;
using System.Collections.Generic;
using Vertica.Utilities_v4.Extensions.ComparableExt;

namespace Vertica.Utilities_v4
{
	[Serializable]
	internal partial struct Closed<T> { }

	[Serializable]
	internal partial struct Open<T> { }
}
