namespace Vertica.Utilities_v4.Data
{
	public interface IStorage
	{
		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> with the specified key.
		/// </summary>
		/// <value></value>
		object this[object key] { get; set; }

		/// <summary>
		/// Clears this instance.
		/// </summary>
		void Clear();

		int Count { get; }
	}
}