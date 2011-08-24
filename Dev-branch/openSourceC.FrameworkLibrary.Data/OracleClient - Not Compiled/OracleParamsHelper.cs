//#define ALLOW_PREFIX_OVERRIDES
using System;
using System.Data;
using System.Data.OracleClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Adds <see cref="T:Nullable&lt;T&gt;"/> type parameter support to a
	///		<see cref="OracleCommand"/> object to make it easier to use.
	/// </summary>
	public sealed class OracleParamsHelper : ParamsHelper<OracleDbFactoryCommand, OracleParamsHelper, OracleDataReaderHelper, OracleConnection, OracleTransaction, OracleCommand, OracleParameter, OracleDataAdapter, OracleDataReader>
	{
		#region Public Methods

		#region Add Parameters

		#region AddCursor

		/// <summary>
		///		Add a parameter representing an Oracle cursor to the command object with a default
		///		direction of output.
		/// </summary>
		///	<param name="parameterName">The name of the parameter.</param>
		///	<returns>Returns a OracleParameter object.</returns>
		public OracleParameter AddCursor(string parameterName)
		{
			return AddCursor(parameterName, ParameterDirection.Output);
		}

		/// <summary>
		///		Add a parameter representing an Oracle cursor to the command object.
		/// </summary>
		///	<param name="parameterName">The name of the parameter.</param>
		/// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
		///	<returns>Returns a OracleParameter object.</returns>
		public OracleParameter AddCursor(string parameterName, ParameterDirection parameterDirection)
		{
			OracleParameter prm = CreateParameter(parameterName, null) as OracleParameter;
			prm.OracleType = OracleType.Cursor;
			prm.IsNullable = true;
			prm.Direction = ParameterDirection.Output;

			return prm as OracleParameter;
		}

		/// <summary>
		///		Add a parameter representing an Oracle cursor to the command object with a default
		///		direction of output.
		/// </summary>
		///	<param name="parameterName">The name of the parameter.</param>
		/// <param name="sourceColumn">The name of the source column.</param>
		///	<returns>Returns a OracleParameter object.</returns>
		public OracleParameter AddCursor(string parameterName, string sourceColumn)
		{
			return AddCursor(parameterName, sourceColumn, ParameterDirection.Output);
		}

		/// <summary>
		///		Add a parameter representing an Oracle cursor to the command object.
		/// </summary>
		///	<param name="parameterName">The name of the parameter.</param>
		/// <param name="sourceColumn">The name of the source column.</param>
		/// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
		///	<returns>Returns a OracleParameter object.</returns>
		public OracleParameter AddCursor(string parameterName, string sourceColumn, ParameterDirection parameterDirection)
		{
			OracleParameter prm = AddCursor(parameterName) as OracleParameter;
			prm.SourceColumn = sourceColumn;
			prm.SourceColumnNullMapping = true;
			prm.Direction = ParameterDirection.Output;

			return prm as OracleParameter;
		}

		#endregion

		#endregion

		#endregion

		#region Protected Methods

		/// <summary>
		///		Returns the name of a parameter normalized for the command client.
		/// </summary>
		///	<param name="parameterName">The name of the parameter.</param>
		///	<returns>
		///		Returns the name of a parameter normalized for the command client.
		///	</returns>
		protected override string NormalizeParameterName(string parameterName)
		{
#if ALLOW_PREFIX_OVERRIDES
			if (parameterName.Equals("RETURN_VALUE", StringComparison.CurrentCultureIgnoreCase)
				|| parameterName.StartsWith("p_", StringComparison.CurrentCultureIgnoreCase)
				|| parameterName.StartsWith("v_", StringComparison.CurrentCultureIgnoreCase))
			{
				return parameterName;
			}
			else
			{
#endif
				return string.Format("p_{0}", parameterName);
#if ALLOW_PREFIX_OVERRIDES
			}
#endif
		}

		#endregion
	}
}
