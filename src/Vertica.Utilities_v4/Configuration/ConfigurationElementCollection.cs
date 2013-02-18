using System.Configuration;

namespace Vertica.Utilities_v4.Configuration
{
	public abstract class ConfigurationElementCollection<TKey, TValue> : ConfigurationElementCollection
		where TValue : ConfigurationElement, new()
	{
		public abstract override ConfigurationElementCollectionType CollectionType { get; }

		protected abstract override string ElementName { get; }

		public TValue this[TKey key]
		{
			get { return (TValue) BaseGet(key); }
		}

		public TValue this[int index]
		{
			get { return (TValue) BaseGet(index); }
		}

		protected abstract TKey GetElementKey(TValue element);

		protected override ConfigurationElement CreateNewElement()
		{
			return new TValue();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return GetElementKey((TValue) element);
		}

		public void Add(TValue path)
		{
			BaseAdd(path);
		}

		public void Remove(TKey key)
		{
			BaseRemove(key);
		}
	}
}