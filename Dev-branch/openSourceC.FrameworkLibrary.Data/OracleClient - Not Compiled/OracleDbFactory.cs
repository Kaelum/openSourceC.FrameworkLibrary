using System;
using System.Configuration;
using System.Data.OracleClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for OracleDbFactory.
	/// </summary>
	public sealed class OracleDbFactory : DbFactory<OracleDbFactory, OracleDbFactoryCommand, OracleParamsHelper, OracleDataReaderHelper, OracleConnection, OracleTransaction, OracleCommand, OracleParameter, OracleDataAdapter, OracleDataReader>
	{
		private const int DEFAULT_CONNECTION_TIMEOUT = 30;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string to use.</param>
		public OracleDbFactory(string connectionStringName) : base(connectionStringName) { }

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
