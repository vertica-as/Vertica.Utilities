using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Vertica.Utilities.Extensions.StringExt;

namespace Vertica.Utilities
{
	public struct Pair<T> : IEnumerable<T>, IEquatable<Pair<T>>
	{
		public T First { get; private set; }
		public T Second { get; private set; }

		public Pair(T first, T second) : this()
		{
			First = first;
			Second = second;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
		}

		public bool Equals(Pair<T> other)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			return comparer.Equals(First, other.First) &&
				comparer.Equals(Second, other.Second);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Pair<T> && Equals((Pair<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;
				return (comparer.GetHashCode(First)*397) ^ comparer.GetHashCode(Second);
			}
		}

		public static bool operator ==(Pair<T> left, Pair<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Pair<T> left, Pair<T> right)
		{
			return !left.Equals(right);
		}

		public static Pair<T> Parse(string pair, char tokenizer)
		{
			Pair<T> result = default(Pair<T>);
			if (pair.IsNotEmpty())
			{
				var tokens = pair.Split(tokenizer);

				Guard.AgainstArgument("pair", tokens.Length != 2,
					Resources.Exceptions.Tuploids_ParseTemplate,
					"2",
					tokenizer.ToString());

				result = new Pair<T>(
					tokens[0].Parse<T>(),
					tokens[1].Parse<T>());
			}
			return result;
		}

		public KeyValuePair<T, T> ToKeyValuePair()
		{
			return new KeyValuePair<T, T>(First, Second);
		}
	}

	public struct Triplet<T> : IEnumerable<T>, IEquatable<Triplet<T>>
	{
		public T First { get; private set; }
		public T Second { get; private set; }
		public T Third { get; private set; }

		public Triplet(T first, T second, T third) : this()
		{
			First = first;
			Second = second;
			Third = third;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
			yield return Third;
		}

		public bool Equals(Triplet<T> other)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			return comparer.Equals(First, other.First) &&
				comparer.Equals(Second, other.Second) &&
				comparer.Equals(Third, other.Third);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Triplet<T> && Equals((Triplet<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;

				int hashCode = comparer.GetHashCode(First);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Second);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Third);
				return hashCode;
			}
		}

		public static bool operator ==(Triplet<T> left, Triplet<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Triplet<T> left, Triplet<T> right)
		{
			return !left.Equals(right);
		}

		public static Triplet<T> Parse(string triplet, char tokenizer)
		{
			Triplet<T> result = default(Triplet<T>);
			if (triplet.IsNotEmpty())
			{
				var tokens = triplet.Split(tokenizer);

				Guard.AgainstArgument("pair", tokens.Length != 2,
					Resources.Exceptions.Tuploids_ParseTemplate,
					"3",
					tokenizer.ToString());

				result = new Triplet<T>(
					tokens[0].Parse<T>(),
					tokens[1].Parse<T>(),
					tokens[2].Parse<T>());
			}
			return result;
		}
	}

	public struct Quartet<T> : IEnumerable<T>, IEquatable<Quartet<T>>
	{
		public T First { get; private set; }
		public T Second { get; private set; }
		public T Third { get; private set; }
		public T Fourth { get; private set; }

		public Quartet(T first, T second, T third, T fourth) : this()
		{
			First = first;
			Second = second;
			Third = third;
			Fourth = fourth;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
			yield return Third;
			yield return Fourth;
		}

		public bool Equals(Quartet<T> other)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			return comparer.Equals(First, other.First) &&
				comparer.Equals(Second, other.Second) &&
				comparer.Equals(Third, other.Third) &&
				comparer.Equals(Fourth, other.Fourth);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Quartet<T> && Equals((Quartet<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;
				int hashCode = comparer.GetHashCode(First);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Second);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Third);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Fourth);
				return hashCode;
			}
		}

		public static bool operator ==(Quartet<T> left, Quartet<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Quartet<T> left, Quartet<T> right)
		{
			return !left.Equals(right);
		}

		public static Quartet<T> Parse(string quintet, char tokenizer)
		{
			Quartet<T> result = default(Quartet<T>);
			if (quintet.IsNotEmpty())
			{
				var tokens = quintet.Split(tokenizer);

				Guard.AgainstArgument("pair", tokens.Length != 4,
					Resources.Exceptions.Tuploids_ParseTemplate,
					"4",
					tokenizer.ToString());

				result = new Quartet<T>(
					tokens[0].Parse<T>(),
					tokens[1].Parse<T>(),
					tokens[2].Parse<T>(),
					tokens[3].Parse<T>());
			}
			return result;
		}
	}

	public struct Quintet<T> : IEnumerable<T>, IEquatable<Quintet<T>>
	{
		public T First { get; private set; }
		public T Second { get; private set; }
		public T Third { get; private set; }
		public T Fourth { get; private set; }
		public T Fifth { get; private set; }

		public Quintet(T first, T second, T third, T fourth, T fifth) : this()
		{
			First = first;
			Second = second;
			Third = third;
			Fourth = fourth;
			Fifth = fifth;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
			yield return Third;
			yield return Fourth;
			yield return Fifth;
		}

		public bool Equals(Quintet<T> other)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			return comparer.Equals(First, other.First) &&
				comparer.Equals(Second, other.Second) &&
				comparer.Equals(Third, other.Third) &&
				comparer.Equals(Fourth, other.Fourth) &&
				comparer.Equals(Fifth, other.Fifth);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Quintet<T> && Equals((Quintet<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;
				int hashCode = comparer.GetHashCode(First);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Second);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Third);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Fourth);
				hashCode = (hashCode*397) ^ comparer.GetHashCode(Fifth);
				return hashCode;
			}
		}

		public static bool operator ==(Quintet<T> left, Quintet<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Quintet<T> left, Quintet<T> right)
		{
			return !left.Equals(right);
		}

		public static Quintet<T> Parse(string quintet, char tokenizer)
		{
			Quintet<T> result = default(Quintet<T>);
			if (quintet.IsNotEmpty())
			{
				var tokens = quintet.Split(tokenizer);

				Guard.AgainstArgument("pair", tokens.Length != 5,
					Resources.Exceptions.Tuploids_ParseTemplate,
					"5",
					tokenizer.ToString());

				result = new Quintet<T>(
					tokens[0].Parse<T>(),
					tokens[1].Parse<T>(),
					tokens[2].Parse<T>(),
					tokens[3].Parse<T>(),
					tokens[5].Parse<T>());
			}
			return result;
		}
	}
}
