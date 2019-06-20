using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for SqlDbFactory.
	/// </summary>
	public sealed class SqlDbFactory : DbFactory<SqlDbFactory, SqlDbFactoryCommand, SqlDbParams, SqlConnection, SqlTransaction, SqlCommand, SqlParameter, SqlDataAdapter, SqlDataReader>
	{
		private const int DEFAULT_CONNECTION_TIMEOUT = 30;

		private byte[] _filestreamTransactionContext;
		private object _filestreamTransactionContextLock = new object();


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
		protected internal override int DefaultConnectionTimeout
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

		#region Private Properties

		/// <summary>
		///		Gets the current transaction context of a session.
		/// </summary>
		private byte[] FilestreamTransactionContext
		{
			get
			{
				if (_filestreamTransactionContext == null)
				{
					lock (_filestreamTransactionContextLock)
					{
						if (_filestreamTransactionContext == null)
						{
							if (!TransactionExists)
							{
								throw new OscErrorException("Not in a transaction");
							}

							using (SqlDbFactoryCommand cmd = CreateTextCommand("SELECT GET_FILESTREAM_TRANSACTION_CONTEXT();"))
							{
								cmd.Transaction = Transaction;

								_filestreamTransactionContext = (byte[])cmd.ExecuteScalar();
							}
						}
					}
				}

				return _filestreamTransactionContext;
			}
		}

		#endregion

		#region Transaction Methods

		/// <summary>
		///		Starts a database transaction with the specified transaction name.
		/// </summary>
		/// <param name="transactionName">The name of the transaction.</param>
		public void BeginTransaction(string transactionName)
		{
			Transaction = Connection.BeginTransaction(transactionName);
		}

		/// <summary>
		///		Starts a database transaction with the specified isolation level and transaction
		///		name.
		/// </summary>
		/// <param name="isolationLevel">One of the <see cref="T:IsolationLevel"/> values.</param>
		/// <param name="transactionName">The name of the transaction.</param>
		public void BeginTransaction(IsolationLevel isolationLevel, string transactionName)
		{
			Transaction = Connection.BeginTransaction(isolationLevel, transactionName);
		}

		/// <summary>
		///		Returns a <see cref="T:SqlFileStream"/> object.
		/// </summary>
		/// <param name="pathName">The logical path to the file. The path can be retrieved by using
		///		the Transact-SQL Pathname function on the underlying FILESTREAM column in the table.</param>
		/// <param name="fileAccess">
		///		The access mode to use when opening the file. Supported
		///		<see cref="T:FileAccess"/> enumeration values are <see cref="T:FileAccess.Read"/>,
		///		<see cref="T:FileAccess.Write"/>, and <see cref="T:FileAccess.ReadWrite"/>.
		///		<para>When using <b>FileAccess.Read</b>, the <b>SqlFileStream</b> object can be used
		///		to read all of the existing data.</para>
		///		<para>When using <b>FileAccess.Write</b>, <b>SqlFileStream</b> points to a zero byte
		///		file. Existing data will be overwritten when the object is closed and the
		///		transaction is committed.</para>
		///		<para>When using <b>FileAccess.ReadWrite</b>, the <b>SqlFileStream</b> points to a
		///		file which has all the existing data in it. The handle is positioned at the
		///		beginning of the file. You can use one of the <b>System.IO</b> Seek methods to move
		///		the handle position within the file to write or append new data.</para>
		///	</param>
		/// <returns>
		///		A <see cref="T:SqlFileStream"/> object.
		/// </returns>
		public SqlFileStream GetFileStream(string pathName, FileAccess fileAccess)
		{
			if (!TransactionExists)
			{
				throw new OscErrorException("Not in a transaction");
			}

			SqlFileStream fileStream = new SqlFileStream(pathName, FilestreamTransactionContext, fileAccess);

			return fileStream;
		}

		/// <summary>
		///		Returns a <see cref="T:SqlFileStream"/> object.
		/// </summary>
		/// <param name="pathName">The logical path to the file. The path can be retrieved by using
		///		the Transact-SQL Pathname function on the underlying FILESTREAM column in the table.</param>
		/// <param name="fileAccess">
		///		The access mode to use when opening the file. Supported
		///		<see cref="T:FileAccess"/> enumeration values are <see cref="T:FileAccess.Read"/>,
		///		<see cref="T:FileAccess.Write"/>, and <see cref="T:FileAccess.ReadWrite"/>.
		///		<para>When using <b>FileAccess.Read</b>, the <b>SqlFileStream</b> object can be used
		///		to read all of the existing data.</para>
		///		<para>When using <b>FileAccess.Write</b>, <b>SqlFileStream</b> points to a zero byte
		///		file. Existing data will be overwritten when the object is closed and the
		///		transaction is committed.</para>
		///		<para>When using <b>FileAccess.ReadWrite</b>, the <b>SqlFileStream</b> points to a
		///		file which has all the existing data in it. The handle is positioned at the
		///		beginning of the file. You can use one of the <b>System.IO</b> Seek methods to move
		///		the handle position within the file to write or append new data.</para>
		///	</param>
		///	<param name="options">Specifies the option to use while opening the file. Supported
		///		<see cref="T:FileOptions"/> values are <see cref="T:FileOptions.Asynchronous"/>,
		///		<see cref="T:FileOptions.WriteThrough"/>, <see cref="T:FileOptions.SequentialScan"/>,
		///		and <see cref="T:FileOptions.RandomAccess"/>.</param>
		///	<param name="allocationSize">The allocation size to use while creating a file. If set to
		///		0, the default value is used.</param>
		/// <returns>
		///		A <see cref="T:SqlFileStream"/> object.
		/// </returns>
		public SqlFileStream GetFileStream(string pathName, FileAccess fileAccess, FileOptions options, long allocationSize)
		{
			if (!TransactionExists)
			{
				throw new OscErrorException("Not in a transaction");
			}

			SqlFileStream fileStream = new SqlFileStream(pathName, FilestreamTransactionContext, fileAccess, options, allocationSize);

			return fileStream;
		}

		#endregion
	}
}
