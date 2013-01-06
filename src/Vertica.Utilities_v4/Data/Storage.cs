using System;
using System.Collections;
using System.Web;

namespace Vertica.Utilities_v4.Data
{
	/* based on RhinoCommon's LocalData */
	public static class Storage
	{
		private static readonly IStorage _current = new StorageData();

		public static readonly object LocalStorageKey = new object();

		private class StorageData : IStorage
		{
			[ThreadStatic]
			private static Hashtable _threadHashtable;

			private static Hashtable LocalHashtable
			{
				get
				{
					if (!RunningInWeb)
					{
						return _threadHashtable ??
						(
							_threadHashtable = new Hashtable()
						);
					}
					var webHashtable = HttpContext.Current.Items[LocalStorageKey] as Hashtable;
					if (webHashtable == null)
					{
						HttpContext.Current.Items[LocalStorageKey] = webHashtable = new Hashtable();
					}
					return webHashtable;
				}
			}

			public object this[object key]
			{
				get { return LocalHashtable[key]; }
				set { LocalHashtable[key] = value; }
			}

			public void Clear()
			{
				LocalHashtable.Clear();
			}

			public int Count
			{
				get { return LocalHashtable.Count; }
			}
		}

		/// <summary>
		/// Gets the current data
		/// </summary>
		/// <value>The data.</value>
		public static IStorage Data
		{
			get { return _current; }
		}

		/// <summary>
		/// Gets a value indicating whether running in the web context
		/// </summary>
		/// <value><c>true</c> if [running in web]; otherwise, <c>false</c>.</value>
		public static bool RunningInWeb
		{
			get { return HttpContext.Current != null; }
		}
	}
}