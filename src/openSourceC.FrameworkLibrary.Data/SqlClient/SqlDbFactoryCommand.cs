﻿using System;
using System.Data.SqlClient;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for SqlFactoryCommand.
	/// </summary>
	public sealed class SqlDbFactoryCommand : DbFactoryCommand<SqlDbFactory, SqlDbFactoryCommand, SqlDbParams, SqlConnection, SqlTransaction, SqlCommand, SqlParameter, SqlDataAdapter, SqlDataReader>
	{ }
}
