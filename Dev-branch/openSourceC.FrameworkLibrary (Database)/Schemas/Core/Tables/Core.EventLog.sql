CREATE TABLE [Core].[EventLog] (
	[EventLogId] uniqueidentifier CONSTRAINT [DF_Core.EventLog_ROWID] DEFAULT (NEWSEQUENTIALID()) ROWGUIDCOL NOT NULL,
	[ApplicationId] uniqueidentifier NOT NULL,
	[LogName] nvarchar(256) NOT NULL,
	[Source] nvarchar(256) NOT NULL,
	[CreateDate] datetimeoffset CONSTRAINT [DF_Core.EventLog_CreateDate] DEFAULT (SYSDATETIMEOFFSET()) NOT NULL,
	[EventId] int NOT NULL,
	[CategoryId] smallint NOT NULL,
	[EntryType] nvarchar(256) NOT NULL, -- System.Diagnostics.EventLogEntryType Enumeration
	[Message] nvarchar(max) NOT NULL,
	[Data] varbinary(max) NULL,
	[MachineName] nvarchar(256) NOT NULL,
	[OSVersion] nvarchar(32) NOT NULL,
	[ApplicationName] nvarchar(128) NOT NULL,
	[ApplicationVersion] nvarchar(32) NOT NULL,
	[WindowsIdentityName] nvarchar(256) NULL,
	[ClientIPAddress] nvarchar(15) NULL,
	[RequestUrl] nvarchar(2048) NULL,
	[RequestData] varbinary(max) NULL,
	[UserId] nvarchar(128) NULL,
	[HelpLink] nvarchar(256) NULL,

	CONSTRAINT [PK_Core.EventLog] PRIMARY KEY CLUSTERED (
		[EventLogId] DESC
	),

	CONSTRAINT [UC_Core.EventLog_CreateDate_EventLogId] UNIQUE NONCLUSTERED (
		[CreateDate] DESC,
		[EventLogId] DESC,
		[ApplicationId] ASC
	),

	CONSTRAINT [UC_Core.EventLog_MachineName_CreateDate_EventLogId] UNIQUE NONCLUSTERED (
		[MachineName] ASC,
		[CreateDate] DESC,
		[EventLogId] DESC,
		[ApplicationId] ASC
	),

	CONSTRAINT [UC_Core.EventLog_UserId_CreateDate_EventLogId] UNIQUE NONCLUSTERED (
		[UserId] ASC,
		[CreateDate] DESC,
		[EventLogId] DESC,
		[ApplicationId] ASC
	),

	CONSTRAINT [UC_Core.EventLog_WindowsIdentityName_CreateDate_EventLogId] UNIQUE NONCLUSTERED (
		[WindowsIdentityName] ASC,
		[CreateDate] DESC,
		[EventLogId] DESC,
		[ApplicationId] ASC
	),
)
GO
