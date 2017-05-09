using System.Collections.Generic;

namespace Vertica.Utilities.Collections
{
	/// <summary>
	/// Reference generic implementation of <see cref="KeyValueAdapter{TKey,TValue}"/>.
	/// <para>Adapts the indexer of a generic <see cref="IDictionary{TKey,TValue}"/>.</para>
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class GenericDictionaryAdapter<TKey, TValue> : KeyValueAdapter<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _adaptee;

		public GenericDictionaryAdapter(IDictionary<TKey, TValue> adaptee)
		{
			_adaptee = adaptee;
		}

		/// <summary>
		/// Adapts the indexer of the dictionary
		/// </summary>
		/// <exception cref="KeyNotFoundException">Bubbled up from the adaptee, when a key is not found.</exception>
		/// <param name="key"></param>
		/// <returns></returns>
		public override TValue this[TKey key]
		{
			get { return _adaptee[key]; }
			set { _adaptee[key] = value; }
		}
	}
}