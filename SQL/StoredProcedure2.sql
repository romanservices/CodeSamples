USE [CustomerMgmt]
GO
/****** Object:  StoredProcedure [dbo].[_SALI_CheckForTutoringPlan]    Script Date: 05/20/2015 15:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[_SALI_CheckForTutoringPlan] (@StudentId INT)
AS
--DECLARE @StudentId INT = 4477539

BEGIN
    DECLARE @PlanId INT = -1
    DECLARE @CurrentPlanId AS INT = -1

    SET @PlanId = -1

    DECLARE @pa1ScoreTotal    AS INT,
            @pa2ScoreTotal    AS INT,
            @pa3ScoreTotal    AS INT,
            @paPresentedTotal AS INT,
            @lsPresentedTotal AS INT,
            @ls1ScoreTotal    AS INT,
            @planResults      AS VARCHAR(2000),
            @plan1Message     AS VARCHAR(255),
            @plan2Message     AS VARCHAR(255),
            @plan3_4Message   AS VARCHAR(255),
            @OverQualified    AS INT = -1

    SELECT @paPresentedTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 1, 2, 3 ))

    PRINT @paPresentedTotal

    SELECT @pa1ScoreTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 1 ))
           AND Score IN ( 1, 9999 )

    SELECT @pa2ScoreTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 2 ))
           AND Score IN ( 1, 9999 )

    SELECT @pa3ScoreTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 3 ))
           AND Score IN ( 1, 9999 )

    IF ( @pa1ScoreTotal = 0
         AND @pa2ScoreTotal = 0
         AND @pa3ScoreTotal = 0 )
      BEGIN
          SET @PlanId = -1
          SET @plan1Message = 'Please start the assessment'
          SET @planResults = 'PA(1) Score Total: '
                             + CONVERT(VARCHAR(10), @pa1ScoreTotal) + ', '
                             + 'PA(2) Score Total: '
                             + CONVERT(VARCHAR(10), @pa2ScoreTotal) + ', '
                             + 'PA(3) Score Total: '
                             + CONVERT(VARCHAR(10), @pa3ScoreTotal) + ', '
                             + @plan1Message
      END

   
    SELECT @lsPresentedTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  TWASectionObjectiveId = 10
                                             AND SetNumber = 1)
           AND TertiaryId = 1
           AND SecondaryId = 1

    SELECT @ls1ScoreTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  TWASectionObjectiveId = 10
                                             AND SetNumber = 1)
           AND Score IN ( 1, 9999 )
           AND TertiaryId = 1
           AND SecondaryId = 1


-- if master both PA and LS then plan 2 OR either PA or LS 
if((@pa1ScoreTotal >= 8
         AND @pa2ScoreTotal >= 8
         AND @pa3ScoreTotal >= 8
         AND @ls1ScoreTotal > 8) or (@pa1ScoreTotal >= 8
         AND @pa2ScoreTotal >= 8
         AND @pa3ScoreTotal >= 8) or ( @ls1ScoreTotal > 8) )
         BEGIN
           SET @PlanId = 2
             SET @plan1Message = 'PA(1-3) and/or LS(1) Mastered Qualify For Plan 2'
            set @OverQualified = -1
         END
         else
         BEGIN
         SET @PlanId = 1
             SET @plan1Message = 'PA(1-3) or LS(1) Not Mastered Qualify For Plan 1'
            set @OverQualified = -1
         END
 IF ( @pa1ScoreTotal = 0
         AND @pa2ScoreTotal = 0
         AND @pa3ScoreTotal = 0
         AND @ls1ScoreTotal = 0
         AND @paPresentedTotal = 0
         AND @lsPresentedTotal = 0 )
      BEGIN
          SET @PlanId = -1
          SET @plan1Message = 'Please start the assessment'

      END
         
         
 

   

    SET @planResults = 'PA(1) Score Total: '
                       + CONVERT(VARCHAR(10), Isnull(@pa1ScoreTotal, 0))
                       + ', ' + 'PA(2) Score Total: '
                       + CONVERT(VARCHAR(10), Isnull(@pa2ScoreTotal, 0))
                       + ', ' + 'PA(3) Score Total: '
                       + CONVERT(VARCHAR(10), Isnull(@pa3ScoreTotal, 0))
                       + ', ' + 'LS(1) Score Total: '
                       + CONVERT(VARCHAR(10), Isnull(@ls1ScoreTotal, 0))
                       + ', ' + @plan1Message

    

    IF ( @PlanId = 2 )
      BEGIN
          IF EXISTS (SELECT *
                     FROM   Members.dbo.LifetimeIndividualScore
                     WHERE  StudentId = @StudentId
                            AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                       FROM   TWAObjective2ClassroomMeasure t2
                                                       WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                              AND t2.SetNumber IN ( 1, 2 )))
            BEGIN
                SET @plan2Message = ''

                DECLARE @2ws1ScoreTotal AS INT,
                        @2ws2ScoreTotal AS INT,
                        @WS3AssessmentScoreTotal int

                SELECT @2ws1ScoreTotal = Count(*)
                FROM   Members.dbo.LifetimeIndividualScore
                WHERE  StudentId = @StudentId
                       AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                  FROM   TWAObjective2ClassroomMeasure t2
                                                  WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                         AND t2.SetNumber = 1)
                       AND Score IN ( 1, 9999 )

                SELECT @2ws2ScoreTotal = Count(*)
                FROM   Members.dbo.LifetimeIndividualScore
                WHERE  StudentId = @StudentId
                       AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                  FROM   TWAObjective2ClassroomMeasure t2
                                                  WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                         AND SetNumber = 2)
                       AND Score IN ( 1, 9999 )
                          SELECT @WS3AssessmentScoreTotal = Count(*)
                FROM   Members.dbo.LifetimeIndividualScore
                WHERE  StudentId = @StudentId
                       AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                  FROM   TWAObjective2ClassroomMeasure t2
                                                  WHERE  t2.TWASectionObjectiveId IN ( 18 )
                                                         AND SetNumber in (1,2,3,4,5,6,7,8))
                       AND Score IN ( 1, 9999 )
--select @WS3AssessmentScoreTotal
                IF( @2ws1ScoreTotal >= 4
                    AND @2ws2ScoreTotal >= 4 )
                  BEGIN
                      SET @plan2Message = 'Mastered WS(1) Set 1-2'
                      SET @PlanId = 2
                      SET @OverQualified = 1
                       IF ( @WS3AssessmentScoreTotal < 40 )
                        BEGIN
                            SET @plan2Message = @plan2Message
                                                + 'Failed WS(3) Recommend plan 3'
                            SET @PlanId = 3
                            SET @OverQualified = -1
                        END
                  END
                ELSE
                  BEGIN
                      IF ( @2ws1ScoreTotal < 4 )
                        BEGIN
                            SET @plan2Message = 'Failed WS(1) Set 1'
                            SET @PlanId = 2
                            SET @OverQualified = -1
                        END

                      IF ( @2ws2ScoreTotal < 4 )
                        BEGIN
                            SET @plan2Message = @plan2Message
                                                + 'Failed WS(2) Set 2 Recommend plan 2'
                            SET @PlanId = 2
                            SET @OverQualified = -1
                        END
                       
                  END

                SET @planResults = 'WS(1) Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@2ws1ScoreTotal, 0))
                                   + ', ' + 'WS(2) Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@2ws2ScoreTotal, 0))
                                   + ', ' + @plan2Message
                SET @planResults = 'WS(1) Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@2ws1ScoreTotal, 0))
                                   + ', ' + 'WS(2) Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@2ws2ScoreTotal, 0))
                                   + ', ' + @plan2Message

                PRINT @planResults
            END
      END

    DECLARE @C11               INT,
            @C12               INT,
            @C21               INT,
            @C22               INT,
            @C31               INT,
            @C32               INT,
            @w1CheckScoreTotal AS INT

    SELECT @C11 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 40 )
                                             AND t2.SetNumber = 1)

    SELECT @C12 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 41 )
                                             AND t2.SetNumber = 1)

    SELECT @C21 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 42 )
                                             AND t2.SetNumber = 1)

    SELECT @C22 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 43 )
                                             AND t2.SetNumber = 1)

    SELECT @C31 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 44 )
                                             AND t2.SetNumber = 1)

    SELECT @C32 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 45 )
                                             AND t2.SetNumber = 1)

    SELECT @C32 = Score
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 45 )
                                             AND t2.SetNumber = 1)

    DECLARE @AllowCheckPlan3_4 INT = 1

    IF( @C11 IS NULL )
      BEGIN
          SET @AllowCheckPlan3_4 = 0
      END

    SELECT @w1CheckScoreTotal = Count(*)
    FROM   Members.dbo.LifetimeIndividualScore
    WHERE  StudentId = @StudentId
           AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                      FROM   TWAObjective2ClassroomMeasure t2
                                      WHERE  t2.TWASectionObjectiveId IN ( 25 )
                                             AND SetNumber = 1)

    IF ( @PlanId = 2
         AND @AllowCheckPlan3_4 = 1 )
      BEGIN
          SET @OverQualified = -1

          IF EXISTS (SELECT *
                     FROM   Members.dbo.LifetimeIndividualScore
                     WHERE  StudentId = @StudentId
                            AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                       FROM   TWAObjective2ClassroomMeasure t2
                                                       WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                              AND t2.SetNumber IN ( 1, 2, 3, 4,
                                                                                    5, 6, 7, 8, 9 )))
            BEGIN
                IF ( 1 = 1 )
                  BEGIN
                      DECLARE @ws1ScoreTotal AS INT,
                              @ws2ScoreTotal AS INT
                      DECLARE @ws3ScoreTotal AS INT,
                              @ws4ScoreTotal AS INT
                      DECLARE @ws5ScoreTotal AS INT,
                              @ws6ScoreTotal AS INT
                      DECLARE @ws7ScoreTotal AS INT,
                              @ws8ScoreTotal AS INT
                      DECLARE @ws9ScoreTotal AS INT

                      SELECT @ws1ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND t2.SetNumber = 1)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws2ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND SetNumber = 2)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws3ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND t2.SetNumber = 3)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws4ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND SetNumber = 4)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws5ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND t2.SetNumber = 5)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws6ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND SetNumber = 6)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws7ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND t2.SetNumber = 7)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws8ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND SetNumber = 8)
                             AND Score IN ( 1, 9999 )

                      SELECT @ws9ScoreTotal = Count(*)
                      FROM   Members.dbo.LifetimeIndividualScore
                      WHERE  StudentId = @StudentId
                             AND ClassroomMeasureId IN (SELECT t2.ClassroomMeasureId
                                                        FROM   TWAObjective2ClassroomMeasure t2
                                                        WHERE  t2.TWASectionObjectiveId IN ( 15 )
                                                               AND SetNumber = 9)
                             AND Score IN ( 1, 9999 )

                      IF ( @ws1ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = 'Failed WS(1) Set 1 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws2ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 2 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws3ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 3 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws4ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 4 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws5ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 5 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws6ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 6 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws7ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 7 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws8ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 8 Recommend Plan 3'
                            SET @PlanId = 3
                        END
                      ELSE IF ( @ws9ScoreTotal < 4 )
                        BEGIN
                            SET @plan3_4Message = + @plan3_4Message
                                                  + ', Failed WS(1) Set 9 Recommend Plan 3'
                            SET @PlanId = 3
                        END

                      IF( @C11 >= 4 )
                        BEGIN
                            PRINT 'HappyHorse1'

                            SET @PlanId = -1
                        END
                      ELSE
                        BEGIN
                            IF( @C11 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappyHorse2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @C12 >= 4 )
                        BEGIN
                            PRINT 'HappyCow1'

                            SET @PlanId = -1
                        END
                      ELSE
                        BEGIN
                            IF( @C12 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappyCow2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @C21 >= 4 )
                        BEGIN
                            PRINT 'HappyCat1'

                            SET @PlanId = -1
                        END
                      ELSE
                        BEGIN
                            IF( @C21 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappyCat2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @C22 >= 4 )
                        BEGIN
                            PRINT 'HappyPig1'

                            SET @PlanId = -1
                        END
                      ELSE
                        BEGIN
                            IF( @C22 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappyPig2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @C31 >= 4 )
                        BEGIN
                            PRINT 'HappySnail1'

                            SET @PlanId = -1
                        END
                      ELSE
                        BEGIN
                            IF( @C31 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappySnail2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @C32 >= 4
                          AND @C32 IS NOT NULL )
                        BEGIN
                            PRINT 'HappyMouse'

                            SET @PlanId = 4
                        END
                      ELSE
                        BEGIN
                            IF( @C32 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappySnail2'

                                  SET @PlanId = 3
                              END
                        END

                      IF( @ws1ScoreTotal >= 4
                          AND @ws2ScoreTotal >= 4
                          AND @ws3ScoreTotal >= 4
                          AND @ws4ScoreTotal >= 4
                          AND @ws5ScoreTotal >= 4
                          AND @ws6ScoreTotal >= 4
                          AND @ws7ScoreTotal >= 4
                          AND @ws8ScoreTotal >= 4
                          AND @ws9ScoreTotal >= 4 )
                        BEGIN
                            SET @plan3_4Message = 'Passed WS(1) Sets 1-9 Recommend Plan 4'

                            PRINT 'passed all sets for WS 1'

                            SET @PlanId = 4

                            IF( @C11 >= 4 )
                              BEGIN
                                  PRINT 'HappyHorse1'

                                  SET @PlanId = -1
                              END
                            ELSE
                              BEGIN
                                  IF( @C11 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappyHorse2'

                                        SET @PlanId = 4
                                    END
                              END

                            IF( @C12 >= 4 )
                              BEGIN
                                  PRINT 'HappyCow1'

                                  SET @PlanId = -1
                              END
                            ELSE
                              BEGIN
                                  IF( @C12 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappyCow2'

                                        SET @PlanId = 4
                                    END
                              END

                            IF( @C21 >= 4 )
                              BEGIN
                                  PRINT 'HappyCat1'

                                  SET @PlanId = -1
                              END
                            ELSE
                              BEGIN
                                  IF( @C21 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappyCat2'

                                        SET @PlanId = 4
                                    END
                              END

                            IF( @C22 >= 4 )
                              BEGIN
                                  PRINT 'HappyPig1'

                                  SET @PlanId = -1
                              END
                            ELSE
                              BEGIN
                                  IF( @C22 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappyPig2'

                                        SET @PlanId = 4
                                    END
                              END

                            IF( @C31 >= 4 )
                              BEGIN
                                  PRINT 'HappySnail1'

                                  SET @PlanId = -1
                              END
                            ELSE
                              BEGIN
                                  IF( @C31 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappySnail2'

                                        SET @PlanId = 4
                                    END
                              END

                            IF( @C32 >= 4
                                AND @C32 IS NOT NULL )
                              BEGIN
                                  PRINT 'HappyMouse'

                                  SET @PlanId = 4
                              END
                            ELSE
                              BEGIN
                                  IF( @C32 IS NOT NULL )
                                    BEGIN
                                        PRINT 'HappySnail2'

                                        SET @PlanId = 4
                                    END
                              END
                        END
                  END

                SET @planResults = 'WS(1)1 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws1ScoreTotal, 0))
                                   + ', ' + 'WS(1)2 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws2ScoreTotal, 0))
                                   + ', ' + 'WS(1)3 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws3ScoreTotal, 0))
                                   + ', ' + 'WS(1)4 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws4ScoreTotal, 0))
                                   + ', ' + 'WS(1)5 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws5ScoreTotal, 0))
                                   + ', ' + 'WS(1)6 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws6ScoreTotal, 0))
                                   + ', ' + 'WS(1)7 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws7ScoreTotal, 0))
                                   + ', ' + 'WS(1)8 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws8ScoreTotal, 0))
                                   + ', ' + 'WS(1)9 Score Total: '
                                   + CONVERT(VARCHAR(10), Isnull(@ws9ScoreTotal, 0))
                                   + ', ' + Isnull(@plan3_4Message, '')

                PRINT @planResults
            END
      END

    IF ( Isnull(@PlanId, -1) <= 0 )
      BEGIN
          SET @PlanId = -1
      END

    SELECT @CurrentPlanId = TWAPlanId
    FROM   TWAStudent2Plan
    WHERE  StudentId = @StudentId

    SELECT Isnull(@PlanId, -1)        AS RecommendedPlanId,
           @StudentId                 AS StudentId,
           Isnull(@CurrentPlanId, -1) AS CurrentPlanId,
           @planResults               AS PlanResults,
           @OverQualified             AS OverQualified
END 
