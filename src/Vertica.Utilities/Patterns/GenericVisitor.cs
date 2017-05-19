using System;
using System.Collections.Generic;

namespace Vertica.Utilities.Patterns
{
	public class GenericVisitor<TBase> : IVisitor<TBase>
	{
		public delegate void VisitDelegate<in TSub>(TSub u) where TSub : TBase;

		readonly Dictionary<RuntimeTypeHandle, object> _delegates = new Dictionary<RuntimeTypeHandle, object>();

		public GenericVisitor<TBase> AddDelegate<TSub>(VisitDelegate<TSub> del) where TSub : TBase
		{
			_delegates.Add(typeof(TSub).TypeHandle, del);
			return this;
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

	public interface IVisitor<in TBase>
	{
		void Visit<TSub>(TSub visitable) where TSub : TBase;
	}

	public interface IVisitable<out TBase>
	{
		void Accept(IVisitor<TBase> visitor);
	}
}