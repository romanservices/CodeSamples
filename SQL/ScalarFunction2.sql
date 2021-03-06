USE [CustomerMgmt]
GO
/****** Object:  UserDefinedFunction [dbo].[_SALI_GetTWAStudentMasteryByGradingPeriod]    Script Date: 05/20/2015 14:42:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[_SALI_GetTWAStudentMasteryByGradingPeriod] (
	  @SchoolId AS INT
	, @SchoolYearId AS INT
	, @GradingPeriodId AS INT
	, @TWAActivityId AS INT
	, @TWAStudentId AS INT
	)
--SELECT dbo._SALI_GetTWAStudentMasteryByGradingPeriod (26171,70,2,14,4393203)
RETURNS INT
--DECLARE @SchoolId        AS INT = 26171,
--        @SchoolYearId    AS INT = 70,
--        @GradingPeriodId AS INT = 5,
--        @TWAActivityId   AS INT = 14,
--        @TWAStudentId    AS INT = 4393203

BEGIN
    DECLARE @Result AS INT = 0
    --GET SCHOOL GRADING PERIOD DATES:	
    DECLARE @GPTable TABLE
      (
         GradingPeriodId INT,
         StartDate       DATE,
         EndDate         DATE,
         HighMastery     INT,
         idx             INT PRIMARY KEY IDENTITY
      )

    INSERT @GPTable
    SELECT GradingPeriodId,
           StartDt,
           EndDt,
           0
    FROM   Members..SchoolCalendars sc
           INNER JOIN Members..SchoolCalendarEvents sce
                   ON sce.SchoolCalendarId = sc.SchoolCalendarId
    WHERE  sc.SchoolId = @SchoolId
           AND sc.SchoolYearId = @SchoolYearId
           AND sce.SchoolCalendarEventTypeId = 4

    DECLARE @row INT = 1
    DECLARE @Start DATE,
            @End   DATE

    WHILE @row <= (SELECT Count(*)
                   FROM   @GPTable)
      BEGIN
          SELECT @Start = StartDate,
                 @End = EndDate
          FROM   @GPTable
          WHERE  idx = @row

          UPDATE @GPTable
          SET    HighMastery = (SELECT TOP 1 measure.ActivityMeasureId
                                       FROM   TWAStudentVerificationAssessment assessment
                                              INNER JOIN TWAStudentVerificationAssessmentAttempt attempt
                                                      ON attempt.StudentVerificationAssessmentId = assessment.StudentVerificationAssessmentId
                                              INNER JOIN TWAStudentVerificationAssessmentAttemptMeasure measure
                                                      ON measure.StudentVerificationAssessmentAttemptId = attempt.StudentVerificationAssessmentAttemptId
                                       WHERE  assessment.TWAActivityId = @TWAActivityId
                                              AND assessment.TWAStudentId = @TWAStudentId
                                              AND DateAssessed BETWEEN @Start AND @End
                                       ORDER  BY ActivityMeasureId DESC)
          WHERE  idx = @row

          SET @row = @row + 1
      END

    return (SELECT Max(HighMastery)
    FROM   @GPTable
    WHERE  GradingPeriodId <= @GradingPeriodId)
END 
