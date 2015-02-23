//#define USES_UNMANGED_CODE
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Adds extension methods to the <see cref="T:DbDataReaderExtensions"/> type.
	/// </summary>
	public static class DbDataReaderExtensions
	{
		#region New Type Extensions

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:DateTime"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		The value of the specified column with the specified <see cref="T:DateTimeKind"/>.
		///	</returns>
		public static DateTime GetDateTime(this DbDataReader dataReader, int ordinal, DateTimeKind kind)
		{
			try { return DateTime.SpecifyKind(dataReader.GetDateTime(ordinal), kind); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:DateTime"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		The value of the specified column with the specified <see cref="T:DateTimeKind"/>.
		///	</returns>
		public static DateTime GetDateTime(this DbDataReader dataReader, string name, DateTimeKind kind)
		{
			try { return GetDateTime(dataReader, dataReader.GetOrdinal(name), kind); }
			catch (Exception) { throw; }
		}

		#endregion

		#region Get by Name Extensions

		/// <summary>
		///     Gets the value of the specified column as a Boolean.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static bool GetBoolean(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetBoolean(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a byte.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static byte GetByte(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetByte(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Reads a stream of bytes from the specified column, starting at location indicated by
		///     <i>dataOffset</i>, into the buffer, starting at the location indicated by
		///     <i>bufferOffset</i>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <returns>The actual number of bytes read.</returns>
		public static long GetBytes(this DbDataReader dataReader, string name, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			try { return dataReader.GetBytes(dataReader.GetOrdinal(name), dataOffset, buffer, bufferOffset, length); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a single character.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static char GetChar(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetChar(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Reads a stream of characters from the specified column, starting at location
		///     indicated by <i>dataOffset</i>, into the buffer, starting at the location indicated
		///     by <i>bufferOffset</i>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to copy the data.</param>
		/// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
		/// <param name="length">The maximum number of characters to read.</param>
		/// <returns>The actual number of characters read.</returns>
		public static long GetChars(this DbDataReader dataReader, string name, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			try { return dataReader.GetChars(dataReader.GetOrdinal(name), dataOffset, buffer, bufferOffset, length); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Returns a <see cref="T:DbDataReader"/> object for the requested column.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>A <see cref="T:DbDataReader"/> object.</returns>
		public static DbDataReader GetData(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetData(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		Gets name of the data type of the specified column.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>A string representing the name of the data type.</returns>
		public static string GetDataTypeName(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetDataTypeName(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:DateTime"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static DateTime GetDateTime(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetDateTime(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:Decimal"/>
		///		object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static decimal GetDecimal(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetDecimal(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a double-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static double? GetDouble(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetDouble(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the data type of the specified column.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The data type of the specified column.</returns>
		public static Type GetFieldType(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetFieldType(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static float GetFloat(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetFloat(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a globally-unique identifier (GUID).
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static Guid GetGuid(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetGuid(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 16-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static short GetInt16(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetInt16(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 32-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static int GetInt32(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetInt32(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a 64-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static long GetInt64(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetInt64(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="String"/>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static string GetString(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetString(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an instance of <see cref="Object"/>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static object GetValue(this DbDataReader dataReader, string name)
		{
			try { return dataReader.GetValue(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		#endregion

		#region IsDBNull Extensions

		/// <summary>
		///		Gets a value that indicates whether the column contains nonexistent or missing
		///		values.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns></returns>
		public static bool IsDBNull(this DbDataReader dataReader, string name)
		{
			try { return dataReader.IsDBNull(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		Gets a value that indicates whether the column contains nonexistent or missing
		///		values.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns></returns>
		public static Task<bool> IsDBNullAsync(this DbDataReader dataReader, string name)
		{
			try { return dataReader.IsDBNullAsync(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		An asynchronous version of <see cref="M:IsDBNull"/>, which gets a value that
		///		indicates whether the column contains non-existent or missing values. Optionally,
		///		sends a notification that operations should be cancelled.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <param name="cancellationToken">The cancellation instruction, which propagates a
		///		notification that operations should be canceled. This does not guarantee the
		///		cancellation. A setting of <b>CancellationToken.None</b> makes this method
		///		equivalent to <see cref="M:IsDBNullAsync(string)"/> . The returned task must be marked as cancelled.</param>
		/// <returns></returns>
		public static Task<bool> IsDBNullAsync(this DbDataReader dataReader, string name, CancellationToken cancellationToken)
		{
			try { return dataReader.IsDBNullAsync(dataReader.GetOrdinal(name), cancellationToken); }
			catch (Exception) { throw; }
		}

		#endregion

		#region DBNull Aware Extensions

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a
		///     <see cref="T:byte"/> array.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static byte[] GetNullableBinary(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? null : (byte[])dataReader.GetValue(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a
		///     <see cref="T:byte"/> array.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static byte[] GetNullableBinary(this DbDataReader dataReader, string name)
		{
			Nullable<int> myInt1 = 0;
			int? myInt2 = myInt1;

			try { return GetNullableBinary(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     Boolean.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static bool? GetNullableBoolean(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (bool?)null : dataReader.GetBoolean(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     Boolean.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static bool? GetNullableBoolean(this DbDataReader dataReader, string name)
		{
			try { return GetNullableBoolean(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable byte.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static byte? GetNullableByte(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (byte?)null : dataReader.GetByte(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable byte.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static byte? GetNullableByte(this DbDataReader dataReader, string name)
		{
			try { return GetNullableByte(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a single
		///     nullable character.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static char? GetNullableChar(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (char?)null : dataReader.GetChar((ordinal)); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a single
		///     nullable character.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static char? GetNullableChar(this DbDataReader dataReader, string name)
		{
			try { return GetNullableChar(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     <see cref="T:DateTime"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column and the <b>DateTimeKind</b> value specified by
		///		the kind parameter.
		///	</returns>
		public static DateTime? GetNullableDateTime(this DbDataReader dataReader, int ordinal, DateTimeKind kind)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : DateTime.SpecifyKind(dataReader.GetDateTime(ordinal), kind); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     <see cref="T:DateTime"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <param name="kind">One of the <see cref="DateTimeKind"/> values.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column and the <b>DateTimeKind</b> value specified by
		///		the kind parameter.
		///	</returns>
		public static DateTime? GetNullableDateTime(this DbDataReader dataReader, string name, DateTimeKind kind)
		{
			try { return GetNullableDateTime(dataReader, dataReader.GetOrdinal(name), kind); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     <see cref="T:Decimal"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static decimal? GetNullableDecimal(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (decimal?)null : dataReader.GetDecimal(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     <see cref="T:Decimal"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static decimal? GetNullableDecimal(this DbDataReader dataReader, string name)
		{
			try { return GetNullableDecimal(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     double-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static double? GetNullableDouble(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (double?)null : dataReader.GetDouble(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     double-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static double? GetNullableDouble(this DbDataReader dataReader, string name)
		{
			try { return GetNullableDouble(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     enumerator.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static TEnum? GetNullableEnum<TEnum>(this DbDataReader dataReader, int ordinal) where TEnum : struct
		{
			try { return dataReader.IsDBNull(ordinal) ? (TEnum?)null : (TEnum?)Enum.ToObject(typeof(TEnum), dataReader.GetValue(ordinal)); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     enumerator.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <typeparam name="TEnum">The enum type to return.</typeparam>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static TEnum? GetNullableEnum<TEnum>(this DbDataReader dataReader, string name) where TEnum : struct
		{
			try { return GetNullableEnum<TEnum>(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     single-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static float? GetNullableFloat(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (float?)null : dataReader.GetFloat(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     single-precision floating point number.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static float? GetNullableFloat(this DbDataReader dataReader, string name)
		{
			try { return GetNullableFloat(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     globally-unique identifier (GUID).
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static Guid? GetNullableGuid(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (Guid?)null : dataReader.GetGuid(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     globally-unique identifier (GUID).
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static Guid? GetNullableGuid(this DbDataReader dataReader, string name)
		{
			try { return GetNullableGuid(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     16-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static short? GetNullableInt16(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (short?)null : dataReader.GetInt16(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     16-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static short? GetNullableInt16(this DbDataReader dataReader, string name)
		{
			try { return GetNullableInt16(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     32-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static int? GetNullableInt32(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (int?)null : dataReader.GetInt32(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     32-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static int? GetNullableInt32(this DbDataReader dataReader, string name)
		{
			try { return GetNullableInt32(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     64-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static long? GetNullableInt64(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (long?)null : dataReader.GetInt64(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as a nullable
		///     64-bit signed integer.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static long? GetNullableInt64(this DbDataReader dataReader, string name)
		{
			try { return GetNullableInt64(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as an instance of
		///     <see cref="String"/>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>The value of the specified column.</returns>
		public static string GetNullableString(this DbDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? null : dataReader.GetString(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets a <see cref="T:DBNull"/> aware value of the specified column as an instance of
		///     <see cref="String"/>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:DbDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>The value of the specified column.</returns>
		public static string GetNullableString(this DbDataReader dataReader, string name)
		{
			try { return GetNullableString(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		#endregion
	}
}
