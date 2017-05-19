using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Vertica.Utilities.Collections
{
	public class FrameworkRandomizer : IRandomizer
	{
		public FrameworkRandomizer() { }

		private int? _maxValue;
		public FrameworkRandomizer(int maxValue)
		{
			_maxValue = maxValue;
		}

		private static int _seed = Environment.TickCount;

		[ThreadStatic]
		private static Random _random;

		private static Random random
		{
			get
			{
				return _random ?? (_random = new Random(Interlocked.Increment(ref _seed)));
			}
		}

		public int Next(int maxValue)
		{
			return random.Next(maxValue);
		}

		private IEnumerable<int> buildEnumerable()
		{
			while (true)
			{
				yield return _maxValue.HasValue ? random.Next(_maxValue.Value) : random.Next();
			}
		}

		public IEnumerator<int> GetEnumerator()
		{
			return buildEnumerable().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}