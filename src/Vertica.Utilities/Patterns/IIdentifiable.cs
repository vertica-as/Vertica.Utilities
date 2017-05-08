namespace Vertica.Utilities_v4.Patterns
{
	public interface IIdentifiable<out K> { K Id { get; } }
}