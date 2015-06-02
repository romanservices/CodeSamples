USE [Archiver]
GO
/****** Object:  StoredProcedure [dbo].[Columnfetcher]    Script Date: 06/02/2015 09:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mark Kamberger
-- Create date: 5/30/2015
-- Description:	Fetches a string of columns in a table
-- =============================================
CREATE PROCEDURE [dbo].[Columnfetcher] (@SourceDatabase VARCHAR(400),
                                       @SourceTable    VARCHAR(400),
                                       @Columns        VARCHAR(MAX) OUTPUT)
AS
  BEGIN
      DECLARE @ColumnsT TABLE
        (
           Value VARCHAR(max)
        )
      DECLARE @ColumnFetcher VARCHAR(8000) = 'DECLARE @Columns VARCHAR(8000) 
											  SELECT @Columns =  COALESCE(@Columns + char(44)+char(32), char(32)) +  char(91)+COLUMN_NAME + CHAR(93)
										      FROM '
        + Replace(@SourceDatabase, '.dbo', '.INFORMATION_SCHEMA.COLUMNS')
        + '
                                              WHERE TABLE_NAME = N'
        + Char(39) + @SourceTable + Char(39)
        + ' Select @Columns'

      INSERT @ColumnsT
      EXEC(@ColumnFetcher)

      SET @Columns = (SELECT TOP 1 Value
                      FROM   @ColumnsT)

      RETURN 0
  END
GO
/****** Object:  Table [dbo].[TargetTable]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TargetTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceTableId] [int] NOT NULL,
	[DatabaseName] [varchar](50) NOT NULL,
	[TableName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_TargetTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TargetChildTable]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TargetChildTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceChildTableId] [int] NOT NULL,
	[TableName] [varchar](400) NOT NULL,
 CONSTRAINT [PK_TargetChildTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SourceTable]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SourceTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseName] [varchar](50) NOT NULL,
	[TableName] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[DateColumnName] [varchar](255) NOT NULL,
	[YearsToMaintain] [int] NOT NULL,
	[MaxDeleteCountPerTransaction] [int] NOT NULL,
	[ProcessChldrenFirst] [bit] NOT NULL,
 CONSTRAINT [PK_SourceTables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SourceChildTables]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SourceChildTables](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceTableId] [int] NOT NULL,
	[TableName] [varchar](400) NOT NULL,
	[ProcessOrder] [int] NOT NULL,
	[Active] [int] NOT NULL,
	[Parent_FK] [varchar](100) NULL,
	[Self_FK] [varchar](100) NULL,
 CONSTRAINT [PK_SourceChildTables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceTable] [varchar](400) NOT NULL,
	[EventStartDate] [datetime] NOT NULL,
	[EventEndDate] [datetime] NULL,
	[Success] [bit] NOT NULL,
	[RowsArchived] [bigint] NOT NULL,
	[ParentTableLogId] [int] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ErrorTransactions]    Script Date: 06/02/2015 09:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogId] [int] NOT NULL,
	[FailedStatement] [varchar](max) NOT NULL,
	[ErrorMessage] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[CreateSingleTableScript]    Script Date: 06/02/2015 09:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Kamberger
-- Create date: 5/26/2015
-- Description:	Creates an archive / delete script for a single table
-- =============================================
CREATE PROCEDURE [dbo].[CreateSingleTableScript] @Source           VARCHAR(400),
                                         @Columns          VARCHAR(8000),
                                         @DestinationTable VARCHAR(400),
                                         @Destination      VARCHAR(400),
                                         @DeleteCount      INT,
                                         @LogId            INT,
                                         @Script           VARCHAR(max) OUTPUT
AS
  BEGIN
      SET NOCOUNT ON;

      DECLARE @TableIdentity AS TABLE
        (
           Name        VARCHAR(400),
           HasIdentity INT
        )

      INSERT @TableIdentity
      SELECT name,
             Objectproperty(id, 'TableHasIdentity') AS TableHasIdentity
      FROM   sysobjects
      WHERE  xtype = 'U'

      DECLARE @delteArchiveQuery VARCHAR(MAX)= ''

      IF( (SELECT HasIdentity
           FROM   @TableIdentity
           WHERE  Name = @DestinationTable) = 1 )
        BEGIN
            SET @delteArchiveQuery = 'SET IDENTITY_INSERT ' + @Destination + ' ON;'
        END

      SET @delteArchiveQuery = @delteArchiveQuery + 'BEGIN TRY
		;WITH deleteList
			AS (SELECT TOP '
                               + CONVERT(VARCHAR(50), @DeleteCount) + ' '
                               + @Columns + ' from ' + @Source
                               + ')
			DELETE FROM deleteList OUTPUT '
                               + Replace(@Columns, '[', 'DELETED.[')
                               + ' INTO ' + @Destination + ' (' + @Columns + ')'
                               + ' update [log] set EventEndDate = GetDate(), RowsArchived = (select RowsArchived from [log] where Id = '
                               + CONVERT(VARCHAR(50), @LogID)
                               + ') + @@ROWCOUNT where Id = '
                               + CONVERT(VARCHAR(50), @LogID)
                               + ' 
       END TRY
	   BEGIN Catch
	        
	   END Catch'
	   set @Script = @delteArchiveQuery
	   return 0
  END
GO
/****** Object:  StoredProcedure [dbo].[CreateMultiTableScript]    Script Date: 06/02/2015 09:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--=============================================
--Author:		Mark Kamberger
--Create date: 5/26/2015
--Description:	Creates an archive / delete script for a single table
--=============================================
CREATE PROCEDURE [dbo].[CreateMultiTableScript] @Source           VARCHAR(400),
                                               @Columns          VARCHAR(8000),
                                               @DestinationTable VARCHAR(400),
                                               @Destination      VARCHAR(400),
                                               @DeleteCount      INT,
                                               @LogId            INT,
                                               @SourceId         INT,
                                               @Script           VARCHAR(max) OUTPUT
AS
  --DECLARE @Source           VARCHAR(400) = '[SFA-DBUA1].Members.dbo.StudentTests Where DateGraded < (SELECT Dateadd(yy, -3, Getdate()))',
  --        @Columns          VARCHAR(8000) = '[StudentTestId], [SchoolTestID], [StudentID], [Score], [NumberCorrect], [TargetGrowth], [TargetScore], [OriginalScore], [Notes], [ScoreTypeID], [TestResultsSourceId], [Graded], [DateGraded], [CustomerContactId], [LastModified]',
  --        @DestinationTable VARCHAR(400) = 'StudentTests',
  --        @Destination      VARCHAR(400) = 'Archiver..StudentTests',
  --        @DeleteCount      INT = '10000',
  --        @LogId            INT = 1,
  --        @SourceId         INT = 7,
  --        @Script           VARCHAR(max)
  BEGIN
      SET NOCOUNT ON;

      /*
      Used to determine if we have to set INSET_IDENTITY ON or not
      */
      DECLARE @TableIdentity AS TABLE
        (
           Name        VARCHAR(400),
           HasIdentity INT
        )

      INSERT @TableIdentity
      SELECT name,
             Objectproperty(id, 'TableHasIdentity') AS TableHasIdentity
      FROM   sysobjects
      WHERE  xtype = 'U'

      /*
      Fetch associated child tables that need to be archived before the parent table - avoiding cascade delete issue, 
      This seems to minimize the log size and increase performance.
      */
      DECLARE @ChildTableQueries NVARCHAR(max)
      DECLARE @ChildTables AS TABLE
        (
           Id            INT,
           SourceTableId INT,
           TableName     VARCHAR(400),
           Parent_FK     VARCHAR(400),
           Self_FK       VARCHAR(400),
           DataBaseName  VARCHAR(400),
           TargetDB      VARCHAR(400),
           TargetTable   VARCHAR(400),
           idx           INT IDENTITY (1, 1)
        )

      INSERT @ChildTables
      SELECT sct.Id,
             sct.SourceTableId,
             sct.TableName,
             Parent_FK,
             Self_FK,
             st.DatabaseName,
             tt.DatabaseName AS 'TargetDB',
             tt.TableName    AS 'TargetTable'
      FROM   SourceChildTables sct
             INNER JOIN SourceTable st
                     ON st.Id = sct.SourceTableId
             INNER JOIN TargetTable tt
                     ON tt.SourceTableId = st.Id
      WHERE  sct.SourceTableId = @SourceId
             AND sct.Active = 1
      ORDER  BY ProcessOrder ASC

      /*
      Build out the columns for a # Temp table
      
      */
      DECLARE @TempTableColumns VARCHAR(8000)

      SELECT @TempTableColumns = COALESCE(@TempTableColumns +', ', '') + '['
                                 + c.name + '] ' + CASE t.name WHEN 'varchar' THEN t.name + '(' + CONVERT(VARCHAR(100), CASE c.max_length WHEN -1 THEN 'max'ELSE CONVERT(VARCHAR(10), c.max_length) END) + ')' WHEN 'nvarchar' THEN t.name + '(' + CONVERT(VARCHAR(100), CASE c.max_length WHEN -1 THEN 'max' ELSE CONVERT(VARCHAR(10), c.max_length)END) + ')' WHEN 'nchar' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'char' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'CHARACTER' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'BINARY' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'VARBINARY' THEN t.name + '(' + CONVERT(VARCHAR(100), CASE c.max_length WHEN -1 THEN 'max' ELSE CONVERT(VARCHAR(10), c.max_length) END) + ')' WHEN 'datetime2' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'time' THEN t.name + '(' + CONVERT(VARCHAR(100), c.max_length) + ')' WHEN 'INTEGER' THEN t.name + '(' + CONVERT(
                                 VARCHAR(100)
                                        , c.PRECISION) + ')' WHEN 'Decimal' THEN t.name + '(' + CONVERT(VARCHAR(100), c.PRECISION) + ',' + CONVERT(VARCHAR(10), c.scale) + ')' WHEN 'Numeric' THEN t.name + '(' + CONVERT(VARCHAR(100), c.PRECISION) + ',' + CONVERT(VARCHAR(10), c.scale) + ')' WHEN 'INTEGER' THEN t.name + '(' + CONVERT(VARCHAR(100), c.PRECISION) + ')' ELSE t.name END
      FROM   sys.columns c
             INNER JOIN sys.types t
                     ON c.user_type_id = t.user_type_id
             LEFT OUTER JOIN sys.index_columns ic
                          ON ic.object_id = c.object_id
                             AND ic.column_id = c.column_id
             LEFT OUTER JOIN sys.indexes i
                          ON ic.object_id = i.object_id
                             AND ic.index_id = i.index_id
      WHERE  c.object_id = Object_id(@DestinationTable)

      /*
      Begin to build out the full query - Create a #temp to house the chunks of parent records to archive, The ID's will be used to archive off the child records
      */
      DECLARE @FullQuery NVARCHAR(max) = '
										Create table #ParentRowTable ('
        + @TempTableColumns + ')
										insert #ParentRowTable
										SELECT TOP '
        + CONVERT(VARCHAR(50), @DeleteCount) + ' '
        + @Columns + ' from ' + @Source
      DECLARE @ChildTableRow INT = 1
      DECLARE @ChildQueries NVARCHAR(max) = ''

      WHILE( @ChildTableRow <= (SELECT Count(*)
                                FROM   @ChildTables) )
        BEGIN
            /*
            for each child table build out the script and append it to the FullQuery
            */
            DECLARE @childColumns   VARCHAR(8000),
                    @ChildTableName VARCHAR(400),
                    @Child_FK_Name  VARCHAR(400),
                    @Parent_FK_Name VARCHAR(400),
                    @DataBaseName   VARCHAR(400),
                    @TargetDB       VARCHAR(400),
                    @TargetTable    VARCHAR(400),
                    @ChildLogId     INT

            SELECT @ChildTableName = TableName,
                   @Child_FK_Name = Self_FK,
                   @Parent_FK_Name = Parent_FK,
                   @DataBaseName = DataBaseName,
                   @TargetDB = TargetDB,
                   @TargetTable = TargetTable
            FROM   @ChildTables
            WHERE  idx = @ChildTableRow

            INSERT [Log]
                   (EventStartDate,
                    SourceTable,
                    RowsArchived,
                    Success,
                    ParentTableLogId)
            VALUES (Getdate(),
                    @DataBaseName + '.' + @ChildTableName,
                    0,
                    0,
                    @LogId)

            SET @ChildLogId = @@IDENTITY

            EXEC Columnfetcher
              @DataBaseName,
              @ChildTableName,
              @childColumns OUTPUT

            DECLARE @chRow NVARCHAR(max) = ''

            IF( (SELECT HasIdentity
                 FROM   @TableIdentity
                 WHERE  Name = @DestinationTable) = 1 )
              BEGIN
                  SET @chRow = 'SET IDENTITY_INSERT ' + @TargetDB + '.'
                               + @ChildTableName + ' ON; '
              END

            SET @chRow = @chRow + ' 
           BEGIN TRY ' + 'Declare @'
                         + @ChildTableName + '_Log int = '
                         + CONVERT(VARCHAR(100), @ChildLogId)
                         + 'delete from ' + @DataBaseName + '.'
                         + @ChildTableName + ' OUTPUT '
                         + Replace(@childColumns, '[', 'DELETED.[')
                         + ' INTO ' + @TargetDB + '.' + @ChildTableName + ' ('
                         + @childColumns + ')' + ' where ' + @Child_FK_Name
                         + ' in (select ' + @Parent_FK_Name
                         + ' From #ParentRowTable)'
                         + ' ; with LogUpdater as (select RowsArchived from [Log] where Id = @'
                         + @ChildTableName
                         + '_Log)
                         Update LogUpdater set RowsArchived = RowsArchived + @@ROWCOUNT'
                         + ' END TRY ' + ' BEGIN Catch 
                        INSERT ErrorTransactions
                           (LogId,
                            FailedStatement,
                            ErrorMessage)
                    VALUES(@'
                         + @ChildTableName
                         + '_Log,
                           ' + Char(39)
                         + 'OUTPUT '
                         + Replace(@childColumns, '[', 'DELETED.[')
                         + ' INTO ' + @TargetDB + ' (' + @ChildTableName + ')'
                         + Char(39) + ',
                           Error_message());
                       '
                         + ' END Catch '

            IF( (SELECT HasIdentity
                 FROM   @TableIdentity
                 WHERE  Name = @DestinationTable) = 1 )
              BEGIN
                  SET @chRow = @chRow
                               + ' 
                SET IDENTITY_INSERT '
                               + @TargetDB + '.' + @ChildTableName + ' OFF; '
              END

            SET @ChildQueries = @ChildQueries + ' 
           ' + @chRow
            SET @ChildTableRow = @ChildTableRow + 1
        END

      SET @FullQuery = @FullQuery + '  
     ' + @ChildQueries

      --SELECT @FullQuery
      DECLARE @parentPKName VARCHAR(400) = (SELECT TOP 1 Parent_FK
                 FROM   @ChildTables),
              @ParentDB     VARCHAR(400),
              @ParentTable  VARCHAR(400)

      SELECT @ParentDB = DatabaseName,
             @ParentTable = TableName
      FROM   SourceTable
      WHERE  Id = @SourceId

      DECLARE @ParentQuery NVARCHAR(MAX)= ''

      IF( (SELECT HasIdentity
           FROM   @TableIdentity
           WHERE  Name = @DestinationTable) = 1 )
        BEGIN
            SET @ParentQuery = @ParentQuery
                               + '
            SET IDENTITY_INSERT '
                               + @Destination + ' ON;'
        END

      SET @ParentQuery = @ParentQuery + '
    BEGIN TRY 
    delete from ' + @ParentDB
                         + '.' + @ParentTable + ' OUTPUT '
                         + Replace(@Columns, '[', 'DELETED.[')
                         + ' INTO ' + @Destination + ' (' + @Columns + ')'
                         + ' where ' + @parentPKName + ' in (select '
                         + @parentPKName + ' From #ParentRowTable)'
                         + ' ; with LogUpdater as (select RowsArchived from [Log] where Id = '
                         + CONVERT(VARCHAR(100), @LOGID)
                         + ')
                         Update LogUpdater set RowsArchived = RowsArchived + @@ROWCOUNT 
  END TRY
  BEGIN Catch
    INSERT ErrorTransactions
                           (LogId,
                            FailedStatement,
                            ErrorMessage) 
                    VALUES('
                         + CONVERT(VARCHAR(50), @LogID) + ',' + Char(39)
                         + 'OUTPUT '
                         + Replace(@Columns, '[', 'DELETED.[')
                         + ' INTO ' + @Destination + ' (' + @Columns + ')'
                         + Char(39) + ', Error_message());
  END Catch'

      IF( (SELECT HasIdentity
           FROM   @TableIdentity
           WHERE  Name = @DestinationTable) = 1 )
        BEGIN
            SET @ParentQuery = @ParentQuery
                               + '
          SET IDENTITY_INSERT '
                               + @Destination + ' OFF;'
        END

      SET @FullQuery = @FullQuery + '    
  ' + @ParentQuery
      SET @Script = @FullQuery

      RETURN 0
  END
GO
/****** Object:  StoredProcedure [dbo].[_ProcessArchive]    Script Date: 06/02/2015 09:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[_ProcessArchive]
as
BEGIN
    DECLARE @SourceCount INT,
            @Row         INT = 1,
            @DeleteCount INT = 50000
    DECLARE @sourceTable AS TABLE
      (
         Source               VARCHAR(max),
         DateColumnName       VARCHAR(255),
         Destination          VARCHAR(max),
         SourceDB             VARCHAR(255),
         SourceT              VARCHAR(400),
         DestinationT         VARCHAR(400),
         Yesars               INT,
         MaxDeletePerT        INT,
         ProcessChildrenFirst BIT,
         Id                   INT,
         idx                  INT IDENTITY
      )

    INSERT @sourceTable
    SELECT s.DatabaseName + '.' + s.TableName,
           DateColumnName,
           t.DatabaseName + '.' + t.TableName,
           s.DatabaseName,
           s.TableName,
           t.TableName,
           s.YearsToMaintain,
           s.MaxDeleteCountPerTransaction,
           s.ProcessChldrenFirst,
           s.Id
    FROM   SourceTable s
           INNER JOIN TargetTable t
                   ON t.SourceTableId = s.Id
    WHERE  Active = 1

    DECLARE @foundRecordCount TABLE
      (
         Value INT
      )

    SET @SourceCount = (SELECT Count(*)
                        FROM   @sourceTable)

    WHILE @Row <= @SourceCount
      BEGIN
          DECLARE @table                VARCHAR(max),
                  @DateColumnName       VARCHAR(255),
                  @Destination          VARCHAR(MAX),
                  @sourcDB              VARCHAR(400),
                  @SourceT              VARCHAR(400),
                  @DestinationT         VARCHAR(400),
                  @LogID                INT,
                  @LogIdV               VARCHAR(50),
                  @Years                INT,
                  @TotalFoundRecords    INT = 0,
                  @HasChildrenToProcess BIT,
                  @SourceId             INT

          SELECT @table = Source,
                 @DateColumnName = DateColumnName,
                 @Destination = Destination,
                 @sourcDB = SourceDB,
                 @SourceT = SourceT,
                 @Years = Yesars,
                 @DeleteCount = MaxDeletePerT,
                 @DestinationT = DestinationT,
                 @HasChildrenToProcess = ProcessChildrenFirst,
                 @SourceId = Id
          FROM   @sourceTable
          WHERE  idx = @Row

          /*
          SQL requires column list for the output insert  
          */
          DECLARE @ColumnString VARCHAR(max) = ''

          EXEC Columnfetcher
            @sourcDB,
            @SourceT,
            @ColumnString OUTPUT

          /*
          Start a log of the process
          */
          INSERT [Log]
                 (EventStartDate,
                  SourceTable,
                  RowsArchived,
                  Success)
          VALUES (Getdate(),
                  @table,
                  0,
                  0)

          SET @LogId = @@IDENTITY

          DECLARE @sTable VARCHAR(max) = @table + ' Where ' + @DateColumnName
            + ' < (SELECT Dateadd(yy, -'
            + CONVERT(VARCHAR(10), @Years)
            + ', Getdate()))'
          /*
          Begin Create the dynamic delete to archive statement 
          */
          DECLARE @delteArchiveQuery VARCHAR(MAX)= ''

          IF( @HasChildrenToProcess = 0 )
            BEGIN
                EXEC Createsingletablescript
                  @sTable,
                  @ColumnString,
                  @DestinationT,
                  @Destination,
                  @DeleteCount,
                  @LogId,
                  @delteArchiveQuery OUTPUT
            END
          ELSE
            BEGIN
                EXEC Createmultitablescript
                  @sTable,
                  @ColumnString,
                  @DestinationT,
                  @Destination,
                  @DeleteCount,
                  @LogId,
                  @SourceId,
                  @delteArchiveQuery OUTPUT
            END

          /*
          How many rows total will need to be archived
          */
          DELETE FROM @foundRecordCount

          INSERT @foundRecordCount
          -- SELECT 1
          /*
          Slowest part of the whole thing
          */
          EXEC('select count(*) from' + @sTable)

          SET @TotalFoundRecords = (SELECT Value
                                    FROM   @foundRecordCount)

      /*
      Uncomment the following line to view the created queries 
      */
          --SELECT @delteArchiveQuery AS 'Dynamic Delete / Archive Query'
          /*
          Begin looping through the data to be deleted - we do this in chunks to keep the log file size low and to increase performance
          */
          WHILE( (SELECT Value
                  FROM   @foundRecordCount) > 0 )
            BEGIN
                BEGIN TRY
                    PRINT 'Begin Delete Archive'

                    --Comment the EXEC out to no actually run the archive
                    EXEC(@delteArchiveQuery)
                END TRY

                BEGIN CATCH
                    INSERT ErrorTransactions
                           (LogId,
                            FailedStatement,
                            ErrorMessage)
                    VALUES(@LogId,
                           @delteArchiveQuery,
                           Error_message());
                END CATCH

                --Adjust the delete count based on rows to be deleted
                UPDATE @foundRecordCount
                SET    Value = ( Value - @DeleteCount )
            END

          IF( ( (SELECT value
                 FROM   @foundRecordCount) <= 0
                AND @TotalFoundRecords > 0 )
               OR @TotalFoundRecords = 0 )
            BEGIN
                UPDATE [log]
                SET    Success = 1,
                       EventEndDate = Getdate()
                WHERE  Id = @LogID;

                WITH ChildLogs
                     AS (SELECT Id,
                                SourceTable,
                                EventStartDate,
                                EventEndDate,
                                Success,
                                RowsArchived,
                                ParentTableLogId
                         FROM   [Log]
                         WHERE  ParentTableLogId = @LogID)
                UPDATE ChildLogs
                SET    EventEndDate = Getdate(),
                       Success = 1
            END
          ELSE
            BEGIN
                UPDATE [log]
                SET    Success = 0,
                       EventEndDate = Getdate()
                WHERE  Id = @LogID
            END

          SET @Row = @Row + 1
      END
END
GO
/****** Object:  Default [DF_SourceTable_DateColumnName]    Script Date: 06/02/2015 09:51:54 ******/
ALTER TABLE [dbo].[SourceTable] ADD  CONSTRAINT [DF_SourceTable_DateColumnName]  DEFAULT ('') FOR [DateColumnName]
GO
/****** Object:  Default [DF_SourceTable_YearsToMaintain]    Script Date: 06/02/2015 09:51:54 ******/
ALTER TABLE [dbo].[SourceTable] ADD  CONSTRAINT [DF_SourceTable_YearsToMaintain]  DEFAULT ((3)) FOR [YearsToMaintain]
GO
/****** Object:  Default [DF_SourceTable_MaxDeleteCountPerTransaction]    Script Date: 06/02/2015 09:51:54 ******/
ALTER TABLE [dbo].[SourceTable] ADD  CONSTRAINT [DF_SourceTable_MaxDeleteCountPerTransaction]  DEFAULT ((50000)) FOR [MaxDeleteCountPerTransaction]
GO
/****** Object:  Default [DF_SourceTable_ProcessChldrenFirst]    Script Date: 06/02/2015 09:51:54 ******/
ALTER TABLE [dbo].[SourceTable] ADD  CONSTRAINT [DF_SourceTable_ProcessChldrenFirst]  DEFAULT ((0)) FOR [ProcessChldrenFirst]
GO
