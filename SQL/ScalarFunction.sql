USE [CustomerMgmt]
GO
/****** Object:  UserDefinedFunction [dbo].[_SALI_GetTWAStudentSessionCountByGradingPeriod]    Script Date: 05/20/2015 14:41:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[_SALI_GetTWAStudentSessionCountByGradingPeriod] (@SchoolId        AS INT,
                                                                       @SchoolYearId    AS INT,
                                                                       @GradingPeriodId AS INT,
                                                                       @TWAStudentId    AS INT)
RETURNS INT
AS
--DECLARE @SchoolId        AS INT = 1001857,
--        @SchoolYearId    AS INT = 70,
--        @GradingPeriodId AS INT = 5,
--        @TWAStudentId    AS INT = 4261782

BEGIN
    DECLARE @Result AS INT = 0
    --GET SCHOOL GRADING PERIOD DATES:	
    DECLARE @GPStartDate AS DATE,
            @GPEndDate   AS DATE

    SELECT @GPStartDate = StartDt,
           @GPEndDate = EndDt
    FROM   Members..SchoolCalendars sc
           INNER JOIN Members..SchoolCalendarEvents sce
                   ON sce.SchoolCalendarId = sc.SchoolCalendarId
    WHERE  sc.SchoolId = @SchoolId
           AND sc.SchoolYearId = @SchoolYearId
           AND sce.GradingPeriodId = @GradingPeriodId
           AND sce.SchoolCalendarEventTypeId = 4 --TYPE 4 = GRADING PERIODS
             
           
    SELECT @Result = (SELECT Sum(StudentSessions)
                      FROM   (SELECT Count(DISTINCT mastery.SessionNumber) AS StudentSessions
                              FROM   TWAActivity2Student student
                                     INNER JOIN TWAActivity2StudentLevelMastery mastery
                                             ON mastery.TWAActivity2StudentId = student.TWAActivity2StudentId
                              WHERE  student.StudentId = @TWAStudentId
                                     AND mastery.SessionNumber between 1 and 10
                                     AND mastery.DateLastAssessed BETWEEN @GPStartDate AND @GPEndDate
                              GROUP  BY TeamTutoringPlanId) AS StudentSessions)

    RETURN @Result
    --SELECT @Result
END 
