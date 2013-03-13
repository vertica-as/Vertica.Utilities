using System;

namespace Vertica.Utilities_v4.Extensions.Infrastructure
{
	public class ExtensionPoint<T> : IExtensionPoint<T>
	{
		private readonly Type _type;
		private readonly T _value;

		public ExtensionPoint(T value)
		{
			_value = value;
		}

		public ExtensionPoint(Type type)
		{
			_type = type;
		}

		public T ExtendedValue
		{
			get { return _value; }
		}

		object IExtensionPoint.ExtendedValue
		{
			get { return _value; }
		}

		public Type ExtendedType
		{
			get { return _type ?? typeof(T); }
		}
	}
}