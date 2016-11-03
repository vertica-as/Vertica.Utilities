using System;
using System.Collections.Generic;
using System.Data;

namespace Vertica.Utilities_v4.Extensions.DataExt
{
	[Obsolete(".NET Standard")]
	/* based on http://www.madprops.org/blog/addwithvalue-via-extension-methods/ and http://www.madprops.org/blog/adding-idbcommand-parameters-with-anonymous-types/ */
	public static class DbCommandExtensions
	{
		[Obsolete(".NET Standard")]
		public static int AddInputParameter<T>(this IDbCommand cmd, string name, T value) where T : class
		{
			var dataParameter = cmd.CreateParameter();
			dataParameter.ParameterName = name;
			dataParameter.Value = (object)value ?? DBNull.Value;
			return cmd.Parameters.Add(dataParameter);
		}

		[Obsolete(".NET Standard")]
		public static int AddInputParameter<T>(this IDbCommand cmd, string name, T? value) where T : struct
		{
			var dataParameter = cmd.CreateParameter();
			dataParameter.ParameterName = name;
			dataParameter.Value = value.HasValue ? (object)value : DBNull.Value;
			return cmd.Parameters.Add(dataParameter);
		}

		[Obsolete(".NET Standard")]
		public static void AddInputParameters<T>(this IDbCommand cmd, T parameters) where T : class
		{
			foreach (var prop in parameters.GetType().GetProperties())
			{
				object val = prop.GetValue(parameters, null);
				var p = cmd.CreateParameter();
				p.ParameterName = prop.Name;
				p.Value = val ?? DBNull.Value;
				cmd.Parameters.Add(p);
			}
		}

		[Obsolete(".NET Standard")]
		public static IDbDataParameter AddOutputParameter(this IDbCommand cmd, string name, DbType dbType)
		{
			var dataParameter = cmd.CreateParameter();
			dataParameter.ParameterName = name;
			dataParameter.DbType = dbType;
			dataParameter.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(dataParameter);
			return dataParameter;
		}

		[Obsolete(".NET Standard")]
		public static void AddOutputParameters<T>(this IDbCommand cmd, T parameters) where T : class
		{
			foreach (var prop in parameters.GetType().GetProperties())
			{
				Type type = prop.PropertyType;
				string name = prop.Name;
				try
				{
					DbType dbType = type.Translate();
					AddOutputParameter(cmd, name, dbType);
				}
				catch (ArgumentException ex)
				{
					string message = string.Format("Cannot convert type for property '{0}'.", name);
					throw new ArgumentException(message, name, ex);
				}
			}
		}

		private static readonly IDictionary<Type, DbType> _map = new Dictionary<Type, DbType>
		{
			{typeof(long), DbType.Int64},
			{typeof(byte[]), DbType.Binary},
			{typeof(bool), DbType.Boolean},
			{typeof(string), DbType.String},
			{typeof(char), DbType.StringFixedLength},
			{typeof(char[]), DbType.StringFixedLength},
			{typeof(DateTime), DbType.DateTime},
			{typeof(DateTimeOffset), DbType.DateTimeOffset},
			{typeof(decimal), DbType.Decimal},
			{typeof(double), DbType.Double},
			{typeof(int), DbType.Int32},
			{typeof(float), DbType.Single},
			{typeof(short), DbType.Int16},
			{typeof(object), DbType.Object},
			{typeof(TimeSpan), DbType.Time},
			{typeof(byte), DbType.Byte},
			{typeof(Guid), DbType.Guid}
		};

		[Obsolete(".NET Standard")]
		public static DbType? TryTranslate(this Type netType)
		{
			DbType found;
			return _map.TryGetValue(netType, out found) ? found : default(DbType?);
		}

		[Obsolete(".NET Standard")]
		public static DbType Translate(this Type netType)
		{
			DbType? tried = netType.TryTranslate();
			if (!tried.HasValue)
			{
				ExceptionHelper.ThrowArgumentException("netType", "Cannot map type '{0}' to any DbType.", netType.Name);
			}
			return tried.Value;
		}
	}

	[Obsolete(".NET Standard")]
	public interface IAddParamStrategy
	{
		int Input { get; }
	}
}
