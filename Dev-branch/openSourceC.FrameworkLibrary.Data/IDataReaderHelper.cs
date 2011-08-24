using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Remoting;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		This is a common interface for wrapping <typeparamref name="TDbDataReader"/> based classes to add
	///		support for <see cref="T:Nullable&lt;T&gt;"/> types and make them easier to use.
	/// </summary>
	/// <typeparam name="TDbDataReader">A <see cref="DbDataReader"/> based object.</typeparam>
	public interface IDataReaderHelper<TDbDataReader>
		where TDbDataReader : DbDataReader
	{
		#region Public Properties

		/// <summary>
		///		Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <value>The value of the specified column.</value>
		object this[string name] { get; }

		/// <summary>
		///		Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <value>The value of the specified column.</value>
		object this[int i] { get; }

		/// <summary>
		///		Gets a value that indicates the depth of nesting for the current row.
		/// </summary>
		/// <value>
		///		The depth of nesting for the current row.
		///	</value>
		///	<remarks>
		///		The outermost table has a depth of zero.
		///	</remarks>
		int Depth { get; }

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
		int FieldCount { get; }

		/// <summary>
		///		Gets a value that indicates whether the <see cref="DbDataReader"/> contains one or
		///		more rows.
		/// </summary>
		/// <value>
		///		<b>true</b> if the <see cref="DbDataReader"/> contains one or more rows; otherwise
		///		<b>false</b>.
		/// </value>
		bool HasRows { get; }

		/// <summary>
		///		Gets a value indicating whether the <see cref="DbDataReader"/> is closed.
		/// </summary>
		/// <value>
		///		<b>true</b> if the data reader is closed; otherwise, <b>false</b>.
		/// </value>
		/// <remarks>
		///		<b>IsClosed</b> and <see cref="RecordsAffected"/> are the only properties that you
		///		can call after the <see cref="T:DataReaderHelper"/> is closed.
		///	</remarks>
		bool IsClosed { get; }

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
		///		that you can call after the <see cref="T:DataReaderHelper"/> is
		///		closed.</para>
		/// </remarks>
		int RecordsAffected { get; }

		/// <summary>
		///		Gets the number of columns in the <typeparamref name="TDbDataReader"/> that are not hidden.
		/// </summary>
		/// <value>
		///		The number of fields that are not hidden.
		/// </value>
		/// <remarks>
		///		This value is used to determine how many fields in the <see cref="DbDataReader"/>
		///		are visible. For example, a SELECT on a partial primary key returns the remaining
		///		parts of the key as hidden fields. The hidden fields are always appended behind the
		///		visible fields.
		/// </remarks>
		int VisibleFieldCount { get; }

		#endregion

		#region Public Methods

		/// <summary>
		///		Closes the <see cref="DbDataReader"/> object.
		/// </summary>
		void Close();

		/// <summary>
		///		Creates an object that contains all the relevant information required to generate a
		///		proxy used to communicate with a remote object.
		/// </summary>
		/// <param name="requestedType">The <see cref="Type"/> of the object that the new
		///		<see cref="ObjRef"/> will reference.</param>
		///	<value>
		///		Information required to generate a proxy.
		///	</value>
		ObjRef CreateObjRef(Type requestedType);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="byte"/> array.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		byte[] GetBinary(string name);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="byte"/> array.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		byte[] GetBinary(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		bool? GetBoolean(string name);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		bool? GetBoolean(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		byte? GetByte(string name);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		byte? GetByte(int ordinal);

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
		long GetBytes(string name, long dataOffset, byte[] buffer, int bufferOffset, int length);

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
		long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length);

		/// <summary>
		///     Gets the value of the specified column as a single character.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		char? GetChar(string name);

		/// <summary>
		///     Gets the value of the specified column as a single character.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		char? GetChar(int ordinal);

		/// <summary>
		///     Returns a char array from the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of characters to read.</param>
		/// <returns>The actual number of characters read.</returns>
		long GetChars(string name, long dataOffset, char[] buffer, int bufferOffset, int length);

		/// <summary>
		///     Returns a char array from the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of characters to read.</param>
		/// <returns>The actual number of characters read.</returns>
		long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length);

		/// <summary>
		///		Gets name of the data type of the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>A string representing the name of the data type.</returns>
		string GetDataTypeName(string name);

		/// <summary>
		///		Gets name of the data type of the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>A string representing the name of the data type.</returns>
		string GetDataTypeName(int ordinal);

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
		DateTime? GetDateTime(string name, DateTimeKind kind);

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
		DateTime? GetDateTime(int ordinal, DateTimeKind kind);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/>
		///		object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		decimal? GetDecimal(string name);

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/>
		///		object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		decimal? GetDecimal(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a double-precision floating point number.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		double? GetDouble(string name);

		/// <summary>
		///     Gets the value of the specified column as a double-precision floating point number.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		double? GetDouble(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a <typeparamref name="TEnum"/> integer.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		TEnum? GetEnum<TEnum>(string name) where TEnum : struct;

		/// <summary>
		///     Gets the value of the specified column as a <typeparamref name="TEnum"/> integer.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		TEnum? GetEnum<TEnum>(int i) where TEnum : struct;

		/// <summary>
		///     Gets the data type of the specified column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The data type of the specified column.</returns>
		Type GetFieldType(string name);

		/// <summary>
		///     Gets the data type of the specified column.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The data type of the specified column.</returns>
		Type GetFieldType(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		float? GetFloat(string name);

		/// <summary>
		///     Gets the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		float? GetFloat(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a globally-unique identifier (GUID).
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		Guid? GetGuid(string name);

		/// <summary>
		///     Gets the value of the specified column as a globally-unique identifier (GUID).
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		Guid? GetGuid(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as a 16-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		short? GetInt16(string name);

		/// <summary>
		///     Gets the value of the specified column as a 16-bit signed integer.
		/// </summary>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		short? GetInt16(int i);

		/// <summary>
		///     Gets the value of the specified column as a 32-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		int? GetInt32(string name);

		/// <summary>
		///     Gets the value of the specified column as a 32-bit signed integer.
		/// </summary>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		int? GetInt32(int i);

		/// <summary>
		///     Gets the value of the specified column as a 64-bit signed integer.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		long? GetInt64(string name);

		/// <summary>
		///     Gets the value of the specified column as a 64-bit signed integer.
		/// </summary>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		long? GetInt64(int i);

		/// <summary>
		///     Gets the name of the column, given the zero-based column ordinal.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The name of the specified column.</returns>
		string GetName(int ordinal);

		/// <summary>
		///     Gets the column ordinal given the name of the column.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The zero-based column ordinal.</returns>
		int GetOrdinal(string name);

		/// <summary>
		///		Returns a <see cref="DataTable"/> that describes the column metadata of the
		///		<see cref="DbDataReader"/>.
		/// </summary>
		/// <returns>A <see cref="DataTable"/> that describes the column metadata.</returns>
		DataTable GetSchemaTable();

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="String"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		string GetString(string name);

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="String"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		string GetString(int ordinal);

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		object GetValue(string name);

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		object GetValue(int ordinal);

		/// <summary>
		///     Gets all attribute columns in the collection for the current row.
		/// </summary>
		/// <param name="values">An array of <see cref="Object"/> into which to copy the attribute columns.</param>
		/// <returns>The number of instances of <see cref="Object"/> in the array.</returns>
		int GetValues(object[] values);

		/// <summary>
		///		Gets a value that indicates whether the column contains nonexistent or missing
		///		values.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns></returns>
		bool IsDBNull(string name);

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
		///		<see cref="DbDataReader.GetChar(System.Int32)"/>, and so on) to avoid raising an
		///		error.
		///	</remarks>
		bool IsDBNull(int ordinal);

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
		bool NextResult();

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
		bool Read();

		#endregion
	}
}
