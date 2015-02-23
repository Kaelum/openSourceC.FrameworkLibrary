CREATE TABLE [Core].[SchemaVersions] (
	[Feature] nvarchar(128) NOT NULL,
	[CompatibleSchemaVersion] nvarchar(128) NOT NULL,
	[IsCurrentVersion] bit NOT NULL,

	CONSTRAINT [PK_Core.SchemaVersions] PRIMARY KEY CLUSTERED (
		[Feature] ASC,
		[CompatibleSchemaVersion] ASC
	),
)
GO
