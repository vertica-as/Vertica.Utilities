using System.Collections.Generic;
using System.Linq;
using Vertica.Utilities_v4.Extensions.EnumerableExt;

namespace Vertica.Utilities_v4.Patterns
{
	public interface IOperation<T>
	{
		IEnumerable<T> Execute(IEnumerable<T> input);
	}

	public class Pipeline<T>
	{
		private readonly List<IOperation<T>> _operations;

		public Pipeline()
		{
			_operations = new List<IOperation<T>>();
		}

		public Pipeline(params IOperation<T>[] operations) : this(operations.AsEnumerable()) { }

		public Pipeline(IEnumerable<IOperation<T>> operations) : this()
		{
			operations.ForEach(o => Register(o));
		}

		public Pipeline<T> Register(IOperation<T> operation)
		{
			if (operation != null) _operations.Add(operation);
			return this;
		}

		public void Execute()
		{
			IEnumerable<T> current = Enumerable.Empty<T>();
			current = _operations.Aggregate(current, (input, operation) => operation.Execute(input));
			foreach (var c in current) { }
		}
	}
}
