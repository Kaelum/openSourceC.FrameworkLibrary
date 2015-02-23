CREATE LOGIN [IIS APPPOOL\FrameworkLibraryApplicationAppPool] FROM WINDOWS WITH
	DEFAULT_DATABASE = [FrameworkLibraryApplication],
	DEFAULT_LANGUAGE = [us_english];
GO


ALTER SERVER ROLE [sysadmin] ADD MEMBER [IIS APPPOOL\FrameworkLibraryApplicationAppPool];
GO


GRANT CONNECT TO [IIS APPPOOL\FrameworkLibraryApplicationAppPool] AS [dbo];
GO
