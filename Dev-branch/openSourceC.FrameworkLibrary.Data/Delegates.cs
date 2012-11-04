using System;
using System.Data.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbReaderDelegate.
	/// </summary>
	/// <typeparam name="TDataReaderHelper"></typeparam>
	/// <typeparam name="TDbDataReader"></typeparam>
	/// <param name="dataReaderHelper"></param>
	/// <returns></returns>
	public delegate void DbReaderDelegate<TDataReaderHelper, TDbDataReader>(TDataReaderHelper dataReaderHelper)
		where TDataReaderHelper : DataReaderHelper<TDataReaderHelper, TDbDataReader>, new()
		where TDbDataReader : DbDataReader;

	/// <summary>
	///		Summary description for DbReaderDelegate.
	/// </summary>
	/// <typeparam name="TDataReaderHelper"></typeparam>
	/// <typeparam name="TDbDataReader"></typeparam>
	/// <typeparam name="TFillObject"></typeparam>
	/// <param name="dataReaderHelper"></param>
	/// <returns></returns>
	public delegate TFillObject DbReaderDelegate<TDataReaderHelper, TDbDataReader, TFillObject>(TDataReaderHelper dataReaderHelper)
		where TDataReaderHelper : DataReaderHelper<TDataReaderHelper, TDbDataReader>, new()
		where TDbDataReader : DbDataReader;
}
