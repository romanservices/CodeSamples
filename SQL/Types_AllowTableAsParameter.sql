USE [CustomerMgmt]
GO

/****** Object:  UserDefinedTableType [dbo].[TableTypeHelper]    Script Date: 05/20/2015 14:45:05 ******/
CREATE TYPE [dbo].[TableTypeHelper] AS TABLE(
	[NUMBER] [int] NULL,
	[string] [varchar](max) NULL
)
GO


