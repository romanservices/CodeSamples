

/****** Object:  DdlTrigger [DDLTrigger]    Script Date: 05/20/2015 14:43:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [DDLTrackingTrigger]
    ON DATABASE
    FOR CREATE_PROCEDURE, ALTER_PROCEDURE, DROP_PROCEDURE, CREATE_FUNCTION, ALTER_FUNCTION, DROP_FUNCTION
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE
        @EventData XML = EVENTDATA();
 
    DECLARE 
        @ip VARCHAR(32) =
        (
            SELECT client_net_address
                FROM sys.dm_exec_connections
                WHERE session_id = @@SPID
        );
 
    INSERT DDLEvents..DDLEventsCustomerMgmt
    (
        EventType,
        EventDDL,
        EventXML,
        DatabaseName,
        SchemaName,
        ObjectName,
        HostName,
        IPAddress,
        ProgramName,
        LoginName
    )
    SELECT
        @EventData.value('(/EVENT_INSTANCE/EventType)[1]',   'NVARCHAR(100)'), 
        @EventData.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'NVARCHAR(MAX)'),
        @EventData,
        DB_NAME(),
        @EventData.value('(/EVENT_INSTANCE/SchemaName)[1]',  'NVARCHAR(255)'), 
        @EventData.value('(/EVENT_INSTANCE/ObjectName)[1]',  'NVARCHAR(255)'),
        HOST_NAME(),
        @ip,
        PROGRAM_NAME(),
        SUSER_SNAME();
        
 
END




GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

DISABLE TRIGGER [DDLTrigger] ON DATABASE
GO

ENABLE TRIGGER [DDLTrigger] ON DATABASE
GO


