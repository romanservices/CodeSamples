USE [Members]
GO
/****** Object:  StoredProcedure [dbo].[_DataTools_ListFtprootsLifetimeAssessmentStatus]    Script Date: 05/20/2015 15:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[_DataTools_ListFtprootsLifetimeAssessmentStatus](@studentId  INT,
                                                                @TertiaryId INT)
AS
    DECLARE @kcProductID AS TABLE
      (
         productId INT
      );

    INSERT INTO @kcProductID
    SELECT ProductId
    FROM   KinderCornerProductIdList;

    DECLARE @RootsClassIds AS TABLE
      (
         rootsClassAssignmentId INT
      );

    DELETE FROM @RootsClassIds;

    INSERT INTO @RootsClassIds
    SELECT lis.ClassroomAssignmentId
    FROM   LifetimeIndividualScore lis
           LEFT JOIN classroomassignments class
                  ON class.ClassroomAssignmentId = lis.ClassroomAssignmentId
    WHERE  StudentId = @studentId
           AND class.ProductId NOT IN(SELECT *
                                      FROM   @kcProductID);

    DECLARE @t2 TABLE
      (
         mid     INT,
         num     INT,
         measure VARCHAR(MAX),
         alv     INT,
         ahv     INT,
         ai      INT,
         mType   INT
      );
    DECLARE @table AS TABLE
      (
         Number             INT,
         ClassroomMeasureId INT,
         ClassroomMeasure   VARCHAR(max),
         AcceptedLowValue   INT,
         AcceptedHighValue  INT,
         AllowedIncrement   INT
      )
 

    DECLARE @ftpm AS TABLE
      (
         ClassroomMeasureId INT,
         Number             INT,
         ClassroomMeasure   VARCHAR(max),
         AcceptedLowValue   INT,
         AcceptedHighValue  INT,
         AllowedIncrement   INT,
         MeasureType        INT
      )

    INSERT @ftpm
    SELECT DISTINCT( ClassroomMeasureId ),
                   t.Number,
                   t.ClassroomMeasure,
                   t.AcceptedLowValue,
                   t.AcceptedHighValue,
                   t.AllowedIncrement,
                   CASE t.ClassroomMeasureId
                     WHEN 132 THEN 1
                     WHEN 133 THEN 1
                     WHEN 81 THEN 1
                     WHEN 130 THEN 1
                     WHEN 10001 THEN 1
                     WHEN 10002 THEN 1
                     WHEN 10003 THEN 1
                     WHEN 10004 THEN 1
                     WHEN 10005 THEN 1
                     WHEN 10006 THEN 1
                     WHEN 10007 THEN 1
                     WHEN 10008 THEN 1
                     WHEN 10009 THEN 1
                     WHEN 10010 THEN 1
                     ELSE 0
                   END
    FROM   @table t
    ORDER  BY Number,
              ClassroomMeasureId

    INSERT @t2
    SELECT *
    FROM   @ftpm

    DECLARE @t1 TABLE
      (
         MeasureId        INT,
         AssessmentNumber INT,
         Measure          VARCHAR(255),
         LowValue         INT,
         HighValue        INT,
         ai               INT,
         MeasureType      INT,
         Score            INT,
         AssessmentTaken  BIT
      );

    INSERT INTO @t1
    SELECT *,
           NULL,
           0
    FROM   @t2
    WHERE  mid < 10000

    UPDATE @t1
    SET    Score = Isnull(lis.Score, -1)
    FROM   LifetimeIndividualScore lis
    WHERE  lis.StudentId = @studentId
           AND Isnull(lis.TertiaryId, @TertiaryId) = @TertiaryId
           AND Isnull(lis.ClassroomMeasureId, MeasureId) = MeasureId
           and isnull(lis.SecondaryId, AssessmentNumber) = AssessmentNumber
           AND lis.ClassroomAssignmentId IN(SELECT *
                                            FROM   @RootsClassIds);

    SELECT a.MeasureId,
           a.AssessmentNumber,
           a.Measure,
           a.LowValue,
           a.HighValue,
           a.ai,
           a.MeasureType,
           a.Score,
           CASE Sign(Isnull(assess.Score, 0))
             WHEN 1 THEN 1
             WHEN 0 THEN 0
             WHEN NULL THEN 0
           END AS AssessmentTaken
    FROM   @t1 a
           JOIN (SELECT Sum(score) AS Score,
                        AssessmentNumber
                 FROM   @t1
                 GROUP  BY AssessmentNumber)AS assess
             ON assess.AssessmentNumber = a.AssessmentNumber
    ORDER  BY a.AssessmentNumber ASC 
