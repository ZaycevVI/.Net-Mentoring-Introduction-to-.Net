/*
** Copyright Microsoft, Inc. 1994 - 2000
** All Rights Reserved.
*/

SET NOCOUNT ON
GO

USE Northwind
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = N'Region')
		   EXECUTE sp_rename @objname = N'[dbo].[Region]', @newname = N'Regions', @objtype = N'OBJECT';
GO

IF COL_LENGTH('dbo.Customers', 'FoundationDate') IS NULL
	ALTER TABLE [dbo].[Customers]
		ADD [FoundationDate] DATE NULL;
GO