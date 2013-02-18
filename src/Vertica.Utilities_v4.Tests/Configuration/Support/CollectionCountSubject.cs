using System.Configuration;
using System.Globalization;
using Vertica.Utilities_v4.Configuration;

namespace Vertica.Utilities_v4.Tests.Configuration.Support
{
	internal class CollectionCountSubject : ConfigurationElementCollection<string, CollectionCountElementSubject>
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMap; }
		}

		protected override string ElementName
		{
			get { return typeof(CollectionCountElementSubject).Name; }
		}

		protected override string GetElementKey(CollectionCountElementSubject elementSubject)
		{
			return elementSubject.Key;
		}

		public static CollectionCountSubject Build(params int[] elementKeys)
		{
			var collection = new CollectionCountSubject();
			if (elementKeys != null)
			{
				foreach (var key in elementKeys)
				{
					collection.Add(new CollectionCountElementSubject { Key = key.ToString(CultureInfo.InvariantCulture) });
				}
			}
			return collection;
		}
	}
}