using System;
using System.Runtime.Serialization;

namespace Vertica.Utilities_v4
{
	[Serializable]
	public partial class InvalidDomainException<T>
	{
		protected InvalidDomainException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}
