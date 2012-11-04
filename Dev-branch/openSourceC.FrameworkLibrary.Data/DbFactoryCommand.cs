//#define USES_UNMANGED_CODE
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryCommand.
	/// </summary>
	/// <typeparam name="TDbFactoryCommand"></typeparam>
	/// <typeparam name="TParamsHelper"></typeparam>
	/// <typeparam name="TDataReaderHelper"></typeparam>
	/// <typeparam name="TDbConnection"></typeparam>
	/// <typeparam name="TDbTransaction"></typeparam>
	/// <typeparam name="TDbCommand">The <see cref="DbCommand"/> type.</typeparam>
	/// <typeparam name="TDbParameter"></typeparam>
	/// <typeparam name="TDataAdapter"></typeparam>
	/// <typeparam name="TDbDataReader"></typeparam>
	public abstract class DbFactoryCommand<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader> : IDisposable
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
		private const int DEFAULT_COMMAND_TIMEOUT = 30;
		private const string DEFAULT_RETURN_VALUE_PARAMETER_NAME = "RETURN_VALUE";

		private TDbCommand _cmd;
		private TParamsHelper _paramsHelper;

		// Track whether Dispose has been called.
		private bool _disposed = false;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		protected DbFactoryCommand() { }

		#endregion

		#region Create

		/// <summary>
		///		Create a new <see cref="T:TDbFactoryCommand"/> object.
		/// </summary>
		/// <param name="commandText">The command string.</param>
		/// <param name="commandType">The type of command in commandText.</param>
		/// <param name="connection">The connection object to use.</param>
		/// <param name="autoPrepare">Indicates that the <b>Execute</b> methods will automatically
		///		call the <see cref="T:TDbCommand.Prepare" /> method before executing.</param>
		///	<returns>
		///		A new <see cref="T:TDbFactoryCommand"/> object.
		///	</returns>
		internal static TDbFactoryCommand Create(string commandText, CommandType commandType, TDbConnection connection, bool autoPrepare)
		{
			TDbFactoryCommand dbFactoryCommand = new TDbFactoryCommand();

			dbFactoryCommand._cmd = (TDbCommand)connection.CreateCommand();

			dbFactoryCommand._cmd.CommandType = commandType;
			dbFactoryCommand._cmd.CommandText = commandText;
			dbFactoryCommand._cmd.CommandTimeout = dbFactoryCommand.DefaultCommandTimeout;
			dbFactoryCommand.AutoPrepare = autoPrepare;

			dbFactoryCommand._paramsHelper = ParamsHelper<TDbFactoryCommand, TParamsHelper, TDataReaderHelper, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDataAdapter, TDbDataReader>.Create(dbFactoryCommand._cmd);
			dbFactoryCommand._paramsHelper.AddInt32(dbFactoryCommand.ReturnValueParameterName).Direction = ParameterDirection.ReturnValue;

			return dbFactoryCommand;
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
					if (_paramsHelper != null)
					{
						// Dispose of the ParamsHelper object.
						_paramsHelper.Dispose();
						_paramsHelper = null;
					}

					if (_cmd != null)
					{
						// Dispose of the command object.
						_cmd.Dispose();
						_cmd = null;
					}

					// Nullify references to managed resources that are not disposable.

					// Nullify references to externally created managed resources.
				}

#if USES_UNMANGED_CODE
				// Dispose of unmanaged resources here.
