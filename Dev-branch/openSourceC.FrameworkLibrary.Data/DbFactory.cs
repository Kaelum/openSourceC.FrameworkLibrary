//#define USES_UNMANGED_CODE
//#define ENABLE_CONNECTION_TIMEOUT
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Transactions;

using openSourceC.FrameworkLibrary.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactory.
	/// </summary>
	/// <typeparam name="TDbFactory"></typeparam>
	/// <typeparam name="TDbFactoryCommand"></typeparam>
	/// <typeparam name="TParamsHelper"></typeparam>
	/// <typeparam name="TDataReaderHelper"></typeparam>
	/// <typeparam name="TDbConnection"></typeparam>
	/// <typeparam name="TDbTransaction"></typeparam>
	/// <typeparam name="TDbCommand">The <see cref="DbCommand"/> type.</typeparam>
	/// <typeparam name="TDbParameter"></typeparam>
	/// <typeparam name="TDataAdapter"></typeparam>
	/// <typeparam name="TDbDataReader"></typeparam>
	public abstract class DbFactory<TDbFactory, TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader> : IDisposable
		where TDbFactory : DbFactory<TDbFactory, TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>
		where TDbFactoryCommand : DbFactoryCommand<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>, new()
		where TParamsHelper : ParamsHelper<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>, new()
		where TDataReaderHelper : DataReaderHelper<TDataReaderHelper, TDbDataReader>, new()
		where TDbConnection : DbConnection
		where TDbTransaction : DbTransaction
		where TDbCommand : DbCommand
		where TDbParameter : DbParameter
		where TDataAdapter : DbDataAdapter, new()
		where TDbDataReader : DbDataReader
	{
		private const int DEFAULT_CONNECTION_TIMEOUT = 30;

		private ConnectionStringSettings _connectionStringSettings;
		private DbProviderFactory _dbProviderFactory;
		private TDbConnection _cn;
#if ENABLE_CONNECTION_TIMEOUT
		private int _connectionTimeout;
#endif

		// Track whether Dispose has been called.
		private bool _disposed = false;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		public DbFactory(string connectionStringName)
		{
			ChangeConnection(connectionStringName);
		}

		#endregion

		#region Dispose

#if USES_UNMANGED_CODE
		/// <summary>
		///     Class finalizer.
		/// </summary>
		~DbManager()
		{
		    // Do not re-create Dispose clean-up code here.
		    // Calling Dispose(false) is optimal in terms of
		    // readability and maintainability.
		    Dispose(false);
		}
#endif

		/// <summary>
		///     Releases all resources used by this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Releases the unmanaged resources used by this instance and optionally
		///     releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///     If true, the method has been called directly or indirectly by a user's code.
		///     Managed and unmanaged resources can be disposed. If false, the method has been
		///     called by the runtime from inside the finalizer and you should not reference
		///     other objects. Only unmanaged resources can be disposed.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose() has already been called.
			if (!_disposed)
			{
				// Check to see if managed resources need to be disposed of.
				if (disposing)
				{
					// Dispose managed resources.
					if (_cn != null)
					{
						// Dispose of the connection object.
						_cn.Dispose();
						_cn = null;
					}

					// Nullify references to managed resources that are not disposable.
					_dbProviderFactory = null;

					// Nullify references to externally created managed resources.
				}

#if USES_UNMANGED_CODE
				// Dispose of unmanaged resources here.
#endif

				_disposed = true;
			}
		}

		#endregion

		#region Connection Properties

		/// <summary>
		///		Gets the connection object of this instance.
		/// </summary>
		protected TDbConnection Connection
		{
			get
			{
				if (_cn == null)
				{
#if ENABLE_CONNECTION_TIMEOUT
					DbConnectionStringBuilder csb = _dbProviderFactory.CreateConnectionStringBuilder();
					csb.ConnectionString = _connectionStringSettings.ConnectionString;
					csb["Connection Timeout"] = _connectionTimeout;
#endif

					_cn = (TDbConnection)_dbProviderFactory.CreateConnection();
#if ENABLE_CONNECTION_TIMEOUT
					_cn.ConnectionString = csb.ConnectionString;
#else
					_cn.ConnectionString = _connectionStringSettings.ConnectionString;
#endif
				}

				return _cn;
			}
		}

		/// <summary>
		///		Gets the connection string.
		/// </summary>
		internal string ConnectionString
		{
			get { return _connectionStringSettings.ConnectionString; }
		}

		/// <summary>
		///		Gets the name of the connection string.
		/// </summary>
		public string ConnectionStringName
		{
			get { return _connectionStringSettings.Name; }
		}

		/// <summary>
		///		Gets the provider name property of the connection string.
		/// </summary>
		internal string ConnectionStringProviderName
		{
			get { return _connectionStringSettings.ProviderName; }
		}

#if ENABLE_CONNECTION_TIMEOUT
		/// <summary>
		///		Gets the connection timeout value.
		/// </summary>
		public int ConnectionTimeout
		{
			get { return _connectionTimeout; }
		}
#endif

		/// <summary>
		///     Gets the default connection timeout.
		/// </summary>
		protected virtual int DefaultConnectionTimeout
		{
			get { return DEFAULT_CONNECTION_TIMEOUT; }
		}

		#endregion

		#region Connection Methods

		/// <summary>
		///		Changes the connection.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string to use.</param>
		public void ChangeConnection(string connectionStringName)
		{
#if ENABLE_CONNECTION_TIMEOUT
			ChangeConnection(connectionStringName, DEFAULT_CONNECTION_TIMEOUT);
		}

		/// <summary>
		///		Changes the connection.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string to use.</param>
		/// <param name="connectionTimeout">The connection timeout to be used.</param>
		public void ChangeConnection(string connectionStringName, int connectionTimeout)
		{
#endif
			if (_cn != null)
			{
				_cn.Dispose();
				_cn = null;
			}

			_connectionStringSettings = GetConnectionStringSettings(connectionStringName);

			if (_connectionStringSettings == null)
			{
				throw new OscErrorException(string.Format("Connection string ({0}) settings not found.", connectionStringName));
			}

#if ENABLE_CONNECTION_TIMEOUT
			_connectionTimeout = connectionTimeout;
#endif

			_dbProviderFactory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName);
		}

		/// <summary>
		///		Gets connection string for the named connection.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection to use.</param>
		/// <returns>
		///		Returns a connection string.
		///	</returns>
		protected abstract ConnectionStringSettings GetConnectionStringSettings(string connectionStringName);

		#endregion

		#region Create Command Methods

		/// <summary>
		///		Create Command object.
		/// </summary>
		/// <param name="commandText">The command string.</param>
		/// <returns></returns>
		public TDbFactoryCommand CreateStoredProcedureCommand(string commandText)
		{
			return DbFactoryCommand<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>.Create(commandText, CommandType.StoredProcedure, Connection, false);
		}

		/// <summary>
		///		Create Command object.
		/// </summary>
		/// <param name="commandText">The command string.</param>
		/// <returns></returns>
		public TDbFactoryCommand CreateTableDirectCommand(string commandText)
		{
			return DbFactoryCommand<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>.Create(commandText, CommandType.TableDirect, Connection, false);
		}

		/// <summary>
		///		Create Command object.
		/// </summary>
		/// <param name="commandText">The command string.</param>
		/// <returns></returns>
		public TDbFactoryCommand CreateTextCommand(string commandText)
		{
			return DbFactoryCommand<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>.Create(commandText, CommandType.Text, Connection, false);
		}

		#endregion
	}
}
