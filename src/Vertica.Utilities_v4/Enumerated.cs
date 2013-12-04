using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Vertica.Utilities_v4.Extensions.StringExt;
using Vertica.Utilities_v4.Resources;

namespace Vertica.Utilities_v4
{
	public interface IEnumerated
	{
		/// <summary>
		/// string reprentation of the enum item
		/// </summary>
		string Name { get; }
	}

	[Serializable]
	public abstract class Enumerated<T> : IEnumerated where T : IEnumerated
	{
		private static readonly EnumeratedRepository<T> _repo = new EnumeratedRepository<T>();

		protected Enumerated(string name)
		{
			Name = name;
			_repo.Add(this);
		}

		public string Name { get; private set; }

		public static IEnumerable<T> Values
		{
			get
			{
				return _repo.FindAll();
			}
		}

		/// <summary>
		/// Parse the enum by enumName
		/// </summary>
		/// <param name="enumName"></param>
		/// <returns></returns>
		public static T Parse(string enumName)
		{
			T value;
			if (!TryParse(enumName, out value))
			{
				ExceptionHelper.ThrowArgumentException("enumName", Exceptions.Enumerated_NotFoundTemplate, enumName);
			}
			return value;
		}

		public static bool TryParse(string enumName, out T value)
		{
			return _repo.TryFind(enumName, out value);
		}

		public static bool operator !=(Enumerated<T> x, Enumerated<T> y)
		{
			return !Equals(x, y);
		}

		public static bool operator ==(Enumerated<T> x, Enumerated<T> y)
		{
			return Equals(x, y);
		}

		protected bool Equals(Enumerated<T> classEnumBase)
		{
			if (classEnumBase == null) return false;
			return GetType() == classEnumBase.GetType() && Equals(Name, classEnumBase.Name);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as Enumerated<T>);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}

	internal class EnumeratedRepository<T> where T : IEnumerated
	{
		private static readonly IDictionary<string, IEnumerated> _inner = new Dictionary<string, IEnumerated>(StringComparer.Ordinal);

		static EnumeratedRepository()
		{
			initializeType(typeof(T));
		}

		public void Add(Enumerated<T> item)
		{
			string name = item.Name;
			if (_inner.ContainsKey(name))
			{
				ExceptionHelper.ThrowArgumentException("enunName", Exceptions.Enumerated_DuplicatedTemplate, name);
			}
			_inner.Add(name, item);
		}

		public bool TryFind(string enumName, out T value)
		{
			Guard.AgainstArgument("enumName", enumName.IsEmpty());

			bool found = _inner.ContainsKey(enumName);
			value = default(T);
			if (found)
			{
				value = (T)_inner[enumName];
			}
			return found;
		}

		private static void initializeType(Type t)
		{
			if (!t.IsSubclassOf(typeof(Enumerated<>))) return;
			FieldInfo[] fis = t.GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
			if (fis.Length > 0)
				fis[0].GetValue(null);

			initializeType(t.BaseType);
		}

		public IEnumerable<T> FindAll()
		{
			return _inner.Values.Cast<T>();
		}
	}
}
