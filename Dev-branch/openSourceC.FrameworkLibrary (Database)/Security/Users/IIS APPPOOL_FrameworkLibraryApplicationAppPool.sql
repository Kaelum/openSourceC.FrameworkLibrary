CREATE USER [IIS APPPOOL\FrameworkLibraryApplicationAppPool]
	FOR LOGIN [IIS APPPOOL\FrameworkLibraryApplicationAppPool]
	WITH DEFAULT_SCHEMA = [dbo];
GO


--ALTER ROLE [db_owner] ADD MEMBER [BUILTIN\Administrators];
--GO


ALTER ROLE [FrameworkLibraryApplicationAccess] ADD MEMBER [IIS APPPOOL\FrameworkLibraryApplicationAppPool];
GO
