using System;
using System.Collections.Generic;

namespace Vertica.Utilities_v4.Patterns
{
	/* based on http://derek-says.blogspot.com/2008/05/implementation-of-visitor-pattern-using.html */
	public class GenericVisitor<TBase>
	{
		public delegate void VisitDelegate<in TSub>(TSub u) where TSub : TBase;

		readonly Dictionary<RuntimeTypeHandle, object> _delegates = new Dictionary<RuntimeTypeHandle, object>();

		public void AddDelegate<TSub>(VisitDelegate<TSub> del) where TSub : TBase
		{
			_delegates.Add(typeof(TSub).TypeHandle, del);
		}

		// excutes the correct registered delegate for the visited class
		public void Visit<TSub>(TSub x) where TSub : TBase
		{
			RuntimeTypeHandle handle = typeof (TSub).TypeHandle;
			if (_delegates.ContainsKey(handle))
			{
				((VisitDelegate<TSub>)_delegates[handle])(x);
			}
		}
	}
}