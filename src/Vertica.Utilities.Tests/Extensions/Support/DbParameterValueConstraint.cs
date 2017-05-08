using System.Data;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Constraint = NUnit.Framework.Constraints.Constraint;

namespace Vertica.Utilities_v4.Tests.Extensions.Support
{
	public class DbParameterValueConstraint : DelegatingConstraint<IDataParameter>
	{
		public DbParameterValueConstraint(Constraint valueConstraint)
		{
			Delegate = Must.Have.Property<IDataParameter>(c => c.Value, valueConstraint);
		}

		protected override bool matches(IDataParameter current)
		{
			return Delegate.Matches(current);
		}
	}

	public class DbOutParameterConstraint : DelegatingConstraint<IDataParameter>
	{
		public DbOutParameterConstraint(DbType type)
		{
			Delegate = Must.Have.Property<IDataParameter>(c => c.Direction, Is.EqualTo(ParameterDirection.Output)) &
				Must.Have.Property<IDataParameter>(c => c.DbType, Is.EqualTo(type));
		}

		protected override bool matches(IDataParameter current)
		{
			return Delegate.Matches(current);
		}
	}

	internal static partial class MustExtensions
	{
		public static DbParameterValueConstraint ConstrainedValue(this Must.HaveEntryPoint entry, Constraint valueConstraint)
		{
			return new DbParameterValueConstraint(valueConstraint);
		}

		public static DbOutParameterConstraint OutParameter(this Must.BeEntryPoint entry, DbType type)
		{
			return new DbOutParameterConstraint(type);
		}
	}
}