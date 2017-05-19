namespace Vertica.Utilities.Patterns
{
	public interface IIdentifiable<out K> { K Id { get; } }
}