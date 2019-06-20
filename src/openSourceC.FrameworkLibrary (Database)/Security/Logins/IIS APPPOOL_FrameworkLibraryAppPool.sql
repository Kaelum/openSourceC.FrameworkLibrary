CREATE LOGIN [IIS APPPOOL\FrameworkLibraryAppPool] FROM WINDOWS WITH
	DEFAULT_DATABASE = [$(DatabaseName)],
	DEFAULT_LANGUAGE = [us_english];
GO


ALTER SERVER ROLE [sysadmin] ADD MEMBER [IIS APPPOOL\FrameworkLibraryAppPool];
GO


GRANT CONNECT TO [IIS APPPOOL\FrameworkLibraryAppPool] AS [dbo];
GO
