using System;
using System.Data.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbReaderDelegate.
	/// </summary>
	/// <typeparam name="TDbDataReader"></typeparam>
	/// <param name="dataReader"></param>
	/// <returns></returns>
	public delegate void DbReaderDelegate<TDbDataReader>(TDbDataReader dataReader)
		where TDbDataReader : DbDataReader;

	/// <summary>
	///		Summary description for DbReaderDelegate.
	/// </summary>
	/// <typeparam name="TDbDataReader"></typeparam>
	/// <typeparam name="TFillObject"></typeparam>
	/// <param name="dataReader"></param>
	/// <returns></returns>
	public delegate TFillObject DbReaderDelegate<TDbDataReader, TFillObject>(TDbDataReader dataReader)
		where TDbDataReader : DbDataReader;
}
