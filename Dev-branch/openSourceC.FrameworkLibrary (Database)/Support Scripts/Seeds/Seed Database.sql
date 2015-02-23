EXECUTE sp_changedbowner 'sa';
GO


/*
SELECT TOP(100) *
FROM [dbo].[AspNetUsers]
ORDER BY [UserName]
FOR XML RAW, ROOT('AspNetUsers'), TYPE
*/


PRINT 'Executing Seed Database script';


DECLARE @systemId uniqueidentifier;
DECLARE @administratorId uniqueidentifier;
DECLARE @xml xml;


IF @@TRANCOUNT > 0
BEGIN
	PRINT 'Transaction count: ' + CAST(@@TRANCOUNT AS varchar);
	PRINT '*** ABORTING ***'
	RETURN;
END;


BEGIN TRY
	PRINT '';
	PRINT '';
	PRINT 'Update AspNetUsers - BEGIN';

	SET @xml = '<AspNetUsers>
		<row Id="ab246659-8d97-43ba-afa8-8d809bd22d63" UserName="System" PasswordHash="APuNBtOAGuwSe2hyHfj1sisotleDmAmR0iUFw1eIXh3ox/NObfe4LtQpgLV5L8Cxkw==" SecurityStamp="d62e0d53-8ff3-4824-a4fa-4ebbfadc845a" Discriminator="SystemUser" />
		<row Id="31bdebe3-1134-40ab-a0d2-526522cd0fcc" UserName="Administrator" PasswordHash="AMKxhSraSzLD/TUdBXrV7GTvBcNYYS0Mo32E8flcOPIAWTbwZl3O5nq7BLWckCIF8w==" SecurityStamp="90a81d11-4ee2-47b7-ab97-2481b41b3630" Discriminator="ApplicationUser" />
	</AspNetUsers>'

	DECLARE @aspNetUsersTable TABLE (
		[Id] nvarchar(128) NOT NULL,
		[UserName] nvarchar(128) NULL,
		[PasswordHash] nvarchar(MAX) NULL,
		[SecurityStamp] nvarchar(MAX) NULL,
		[Discriminator] nvarchar(128) NOT NULL,

		PRIMARY KEY CLUSTERED (
			[Id] ASC
		),

		UNIQUE NONCLUSTERED (
			[UserName] ASC,
			[Discriminator] ASC
		)
	);

	INSERT INTO @aspNetUsersTable (
		[Id],
		[UserName],
		[PasswordHash],
		[SecurityStamp],
		[Discriminator]
	)
	SELECT
		x.value('@Id', 'uniqueidentifier') [Id],
		x.value('@UserName', 'nvarchar(128)') [UserName],
		x.value('@PasswordHash', 'nvarchar(max)') [PasswordHash],
		x.value('@SecurityStamp', 'nvarchar(max)') [SecurityStamp],
		x.value('@Discriminator', 'nvarchar(128)') [Discriminator]
	FROM
		@xml.nodes('/AspNetUsers/row') u(x);


	BEGIN TRANSACTION;


	UPDATE [dbo].[AspNetUsers]
	SET
		[UserName] = tt.[UserName],
		[PasswordHash] = tt.[PasswordHash],
		[SecurityStamp] = tt.[SecurityStamp],
		[Discriminator] = tt.[Discriminator]
	FROM
		@aspNetUsersTable tt
		JOIN [dbo].[AspNetUsers] anu ON anu.[Id] = tt.[Id]
	WHERE
		anu.[UserName] != tt.[UserName]
		OR anu.[PasswordHash] != tt.[PasswordHash]
		OR anu.[SecurityStamp] != tt.[SecurityStamp]
		OR anu.[Discriminator] != tt.[Discriminator];


	INSERT INTO [dbo].[AspNetUsers] (
		[Id],
		[UserName],
		[PasswordHash],
		[SecurityStamp],
		[Discriminator]
	)
	SELECT
		tt.[Id],
		tt.[UserName],
		tt.[PasswordHash],
		tt.[SecurityStamp],
		tt.[Discriminator]
	FROM
		@aspNetUsersTable tt
		LEFT JOIN [dbo].[AspNetUsers] anu ON anu.[Id] = tt.[Id]
	WHERE
		anu.[Id] IS NULL;

	COMMIT TRANSACTION;

	PRINT 'Update AspNetUsers - END';
END TRY
BEGIN CATCH
	PRINT 'A failure occured while updating AspNetUsers.';

	IF @@TRANCOUNT > 0
	BEGIN
		PRINT '*** ROLLING BACK TRANSACTION ***'

		ROLLBACK TRANSACTION;
	END;

	PRINT '*** ABORTING ***'
	RETURN;
END CATCH;


SELECT @systemId = [Id] FROM @aspNetUsersTable WHERE [UserName] = 'System';

IF @@ROWCOUNT != 1
BEGIN
	PRINT 'Unable to find System account.'
	PRINT '*** ABORTING ***'
	RETURN;
END;


SELECT @administratorId  = [Id] FROM @aspNetUsersTable WHERE [UserName] = 'Administrator';

IF @@ROWCOUNT != 1
BEGIN
	PRINT 'Unable to find Administrator account.'
	PRINT '*** ABORTING ***'
	RETURN;
END;



