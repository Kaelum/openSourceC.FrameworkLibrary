ALTER DATABASE [$(DatabaseName)]
	ADD FILE (
		NAME = [openSourceC.FrameworkLibrary],
		FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf',
		SIZE = 10240 KB,
		FILEGROWTH = 10240 KB
	) TO FILEGROUP [PRIMARY];
GO


ALTER DATABASE [$(DatabaseName)]
	ADD LOG FILE (
		NAME = [openSourceC.FrameworkLibrary_log],
		FILENAME = '$(DefaultLogPath)$(DatabaseName).ldf',
		SIZE = 10240 KB,
		MAXSIZE = 2097152 MB,
		FILEGROWTH = 10240 KB
	);
GO
