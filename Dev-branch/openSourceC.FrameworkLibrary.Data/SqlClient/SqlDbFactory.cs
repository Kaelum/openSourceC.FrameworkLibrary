using System;
using System.Configuration;
using System.Data.SqlClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for SqlDbFactory.
	/// </summary>
	public sealed class SqlDbFactory : DbFactory<SqlDbFactory, SqlDbFactoryCommand, SqlParamsHelper, SqlDataReaderHelper, SqlConnection, SqlTransaction, SqlCommand, SqlParameter, SqlDataAdapter, SqlDataReader>
	{
		private const int DEFAULT_CONNECTION_TIMEOUT = 30;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string to use.</param>
		public SqlDbFactory(string connectionStringName) : base(connectionStringName) { }

		#endregion

		#region Protected Overrides

		/// <summary>
		///     Gets the default connection timeout.
		/// </summary>
		protected override int DefaultConnectionTimeout
		{
			get { return DEFAULT_CONNECTION_TIMEOUT; }
		}

		/// <summary>
		///		Gets a connection string.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string</param>
		/// <returns></returns>
		protected override ConnectionStringSettings GetConnectionStringSettings(string connectionStringName)
		{
			return ConfigurationManager.ConnectionStrings[connectionStringName];
		}

		#endregion
	}
}
