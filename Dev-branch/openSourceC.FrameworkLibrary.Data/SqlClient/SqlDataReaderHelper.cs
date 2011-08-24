using System;
using System.Data.SqlClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Wraps the <see cref="SqlDataReader"/> class to add support for <see cref="T:Nullable&lt;T&gt;"/>
	///		types and make it easier to use.
	/// </summary>
	public sealed class SqlDataReaderHelper : DataReaderHelper<SqlDataReaderHelper, SqlDataReader>
	{
		#region Public Methods

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;DateTimeOffset&gt;"/>
		///		object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTimeOffset&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTimeOffset? GetDateTimeOffset(string name)
		{
			try { return GetDateTimeOffset(GetOrdinal(name)); }
			catch (Exception) { throw; }

		}

		/// <summary>
		///     Gets the local time value of the specified column as a
		///     <see cref="Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTime? GetDateTimeOffsetAsDateTimeLocal(string name)
		{
			try { return GetDateTimeOffsetAsDateTimeLocal(GetOrdinal(name)); }
			catch (Exception) { throw; }

		}

		/// <summary>
		///     Gets the Coordinated Universal Time (UTC) value of the specified column as a
		///     <see cref="Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTime? GetDateTimeOffsetAsDateTimeUtc(string name)
		{
			try { return GetDateTimeOffsetAsDateTimeUtc(GetOrdinal(name)); }
			catch (Exception) { throw; }

		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="Nullable&lt;DateTimeOffset&gt;"/>
		///		object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTimeOffset&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTimeOffset? GetDateTimeOffset(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTimeOffset?)null : dataReader.GetDateTimeOffset(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the local time value of the specified column as a
		///     <see cref="Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTime? GetDateTimeOffsetAsDateTimeLocal(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : dataReader.GetDateTimeOffset(ordinal).LocalDateTime; }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the Coordinated Universal Time (UTC) value of the specified column as a
		///     <see cref="Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public DateTime? GetDateTimeOffsetAsDateTimeUtc(int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : dataReader.GetDateTimeOffset(ordinal).UtcDateTime; }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		#endregion
	}
}
