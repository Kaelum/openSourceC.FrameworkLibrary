/*
-- The following line gets the SID of the user, which can be duplicated on another server.
DECLARE @userSid varbinary(16);

SELECT @userSid = SUSER_SID('sa');
SELECT @userSid [SID];
*/



/*
-- The following line lists the users and their SIDs for the current database, which can be duplicated on another server.
sp_helpuser;
*/



/*
DECLARE @password nvarchar(128);
DECLARE @passwordHash varbinary(128);

SET @password = '<secure password>';
SET @passwordHash = PWDENCRYPT(@password);

SELECT @password [password], @passwordHash [passwordHash], PWDCOMPARE(@password, @passwordHash) [PWDCOMPARE];

ALTER LOGIN [sa] WITH PASSWORD = @passwordHash HASHED;
*/



/*
CREATE LOGIN [NewUsername] WITH
	PASSWORD = @passwordHash HASHED,
	SID = @userSid,
	DEFAULT_DATABASE = [master],
	DEFAULT_LANGUAGE = [us_english],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = OFF;
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NewUsername];
GO
*/