#endif

				_disposed = true;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets the current command.
		/// </summary>
		public TDbCommand Command
		{
			get { return _cmd; }
		}

		/// <summary>
		///		Gets or sets the text command to run against the data source.
		/// </summary>
		/// <value>
		///		The text command to execute. The default value is an empty string ("").
		/// </value>
		public string CommandText
		{
			get { return _cmd.CommandText; }
			set { _cmd.CommandText = value; }
		}

		/// <summary>
		///		Gets or sets the wait time before terminating the attempt to execute a command and
		///		generating an error.
		/// </summary>
		/// <value>
		///		The time (in seconds) to wait for the command to execute. The default value is 30
		///		seconds.
		/// </value>
		public int CommandTimeout
		{
			get { return _cmd.CommandTimeout; }
			set { _cmd.CommandTimeout = value; }
		}

		/// <summary>
		///		Indicates or specifies how the <see cref="CommandText" /> property is interpreted.
		/// </summary>
		/// <value>
		///		One of the <see cref="CommandType" /> values. The default is <b>Text</b>.
		/// </value>
		public CommandType CommandType
		{
			get { return _cmd.CommandType; }
			set { _cmd.CommandType = value; }
		}

		/// <summary>
		///		Gets or sets the <see cref="DbConnection"/> used by this instance of the
		///		<see cref="IDbCommand" />.
		/// </summary>
		/// <value>
		///		The connection to the data source.
		/// </value>
		public TDbConnection Connection
		{
			get { return (TDbConnection)_cmd.Connection; }
		}

		/// <summary>
		///		Gets the current Params object.
		/// </summary>
		public TParamsHelper Params
		{
			get { return _paramsHelper; }
		}

		/// <summary>
		///		Gets the return value from the last executed command.
		///		<para>NOTE: May not be valid while the data reader is open.</para>
		/// </summary>
		public int? ReturnValue
		{
			get { return Params.GetInt32(ReturnValueParameterName); }
		}

		/// <summary>
		///     Gets the name of the parameter that contains the return value.
		/// </summary>
		public virtual string ReturnValueParameterName
		{
			get { return DEFAULT_RETURN_VALUE_PARAMETER_NAME; }
		}

		/// <summary>
		///		Gets or sets the transaction within which the <b>Command</b> object of a .NET
		///		Framework data provider executes.
		/// </summary>
		/// <value>
		///		The <b>Command</b> object of a .NET Framework data provider executes. The default
		///		value is a null reference (<b>Nothing</b> in Visual Basic).
		/// </value>
		public TDbTransaction Transaction
		{
			get { return (TDbTransaction)_cmd.Transaction; }
			set { _cmd.Transaction = value; }
		}

		/// <summary>
		///		Gets or sets how command results are applied to the <see cref="DataRow" /> when
		///		used by the <see cref="M:DbDataAdapter.Update" /> method of a <see cref="DbDataAdapter" />.
		/// </summary>
		/// <value>
		///		One of the <see cref="UpdateRowSource" /> values.
		/// </value>
		/// <remarks>
		///		The default is <b>Both</b> unless the command is automatically generated. Then the
		///		default is <b>None</b>.
		/// </remarks>
		public UpdateRowSource UpdatedRowSource
		{
			get { return _cmd.UpdatedRowSource; }
			set { _cmd.UpdatedRowSource = value; }
		}

		#endregion

		#region Protected Properties

		/// <summary>
		///		Gets or sets a value indicating that a prepared (or compiled) version of the
		///		command will be created on the data source
		/// </summary>
		protected bool AutoPrepare { get; set; }

		/// <summary>
		///     Gets the default command timeout.
		/// </summary>
		protected virtual int DefaultCommandTimeout
		{
			get { return DEFAULT_COMMAND_TIMEOUT; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		///		Gets a DataTable object from the executed command.
		/// </summary>
		/// <param name="dataSet">DataSet object.  null on error.</param>
		/// <returns>
		///		The number of rows successfully added to or refreshed in the <see cref="DataSet" />.
		///		This does not include rows affected by statements that do not return rows.
		/// </returns>
		public int ExecuteDataSet(DataSet dataSet)
		{
			using (TDataAdapter dataAdapter = new TDataAdapter())
			{
				dataAdapter.SelectCommand = _cmd;

				if (AutoPrepare)
				{
					Command.Prepare();
				}

				return dataAdapter.Fill(dataSet);
			}
		}

		/// <summary>
		///		Executes a Transact-SQL statement against the connection.
		/// </summary>
		/// <returns>
		///		The number of rows affected.
		/// </returns>
		public int ExecuteNonQuery()
		{
			bool isOpened = (_cmd.Connection.State == ConnectionState.Open);

			if (!isOpened)
			{
				_cmd.Connection.Open();
			}

			try
			{
				if (AutoPrepare)
				{
					_cmd.Prepare();
				}

				return _cmd.ExecuteNonQuery();
			}
			finally
			{
				if (!isOpened)
				{
					//_cmd.Connection.Close();
				}
			}
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" />.
		/// </summary>
		///	<returns>
		///		A <see cref="T:TDataReaderHelper"/> object.
		///	</returns>
		public TDataReaderHelper ExecuteReader()
		{
			return ExecuteReader(CommandBehavior.Default);
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" /> using
		///		one of the <see cref="CommandBehavior" /> values.
		/// </summary>
		/// <param name="behavior">One of the <see cref="P:CommandBehavior"/> values.</param>
		///	<returns>
		///		A <see cref="T:TDataReaderHelper"/> object.
		///	</returns>
		public TDataReaderHelper ExecuteReader(CommandBehavior behavior)
		{
			bool isOpened = (_cmd.Connection.State == ConnectionState.Open);

			if (!isOpened)
			{
				_cmd.Connection.Open();
			}

			if (AutoPrepare)
			{
				_cmd.Prepare();
			}

			return DataReaderHelper<TDataReaderHelper, TDbDataReader>.Create((TDbDataReader)_cmd.ExecuteReader(behavior));
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" />.
		/// </summary>
		/// <param name="readerDelegate">The delegate that processes the data reader.</param>
		public void ExecuteReader(DbReaderDelegate<TDataReaderHelper, TDbDataReader> readerDelegate)
		{
			ExecuteReader(CommandBehavior.Default, readerDelegate);
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" /> using
		///		one of the <see cref="CommandBehavior" /> values.
		/// </summary>
		/// <param name="behavior">One of the <see cref="P:CommandBehavior"/> values.</param>
		/// <param name="readerDelegate">The delegate that processes the data reader.</param>
		public void ExecuteReader(CommandBehavior behavior, DbReaderDelegate<TDataReaderHelper, TDbDataReader> readerDelegate)
		{
			bool isOpened = (_cmd.Connection.State == ConnectionState.Open);

			if (!isOpened)
			{
				_cmd.Connection.Open();
			}

			try
			{
				if (AutoPrepare)
				{
					_cmd.Prepare();
				}

				using (TDbDataReader dr = (TDbDataReader)_cmd.ExecuteReader(behavior))
				{
					try
					{
						readerDelegate(DataReaderHelper<TDataReaderHelper, TDbDataReader>.Create(dr));
					}
					finally
					{
						dr.Close();
					}
				}
			}
			finally
			{
				if (!isOpened)
				{
					//_cmd.Connection.Close();
				}
			}
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" />, and
		///		returns a <see cref="T:TFillObject"/>.
		/// </summary>
		/// <typeparam name="TFillObject">The type of the object to return.</typeparam>
		/// <param name="readerDelegate">The delegate that processes the data reader and returns a
		///		<see cref="T:TFillObject"/> object.</param>
		///	<returns>
		///		A <see cref="T:TFillObject"/> object populated by the data reader.
		///	</returns>
		public TFillObject ExecuteReader<TFillObject>(DbReaderDelegate<TDataReaderHelper, TDbDataReader, TFillObject> readerDelegate)
		{
			return ExecuteReader<TFillObject>(CommandBehavior.Default, readerDelegate);
		}

		/// <summary>
		///		Executes the <see cref="CommandText" /> against the <see cref="Connection" /> using
		///		one of the <see cref="CommandBehavior" /> values, and returns a <see cref="T:TFillObject"/>.
		/// </summary>
		/// <typeparam name="TFillObject">The type of the object to return.</typeparam>
		/// <param name="behavior">One of the <see cref="P:CommandBehavior"/> values.</param>
		/// <param name="readerDelegate">The delegate that processes the data reader and returns a
		///		<see cref="T:TFillObject"/> object.</param>
		///	<returns>
		///		A <see cref="T:TFillObject"/> object populated by the data reader.
		///	</returns>
		public TFillObject ExecuteReader<TFillObject>(CommandBehavior behavior, DbReaderDelegate<TDataReaderHelper, TDbDataReader, TFillObject> readerDelegate)
		{
			bool isOpened = (_cmd.Connection.State == ConnectionState.Open);

			if (!isOpened)
			{
				_cmd.Connection.Open();
			}

			try
			{
				if (AutoPrepare)
				{
					_cmd.Prepare();
				}

				using (TDbDataReader dr = (TDbDataReader)_cmd.ExecuteReader(behavior))
				{
					try
					{
						return readerDelegate(DataReaderHelper<TDataReaderHelper, TDbDataReader>.Create(dr));
					}
					finally
					{
						dr.Close();
					}
				}
			}
			finally
			{
				if (!isOpened)
				{
					//_cmd.Connection.Close();
				}
			}
		}

		/// <summary>
		///		Executes the query, and returns the first column of the first row in the result set
		///		returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <returns>
		///		The first column of the first row in the result set, or a null reference
		///		(<b>Nothing</b> in Visual Basic) if the result set is empty.
		///	</returns>
		public object ExecuteScalar()
		{
			bool isOpened = (_cmd.Connection.State == ConnectionState.Open);

			if (!isOpened)
			{
				_cmd.Connection.Open();
			}

			try
			{
				if (AutoPrepare)
				{
					_cmd.Prepare();
				}

				return _cmd.ExecuteScalar();
			}
			finally
			{
				if (!isOpened)
				{
					//_cmd.Connection.Close();
				}
			}
		}

		/// <summary>
		///		Creates a prepared version of the command on an instance of SQL Server.
		/// </summary>
		public void Prepare()
		{
			_cmd.Prepare();
		}

		/// <summary>
		///		Converts the command to a SQL string that can be executed directly against the
		///		database.
		/// </summary>
		///	<returns>
		///		<para>
		///			A SQL execution string for the command.
		///		</para>
		///	</returns>
		public override string ToString()
		{
			return DbHelper.CommandToString(_cmd);
		}

		#endregion
	}
}
