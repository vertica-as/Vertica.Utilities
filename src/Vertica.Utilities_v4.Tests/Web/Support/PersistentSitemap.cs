using System;
using System.IO;
using System.Xml;

namespace Vertica.Utilities_v4.Tests.Web.Support
{
	internal class PersistentSitemap : IDisposable
	{
		public static void SelfCleaning(Action<PersistentSitemap> @do)
		{
			using (var m = new PersistentSitemap())
			{
				@do(m);
			}
		}

		public static void SelfCleaning(Action<PersistentSitemap> @do, Action<PersistentSitemap> onDispose)
		{
			using (var m = new PersistentSitemap(onDispose))
			{
				@do(m);
			}
		}

		public PersistentSitemap() : this(_ => {}) { }

		private readonly Action<PersistentSitemap> _onDispose;
		public PersistentSitemap(Action<PersistentSitemap> onDispose)
		{
			_onDispose = onDispose;
		}

		public void Dispose()
		{
			File.Delete(Path);
			_onDispose(this);
		}

		internal string GetXml()
		{
			var dom = new XmlDocument();
			dom.Load(Path);
			string result = dom.OuterXml;
			return result;
		}

		private static readonly string _filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "output.xml");
		internal string Path { get { return _filePath; } }

		internal void CreateEmptyFile()
		{
			StreamWriter sw = File.CreateText(Path);
			sw.Flush();
			sw.Close();
		}
	}
}