BEGIN TRY
	PRINT '';
	PRINT '';
	PRINT 'Update Category - BEGIN';

	SET @xml = '<Category>
		<row CategoryId="05D11090-5642-E311-8FC2-FC4DD44B34BB" Name="Driver License" Description="Driver licenses by state" />
	</Category>'

	DECLARE @categoryTable TABLE (
		[CategoryId] uniqueidentifier DEFAULT (NEWSEQUENTIALID()) ROWGUIDCOL NOT NULL,
		[Name] varchar(64) NOT NULL,
		[Description] varchar(256) NULL

		PRIMARY KEY CLUSTERED (
			[CategoryId] ASC
		),

		UNIQUE (
			[Name] ASC
		)
	);

	INSERT INTO @categoryTable (
		[CategoryId],
		[Name],
		[Description]
	)
	SELECT
		x.value('@CategoryId', 'uniqueidentifier') [CategoryId],
		x.value('@Name', 'nvarchar(64)') [Name],
		x.value('@Description', 'nvarchar(256)') [Description]
	FROM
		@xml.nodes('/Category/row') u(x);


	BEGIN TRANSACTION;


	UPDATE [Image].[Category]
	SET
		[Name] = tt.[Name],
		[Description] = tt.[Description]
	FROM
		@categoryTable tt
		JOIN [Image].[Category] c ON c.[CategoryId] = tt.[CategoryId]
	WHERE
		c.[CategoryId] != tt.[CategoryId]
		OR c.[Name] != tt.[Name]
		OR [Image].[NullableEquals](c.[Description], tt.[Description]) = 0;


	INSERT INTO [Image].[Category] (
		[CategoryId],
		[Name],
		[Description],
		[ModifiedBy]
	)
	SELECT
		tt.[CategoryId],
		tt.[Name],
		tt.[Description],
		@systemId [ModifiedBy]
	FROM
		@categoryTable tt
		LEFT JOIN [Image].[Category] c ON c.[CategoryId] = tt.[CategoryId]
	WHERE
		c.[CategoryId] IS NULL;

	COMMIT TRANSACTION;

	PRINT 'Update Category - END';
END TRY
BEGIN CATCH
	PRINT 'A failure occured while updating Category.';

	IF @@TRANCOUNT > 0
	BEGIN
		PRINT '*** ROLLING BACK TRANSACTION ***'

		ROLLBACK TRANSACTION;
	END;

	PRINT '*** ABORTING ***'
	RETURN;
END CATCH;



BEGIN TRY
	PRINT '';
	PRINT '';
	PRINT 'Update Subcategory - BEGIN';

	SET @xml = '<Subcategory>
		<row CategoryId="05D11090-5642-E311-8FC2-FC4DD44B34BB" SubcategoryId="D5D11692-6442-E311-8FC2-FC4DD44B34BB" Name="US-CA" Description="United States - California" ModifiedBy="ab246659-8d97-43ba-afa8-8d809bd22d63" ModifyDate="2013-10-31T12:42:27.6378709-07:00" />
	</Subcategory>';

	DECLARE @subcategoryTable TABLE (
		[CategoryId] uniqueidentifier NOT NULL,
		[SubcategoryId] uniqueidentifier DEFAULT (NEWSEQUENTIALID()) ROWGUIDCOL NOT NULL,
		[Name] varchar(64) NOT NULL,
		[Description] varchar(256) NULL

		PRIMARY KEY CLUSTERED (
			[CategoryId] ASC
		),

		UNIQUE (
			[Name] ASC
		)
	);

	INSERT INTO @categoryTable (
		[CategoryId],
		[Name],
		[Description]
	)
	SELECT
		x.value('@CategoryId', 'uniqueidentifier') [CategoryId],
		x.value('@Name', 'nvarchar(64)') [Name],
		x.value('@Description', 'nvarchar(256)') [Description]
	FROM
		@xml.nodes('/Category/row') u(x);


	BEGIN TRANSACTION;


	UPDATE [Image].[Category]
	SET
		[Name] = tt.[Name],
		[Description] = tt.[Description]
	FROM
		@categoryTable tt
		JOIN [Image].[Category] c ON c.[CategoryId] = tt.[CategoryId]
	WHERE
		c.[CategoryId] != tt.[CategoryId]
		OR c.[Name] != tt.[Name]
		OR [Image].[NullableEquals](c.[Description], tt.[Description]) = 0;


	INSERT INTO [Image].[Category] (
		[CategoryId],
		[Name],
		[Description],
		[ModifiedBy]
	)
	SELECT
		tt.[CategoryId],
		tt.[Name],
		tt.[Description],
		@systemId [ModifiedBy]
	FROM
		@categoryTable tt
		LEFT JOIN [Image].[Category] c ON c.[CategoryId] = tt.[CategoryId]
	WHERE
		c.[CategoryId] IS NULL;

	COMMIT TRANSACTION;

	PRINT 'Update Category - END';
END TRY
BEGIN CATCH
	PRINT 'A failure occured while updating Category.';

	IF @@TRANCOUNT > 0
	BEGIN
		PRINT '*** ROLLING BACK TRANSACTION ***'

		ROLLBACK TRANSACTION;
	END;

	PRINT '*** ABORTING ***'
	RETURN;
END CATCH;
