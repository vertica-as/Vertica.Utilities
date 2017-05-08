using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Testing.Commons;
using Vertica.Utilities_v4.Extensions.DataExt;
using Vertica.Utilities_v4.Tests.Extensions.Support;

namespace Vertica.Utilities_v4.Tests.Extensions
{
	[TestFixture]
	public class DbCommandExtensionsTester
	{
		#region AddInputParameter

		[Test]
		public void AddInputParameter_NotNull_AddedWithValue()
		{
			IDbCommand cmd = new SqlCommand();
			var ex = new Exception();
			cmd.AddInputParameter("arg", ex);

			Assert.That(cmd.Parameters["arg"], Must.Have.ConstrainedValue(Is.SameAs(ex)));
		}

		[Test]
		public void AddInputParameter_Null_AddedAsDbNull()
		{
			IDbCommand cmd = new SqlCommand();
			Exception @null = null;
			cmd.AddInputParameter("null", @null);

			Assert.That(cmd.Parameters["null"], Must.Have.ConstrainedValue(Is.SameAs(DBNull.Value)));
		}

		[Test]
		public void AddInputParameter_Value_AddedWithValue()
		{
			IDbCommand cmd = new SqlCommand();
			int? withValue = 3;
			cmd.AddInputParameter("withValue", withValue);

			Assert.That(cmd.Parameters["withValue"], Must.Have.ConstrainedValue(Is.EqualTo(3)));
		}

		[Test]
		public void AddInputParameter_WithoutValue_AddedAsDBNull()
		{
			IDbCommand cmd = new SqlCommand();
			int? withoutValue = null;
			cmd.AddInputParameter("withoutValue", withoutValue);

			Assert.That(cmd.Parameters["withoutValue"], Must.Have.ConstrainedValue(Is.SameAs(DBNull.Value)));
		}

		[Test]
		public void AddInputParameter_String_AddedWithValue()
		{
			IDbCommand cmd = new SqlCommand();
			string withValue = "value";
			cmd.AddInputParameter("withValue", withValue);

			Assert.That(cmd.Parameters["withValue"], Must.Have.ConstrainedValue(Is.EqualTo("value")));
		}

		[Test]
		public void AddInputParameter_NullString_AddedAsDbNUll()
		{
			IDbCommand cmd = new SqlCommand();
			string @null = null;
			cmd.AddInputParameter("null", @null);

			Assert.That(cmd.Parameters["null"], Must.Have.ConstrainedValue(Is.SameAs(DBNull.Value)));
		}

		[Test]
		public void AddInputParameter_EmptyString_AddedAsEmpty()
		{
			IDbCommand cmd = new SqlCommand();
			string empty = string.Empty;
			cmd.AddInputParameter("empty", empty);

			Assert.That(cmd.Parameters["empty"], Must.Have.ConstrainedValue(Is.Empty));
		}

		#endregion

		#region AddInputParameters

		[Test]
		public void AddInputParameters_NotNullValuesAdded_NullValuesAddedAsDbNull()
		{
			var cmd = new SqlCommand();

			cmd.AddInputParameters(new
			{
				I = 1,
				E = new Exception("msg"),
				S = string.Empty,
				N = (string)null
			});

			Assert.That(cmd.Parameters["I"], Must.Have.ConstrainedValue(Is.EqualTo(1)));
			Assert.That(cmd.Parameters["E"], Must.Have.ConstrainedValue(
				Is.InstanceOf<Exception>().With.Message.EqualTo("msg")));
			Assert.That(cmd.Parameters["S"], Must.Have.ConstrainedValue(Is.Empty));
			Assert.That(cmd.Parameters["N"], Must.Have.ConstrainedValue(Is.SameAs(DBNull.Value)));
		}

		#endregion

		[Test]
		public void AddOutputParameter_DirectionAndType()
		{
			IDbCommand cmd = new SqlCommand();

			cmd.AddOutputParameter("out", DbType.DateTimeOffset);

			Assert.That(cmd.Parameters["out"], Must.Be.OutParameter(DbType.DateTimeOffset));
		}

		[Test]
		public void Translate_TranslatableType_CorrespondingDbType()
		{
			Assert.That(typeof(string).Translate(), Is.EqualTo(DbType.String));
			Assert.That(typeof(long).Translate(), Is.EqualTo(DbType.Int64));
		}

		[Test]
		public void Translate_NotTranslatableType_Exception()
		{
			Assert.That(()=>typeof(Exception).Translate(), Throws.ArgumentException
				.With.Message.StringContaining("Exception"));
		}

		[Test]
		public void TryTranslate_TranslatableType_CorrespondingDbType()
		{
			Assert.That(typeof(string).TryTranslate(), Is.EqualTo(DbType.String));
			Assert.That(typeof(long).TryTranslate(), Is.EqualTo(DbType.Int64));
		}

		[Test]
		public void TryTranslate_NotTranslatableType_Null()
		{
			Assert.That(typeof(Exception).TryTranslate(), Is.Null);
		}

		[Test]
		public void AddOutputParameters_DirectionAndTypeForAllProperties()
		{
			var cmd = new SqlCommand();

			cmd.AddOutputParameters(new
			{
				I = 1,
				D = 2m,
				S = string.Empty,
				N = (string)null
			});

			Assert.That(cmd.Parameters["I"], Must.Be.OutParameter(DbType.Int32));
			Assert.That(cmd.Parameters["D"], Must.Be.OutParameter(DbType.Decimal));
			Assert.That(cmd.Parameters["S"], Must.Be.OutParameter(DbType.String));
			Assert.That(cmd.Parameters["N"], Must.Be.OutParameter(DbType.String));
		}

		[Test]
		public void AddOutputParameters_SomePropertyNotTranslatableToDbType_Exception()
		{
			var cmd = new SqlCommand();

			Assert.That(() => cmd.AddOutputParameters(new { I = 1u }), Throws.ArgumentException
				.With.Message.StringContaining("I")
				.And.With.InnerException.With.Message.StringContaining("UInt32"));
		}
	}
}