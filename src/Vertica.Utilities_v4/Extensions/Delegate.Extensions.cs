using System;

namespace Vertica.Utilities_v4.Extensions.DelegateExt
{
	public static class DelegateExtensions
	{
		public static T Cast<T>(this Delegate value) where T : class
		{
			if (value == null) return null;

			Delegate[] delegates = value.GetInvocationList();
			if (delegates.Length == 1)
			{
				return Delegate.CreateDelegate(typeof(T), delegates[0].Target, delegates[0].Method) as T;
			}
			for (int i = 0; i < delegates.Length; i++)
			{
				delegates[i] = Delegate.CreateDelegate(typeof(T), delegates[i].Target, delegates[0].Method);
			}
			return Delegate.Combine(delegates) as T;
		}

		public static Action<TBase> Cast<TBase, TDerived>(this Action<TDerived> source) where TDerived : TBase
		{
			return baseValue => source((TDerived)baseValue);
		}
	}
}