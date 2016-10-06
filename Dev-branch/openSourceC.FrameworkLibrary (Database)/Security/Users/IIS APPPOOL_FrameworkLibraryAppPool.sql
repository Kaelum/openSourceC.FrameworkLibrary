CREATE USER [IIS APPPOOL\FrameworkLibraryAppPool]
	FOR LOGIN [IIS APPPOOL\FrameworkLibraryAppPool]
	WITH DEFAULT_SCHEMA = [dbo];
GO


--ALTER ROLE [db_owner] ADD MEMBER [BUILTIN\Administrators];
--GO


ALTER ROLE [FrameworkLibraryAccess] ADD MEMBER [IIS APPPOOL\FrameworkLibraryAppPool];
GO
