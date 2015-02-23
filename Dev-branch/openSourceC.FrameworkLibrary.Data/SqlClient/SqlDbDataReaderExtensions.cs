using System;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Adds extension methods to the <see cref="T:SqlDataReader"/> type.
	/// </summary>
	public static class SqlDbDataReaderExtensions
	{
		#region Public Methods

		/// <summary>
		///     Gets the local time value of the specified column as a
		///     <see cref="T:Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTime? GetDateTimeOffsetAsNullableDateTimeLocal(this SqlDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : dataReader.GetDateTimeOffset(ordinal).LocalDateTime; }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the local time value of the specified column as a
		///     <see cref="T:Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTime? GetDateTimeOffsetAsNullableDateTimeLocal(this SqlDataReader dataReader, string name)
		{
			try { return GetDateTimeOffsetAsNullableDateTimeLocal(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }

		}

		/// <summary>
		///     Gets the Coordinated Universal Time (UTC) value of the specified column as a
		///     <see cref="T:Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTime? GetDateTimeOffsetAsNullableDateTimeUtc(this SqlDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTime?)null : dataReader.GetDateTimeOffset(ordinal).UtcDateTime; }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the Coordinated Universal Time (UTC) value of the specified column as a
		///     <see cref="T:Nullable&lt;DateTime&gt;"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTime&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTime? GetDateTimeOffsetAsNullableDateTimeUtc(this SqlDataReader dataReader, string name)
		{
			try { return GetDateTimeOffsetAsNullableDateTimeUtc(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }

		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:Nullable&lt;DateTimeOffset&gt;"/>
		///		object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;DateTimeOffset&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (DateTimeOffset?)null : dataReader.GetDateTimeOffset(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:Nullable&lt;DateTimeOffset&gt;"/>
		///		object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;DateTimeOffset&gt;"/> object consisting of the time represented
		///		by the value of the specified column.
		///	</returns>
		public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataReader dataReader, string name)
		{
			try { return GetNullableDateTimeOffset(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:Nullable&lt;TimeSpan&gt;"/>
		///		object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		A <see cref="Nullable&lt;TimeSpan&gt;"/> object consisting of the timespan
		///		represented by the value of the specified column.
		///	</returns>
		public static TimeSpan? GetNullableTimeSpan(this SqlDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (TimeSpan?)null : dataReader.GetTimeSpan(ordinal); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as a <see cref="T:Nullable&lt;TimeSpan&gt;"/>
		///		object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		A <see cref="T:Nullable&lt;TimeSpan&gt;"/> object consisting of the timespan
		///		represented by the value of the specified column.
		///	</returns>
		public static TimeSpan? GetNullableTimeSpan(this SqlDataReader dataReader, string name)
		{
			try { return GetNullableTimeSpan(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an <see cref="XElement"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		An <see cref="XElement"/> object that represents the XML contents of the specified
		///		column.
		///	</returns>
		public static XElement GetNullableXElement(this SqlDataReader dataReader, int ordinal)
		{
			try { return dataReader.IsDBNull(ordinal) ? (XElement)null : XElement.Load(dataReader.GetXmlReader(ordinal), LoadOptions.PreserveWhitespace); }
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as an <see cref="XElement"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		An <see cref="XElement"/> object that represents the XML contents of the specified
		///		column.
		///	</returns>
		public static XElement GetNullableXElement(this SqlDataReader dataReader, string name)
		{
			try { return GetNullableXElement(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///     Gets the value of the specified column as an <see cref="XmlElement"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="ordinal">The zero-based column ordinal.</param>
		/// <returns>
		///		An <see cref="XmlElement"/> object that represents the XML contents of the specified
		///		column.
		///	</returns>
		public static XmlElement GetNullableXmlElement(this SqlDataReader dataReader, int ordinal)
		{
			try
			{
				if (dataReader.IsDBNull(ordinal)) { return null; }

				XmlDocument doc = new XmlDocument();
				doc.Load(dataReader.GetXmlReader(ordinal));

				return doc.DocumentElement;
			}
			catch (InvalidCastException ex) { throw new InvalidCastException(string.Format(SR.Type_mismatch_on_column, ordinal, dataReader.GetName(ordinal)), ex); }
		}

		/// <summary>
		///     Gets the value of the specified column as an <see cref="XmlElement"/> object.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		An <see cref="XmlElement"/> object that represents the XML contents of the specified
		///		column.
		///	</returns>
		public static XmlElement GetNullableXmlElement(this SqlDataReader dataReader, string name)
		{
			try { return GetNullableXmlElement(dataReader, dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		/// <summary>
		///		Retrieves data of type XML as an <see cref="T:XmlReader"/>.
		/// </summary>
		/// <param name="dataReader">The <see cref="T:SqlDataReader"/> to extend.</param>
		/// <param name="name">The name of the column.</param>
		/// <returns>
		///		An <see cref="T:XmlReader"/> that contains the XML contents of the specified column.
		/// </returns>
		public static XmlReader GetXmlReader(this SqlDataReader dataReader, string name)
		{
			try { return dataReader.GetXmlReader(dataReader.GetOrdinal(name)); }
			catch (Exception) { throw; }
		}

		#endregion
	}
}
