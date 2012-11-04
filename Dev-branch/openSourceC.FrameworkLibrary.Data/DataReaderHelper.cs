//#define USES_UNMANGED_CODE
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Remoting;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Wraps the <see cref="T:TDbDataReader"/> class to add support for <see cref="T:Nullable&lt;T&gt;"/>
	///		types and make it easier to use.
	/// </summary>
	/// <typeparam name="TDataReaderHelper">The <see cref="T:DataReaderHelper"/> type.</typeparam>
	/// <typeparam name="TDbDataReader">The <see cref="DbDataReader"/> type.</typeparam>
	public abstract class DataReaderHelper<TDataReaderHelper, TDbDataReader> : IDataReaderHelper<TDbDataReader>, IDisposable
		where TDataReaderHelper : DataReaderHelper<TDataReaderHelper, TDbDataReader>, new()
		where TDbDataReader : DbDataReader
	{
		/// <summary>The current <see cref="T:TDbDataReader"/> object.</summary>
		protected internal TDbDataReader dataReader;

		// Track whether Dispose has been called.
		private bool _disposed = false;


		#region Class Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		protected DataReaderHelper() { }

		#endregion

		#region Create

		/// <summary>
		///		Create a new <see cref="T:TDataReaderHelper"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:TDbDataReader"/> object being wrapped.</param>
		/// <returns>
		///		A new <see cref="T:TDataReaderHelper"/> object.
		/// </returns>
		internal static TDataReaderHelper Create(TDbDataReader dataReader)
		{
			TDataReaderHelper dataReaderHelper = new TDataReaderHelper();
			dataReaderHelper.dataReader = dataReader;

			return dataReaderHelper;
		}

		#endregion

		#region Dispose

#if USES_UNMANGED_CODE
		/// <summary>
		///     Class finalizer.
		/// </summary>
		~DataReaderHelper()
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
		protected void Dispose(bool disposing)
		{
			// Check to see if Dispose() has already been called.
			if (!_disposed)
			{
				// Check to see if managed resources need to be disposed of.
				if (disposing)
				{
					// Dispose managed resources.
					if (!dataReader.IsClosed)
					{
						dataReader.Close();
					}

					// Nullify references to managed resources that are not disposable.

					// Nullify references to externally created managed resources.
					dataReader = null;
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
		///		Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <value>The value of the specified column.</value>
		public object this[string name]
		{
			get { return dataReader[name]; }
		}

		/// <summary>
		///		Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <value>The value of the specified column.</value>
		public object this[int ordinal]
		{
			get { return dataReader[ordinal]; }
		}

		/// <summary>
		///		Gets a value that indicates the depth of nesting for the current row.
		/// </summary>
		/// <value>
		///		The depth of nesting for the current row.
		///	</value>
		///	<remarks>
		///		The outermost table has a depth of zero.
		///	</remarks>
		public int Depth
		{
			get { return dataReader.Depth; }
		}

		/// <summary>
		///		Gets the number of columns in the current row.
		/// </summary>
		/// <value>
		///		The number of columns in the current row.
		/// </value>
		/// <remarks>
		///		Includes hidden fields. Use <see cref="VisibleFieldCount"/> to exclude hidden
		///		fields.
		/// </remarks>
		public int FieldCount
		{
			get { return dataReader.FieldCount; }
		}

		/// <summary>
		///		Gets a value that indicates whether the <see cref="T:TDbDataReader"/> contains one or
		///		more rows.
		/// </summary>
		/// <value>
		///		<b>true</b> if the <see cref="T:TDbDataReader"/> contains one or more rows; otherwise
		///		<b>false</b>.
		/// </value>
		public bool HasRows
		{
			get { return dataReader.HasRows; }
		}

		/// <summary>
		///		Gets a value indicating whether the <see cref="T:TDbDataReader"/> is closed.
		/// </summary>
		/// <value>
		///		<b>true</b> if the data reader is closed; otherwise, <b>false</b>.
		/// </value>
		/// <remarks>
		///		<b>IsClosed</b> and <see cref="RecordsAffected"/> are the only properties that you
		///		can call after the <see cref="T:DataReaderHelper"/> is closed.
		///	</remarks>
		public bool IsClosed
		{
			get { return dataReader.IsClosed; }
		}

		/// <summary>
		///		Gets the number of rows changed, inserted, or deleted by execution of the SQL
		///		statement.
		/// </summary>
		/// <value>
		///		The number of rows changed, inserted, or deleted; 0 if no rows were affected or the
		///		statement failed; and -1 for SELECT statements.
		/// </value>
		/// <remarks>
		///		The <b>RecordsAffected</b> property is not set until all rows are read and you
		///		close the <see cref="T:DataReaderHelper"/>.
		/// 
		///		<para><see cref="IsClosed"/> and <b>RecordsAffected</b> are the only properties
		///		that you can call after the <see cref="T:DataReaderHelper"/> is closed.</para>
		/// </remarks>
		public int RecordsAffected
		{
			get { return dataReader.RecordsAffected; }
		}

		/// <summary>
		///		Gets the number of columns in the <see cref="T:TDbDataReader"/> that are not hidden.
		/// </summary>
		/// <value>
		///		The number of fields that are not hidden.
		/// </value>
		/// <remarks>
		///		This value is used to determine how many fields in the <see cref="T:TDbDataReader"/>
		///		are visible. For example, a SELECT on a partial primary key returns the remaining
		///		parts of the key as hidden fields. The hidden fields are always appended behind the
		///		visible fields.
		/// </remarks>
		public int VisibleFieldCount
		{
			get { return dataReader.VisibleFieldCount; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		///		Closes the <see cref="T:TDbDataReader"/> object.
		/// </summary>
		public void Close()
		{
			dataReader.Close();
		}

		/// <summary>
		///		Creates an object that contains all the relevant information required to generate a
		///		proxy used to communicate with a remote object.
		/// </summary>
		/// <param name="requestedType">The <see cref="Type"/> of the object that the new
		///		<see cref="ObjRef"/> will reference.</param>
		///	<value>
		///		Information required to generate a proxy.
		///	</value>
		public ObjRef CreateObjRef(Type requestedType)
		{
			return dataReader.CreateObjRef(requestedType);
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="byte"/> array.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public byte[] GetBinary(string name)
		{
			try { return GetBinary(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="byte"/> array.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public byte[] GetBinary(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? null : (byte[])dataReader.GetValue(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public bool? GetBoolean(string name)
		{
			try { return GetBoolean(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public bool? GetBoolean(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (bool?)null : dataReader.GetBoolean(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public byte? GetByte(string name)
		{
			try { return GetByte(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public byte? GetByte(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (byte?)null : dataReader.GetByte(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Reads a stream of bytes from the specified column, starting at location indicated
		///		by dataIndex, into the buffer, starting at the location indicated by bufferIndex.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <returns>The actual number of bytes read.</returns>
		public long GetBytes(string name, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			try { return GetBytes(GetOrdinal(name), dataOffset, buffer, bufferOffset, length); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <value>
		///		The actual number of bytes read.
		/// </value>
		public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			try { return dataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a single character.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public char? GetChar(string name)
		{
			try { return GetChar(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a single character.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public char? GetChar(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (char?)null : dataReader.GetChar((ordinal)); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Returns a char array from the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of characters to read.</param>
		/// <returns>The actual number of characters read.</returns>
		public long GetChars(string name, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			try { return GetChars(GetOrdinal(name), dataOffset, buffer, bufferOffset, length); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Returns a char array from the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of characters to read.</param>
		/// <returns>The actual number of characters read.</returns>
		public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			try { return dataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///		Gets name of the data type of the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>A string representing the name of the data type.</returns>
		public string GetDataTypeName(string name)
		{
			try { return GetDataTypeName(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		Gets name of the data type of the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>A string representing the name of the data type.</returns>
		public string GetDataTypeName(int ordinal)
		{
			try { return dataReader.GetDataTypeName(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/>
		///		object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column and the <b>DateTimeKind</b> value specified by
		///		the kind parameter.
		///	</returns>
		public DateTime? GetDateTime(string name, DateTimeKind kind)
		{
			try { return GetDateTime(GetOrdinal(name), kind); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/>
		///		object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column and the <b>DateTimeKind</b> value specified by
		///		the kind parameter.
		///	</returns>
		public DateTime? GetDateTime(int ordinal, DateTimeKind kind)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : DateTime.SpecifyKind(dataReader.GetDateTime(ordinal), kind); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/>
		///		object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public decimal? GetDecimal(string name)
		{
			try { return GetDecimal(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/>
		///		object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public decimal? GetDecimal(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (decimal?)null : dataReader.GetDecimal(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a double-precision floating point number.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public double? GetDouble(string name)
		{
			try { return GetDouble(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a double-precision floating point number.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public double? GetDouble(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (double?)null : dataReader.GetDouble(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:TEnum"/> integer.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public TEnum? GetEnum<TEnum>(string name) where TEnum : struct
		{
			try { return GetEnum<TEnum>(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:TEnum"/> integer.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public TEnum? GetEnum<TEnum>(int ordinal) where TEnum : struct
		{
			try { return dataReader.IsDBNull(ordinal) ? (TEnum?)null : (TEnum?)Enum.ToObject(typeof(TEnum), dataReader.GetValue(ordinal)); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the data type of the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The data type of the specified column.</returns>
		public Type GetFieldType(string name)
		{
			try { return GetFieldType(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the data type of the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The data type of the specified column.</returns>
		public Type GetFieldType(int ordinal)
		{
			try { return dataReader.GetFieldType(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public float? GetFloat(string name)
		{
			try { return GetFloat(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public float? GetFloat(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (float?)null : dataReader.GetFloat(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a globally-unique identifier (GUID).
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public Guid? GetGuid(string name)
		{
			try { return GetGuid(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a globally-unique identifier (GUID).
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public Guid? GetGuid(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (Guid?)null : dataReader.GetGuid(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a 16-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public short? GetInt16(string name)
		{
			try { return GetInt16(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 16-bit signed integer.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public short? GetInt16(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (short?)null : dataReader.GetInt16(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a 32-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public int? GetInt32(string name)
		{
			try { return GetInt32(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 32-bit signed integer.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public int? GetInt32(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (int?)null : dataReader.GetInt32(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a 64-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public long? GetInt64(string name)
		{
			try { return GetInt64(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 64-bit signed integer.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public long? GetInt64(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (long?)null : dataReader.GetInt64(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the name of the column, given the zero-based column ordinal.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The name of the specified column.</returns>
		public string GetName(int ordinal)
		{
			return dataReader.GetName(ordinal);
		}

		/// <summary>
		///     Gets the column ordinal given the name of the column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The zero-based column ordinal.</returns>
		public int GetOrdinal(string name)
		{
			try { return dataReader.GetOrdinal(name); }
			catch (IndexOutOfRangeException ex) { throw new IndexOutOfRangeException(string.Format(SR.Unknown_column, name), ex); }
		}

		/// <summary>
		///		Returns a <see cref="DataTable"/> that describes the column metadata of the
		///		<see cref="T:TDbDataReader"/>.
		/// </summary>
		/// <returns>A <see cref="DataTable"/> that describes the column metadata.</returns>
		public DataTable GetSchemaTable()
		{
			return dataReader.GetSchemaTable();
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="String"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public string GetString(string name)
		{
			try { return GetString(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="String"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public string GetString(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? null : dataReader.GetString(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public object GetValue(string name)
		{
			try { return GetValue(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public object GetValue(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? null : dataReader.GetValue(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets all attribute columns in the collection for the current row.
		/// </summary>
		/// <param name="values">An array of <see cref="Object"/> into which to copy the attribute columns.</param>
		/// <returns>The number of instances of <see cref="Object"/> in the array.</returns>
		public int GetValues(object[] values)
		{
			return dataReader.GetValues(values);
		}

		/// <summary>
		///		Gets a value that indicates whether the column contains nonexistent or missing
		///		values.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns></returns>
		public bool IsDBNull(string name)
		{
			try { return IsDBNull(GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		Gets a value that indicates whether the column contains nonexistent or missing
		///		values.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		<b>true</b> if the specified column is equivalent to <see cref="DBNull"/>;
		///		otherwise <b>false</b>.
		///	</returns>
		///	<remarks>
		///		Call this method to check for null column values before calling the typed get
		///		methods (for example, <see cref="GetByte(System.Int32)"/>,
		///		<see cref="GetChar(System.Int32)"/>, and so on) to avoid raising an
		///		error.
		///	</remarks>
		public bool IsDBNull(int ordinal)
		{
			return dataReader.IsDBNull(ordinal);
		}

		/// <summary>
		///		Advances the reader to the next result when reading the results of a batch of
		///		statements.
		/// </summary>
		/// <returns>
		///		<b>true</b> if there are more result sets; otherwise <b>false</b>.
		/// </returns>
		/// <remarks>
		///		This method allows you to process multiple result sets returned when a batch is
		///		submitted to the data provider.
		/// </remarks>
		public bool NextResult()
		{
			return dataReader.NextResult();
		}

		/// <summary>
		///		Advances the reader to the next record in a result set.
		/// </summary>
		/// <returns>
		///		<b>true</b> if there are more rows; otherwise <b>false</b>.
		/// </returns>
		/// <remarks>
		///		The default position of a data reader is before the first record. Therefore, you
		///		must call <b>Read</b> to begin accessing data.
		/// </remarks>
		public bool Read()
		{
			return dataReader.Read();
		}

		#endregion
	}
}
