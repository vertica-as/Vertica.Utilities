﻿namespace Vertica.Utilities.Extensions.Infrastructure
{
	public static partial class EntryPoints
	{
		public static CastExtensionPoint<object> Cast(this object subject)
		{
			return new CastExtensionPoint<object>(subject);
		}
	}
}