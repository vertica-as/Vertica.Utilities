using System.IO;

namespace Vertica.Utilities_v4.Tests.Web.Support
{
	public class PersistentSitemap
	{
		 public static readonly string FilePath = Path.Combine(Path.GetTempPath(), "output.xml");
	}
}