namespace Vertica.Utilities_v4.Tests.Collections
{
	/// <summary>
	/// Initially to adapt the string indexer from IDictionary and NameValueCollection now adapts any indexer.
	/// </summary>
	/// <typeparam name="TKey">Type of the key elements.</typeparam>
	/// <typeparam name="TValue">Type of the values.</typeparam>
	public abstract class KeyValueAdapter<TKey, TValue>
	{
		public abstract TValue this[TKey key] { get; set; }
	}
}