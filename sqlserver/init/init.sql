IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TestDatabase')
BEGIN
	CREATE DATABASE TestDatabase;		
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = 'testuser')
BEGIN
	CREATE LOGIN testuser WITH PASSWORD = 'Test@User123';
END;

USE TestDatabase;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'testuser')
BEGIN
	CREATE USER testuser FOR LOGIN testuser;
END;

ALTER ROLE db_owner ADD MEMBER testuser;
