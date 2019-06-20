using System;
using System.Data.OracleClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Wraps the <see cref="OracleDataReader"/> class to add support for <see cref="T:Nullable&lt;T&gt;"/>
	///		types and make it easier to use.
	/// </summary>
	public sealed class OracleDataReaderHelper : DataReaderHelper<OracleDataReaderHelper, OracleDataReader>
	{ }
}
