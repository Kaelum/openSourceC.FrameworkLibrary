-- Enable Change Data Capture for the database
EXEC [sys].[sp_cdc_enable_db];
GO


-- Enable Change Data Capture for a table
EXEC [sys].[sp_cdc_enable_table]
	@source_schema = N'dbo',
	@source_name   = N'MyTable',
	@role_name     = N'cdc_DataAccess',
	@filegroup_name = N'CDC',
	@supports_net_changes = 1;
GO


-- Disable Change Data Capture for a table
EXEC [sys].[sp_cdc_disable_table]
	@source_schema = N'dbo',
	@source_name   = N'MyTable',
	@capture_instance = N'all';
GO


-- Disable Change Data Capture for the database
EXEC [sys].[sp_cdc_disable_db];
GO
