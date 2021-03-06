USE [CustomerMgmt]
GO
/****** Object:  UserDefinedFunction [dbo].[_SALI_TwaStoryProgressionMatrixCheck]    Script Date: 05/20/2015 14:38:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--/*
--Story progression function
--*/
--/*
--Parameters Needed:
--	@LanguageID
--	@storyQuestionsMastery
--	@storySequencingMastery
--	@organizerMastery
--	@fluencySmoothnessMastery
--	@fluencyAccuracyMastery
--	@TeamTutoringPlanId 
--RETURNS:
--matrix of activities,storyid ect...
--*/
ALTER FUNCTION [dbo].[_SALI_TwaStoryProgressionMatrixCheck](@LanguageID               INT,
                                                            @storyQuestionsMastery    INT,
                                                            @sequencingMastery        INT,
                                                            @organizerMastery         INT,
                                                            @fluencySmoothnessMastery INT,
                                                            @fluencyAccuracyMastery   INT,
                                                            @TeamTutoringPlanId       INT,
                                                            @TeamTutoringPeriodId     INT)
--DECLARE
--	@LanguageID INT = 3,
--	@storyQuestionsMastery    INT = 16,
--    @sequencingMastery        INT = 17,
--    @organizerMastery         INT = 18,
--    @fluencySmoothnessMastery INT = 15,
--    @fluencyAccuracyMastery   INT = 14,
--    @TeamTutoringPlanId       INT = 614
--    @TeamTutoringPeriodId     INT


                                                           

--DECLARE @table TABLE (
--  ActivityId    INT,
--  SharedStoryId INT,
--  TemplateId    INT,
--  IncrimentVal  INT)

Returns @table TABLE (
  ActivityId    INT,
  SharedStoryId INT,
  TemplateId    INT,
  IncrimentVal  INT)
AS
  BEGIN
      ---------- Mis Declarations 
      DECLARE @i                    INT,
              @thisStudentId        INT,
              @tempValue            INT,
              @sharedStoryId        INT,
              @templateId           INT,
              @incrimentVal         INT = 1,
              @SchoolYearId         INT,
              @CustomerContactId    INT,
              @TeamId               INT,
              @StudentId            INT
      DECLARE @storyProgressionMatrix TABLE
        (
           ActivityId    INT,
           SharedStoryId INT,
           TemplateId    INT,
           IncrimentVal  INT
        )

      SELECT @sharedStoryId = SharedStoryId,
             @SchoolYearId = SchoolYearId,
             @CustomerContactId = CustomerContactId,
             @TeamId = @TeamId,
             @TeamTutoringPeriodId = TeamTutoringPeriodId
      FROM   TWATeamTutoringPeriod
      WHERE  TWATeamTutoringPeriod.TeamTutoringPeriodId = @TeamTutoringPeriodId

      SELECT @templateId = TutoringActivityTemplateId
      FROM   TWATeamTutoringPlan
      WHERE  TWATeamTutoringPlanId = @TeamTutoringPlanId

      /*
      Insert the Shared Story ID first asigned to the team (the default story)
      */
      INSERT @storyProgressionMatrix
      VALUES (16,
              @sharedStoryId,
              @templateId,
              0),
             (21,
              @sharedStoryId,
              @templateId,
              0),
             (17,
              @sharedStoryId,
              @templateId,
              0),
             (22,
              @sharedStoryId,
              @templateId,
              0),
             (18,
              @sharedStoryId,
              @templateId,
              0),
             (23,
              @sharedStoryId,
              @templateId,
              0),
             (11,
              @sharedStoryId,
              @templateId,
              0),
             (15,
              @sharedStoryId,
              @templateId,
              0),
             (19,
              @sharedStoryId,
              @templateId,
              0),
             (10,
              @sharedStoryId,
              @templateId,
              0),
             (14,
              @sharedStoryId,
              @templateId,
              0),
             (20,
              @sharedStoryId,
              @templateId,
              0)

      /*
      Check the mastery table for mastered or incrimented SharedStoryId and update per activity 
      */
      UPDATE @storyProgressionMatrix
      SET    SharedStoryId = resultsTable.SharedStoryId
      FROM   (SELECT TWAActivityId,
                     Max(SharedStoryId)AS 'SharedStoryId'
              FROM   TWAActivity2Student student
                     INNER JOIN TWAActivity2StudentLevelMastery mastery
                             ON mastery.TWAActivity2StudentId = student.TWAActivity2StudentId
              WHERE 
              mastery.TeamTutoringPeriodId = @TeamTutoringPeriodId 
              --student.StudentId IN (SELECT t.StudentId
              --                             FROM   TWATeamTutoringPeriodStudentInfo t
              --                             WHERE  t.TeamTutoringPeriodId = @TeamTutoringPeriodId)
              GROUP  BY TWAActivityId) AS resultsTable
      WHERE  ActivityId = resultsTable.TWAActivityId
             AND resultsTable.SharedStoryId > 0

      /*
      Apply the progression rules for the template of activities 
      */
      DECLARE @activities TABLE
        (
           activityId INT
        )

      IF( @templateId = 1 )
        BEGIN
            SET @sharedStoryId = @sharedStoryId
        END

      IF( @templateId = 2 )
        BEGIN
            /*
            
            */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (17),
                   (18),
                   (21),
                   (22),
                   (23)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sequencingMastery > 0
                AND @organizerMastery > 0
                AND @sharedStoryId < 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        /*
                        These Stories don't exist for the UK so skip them
                        */
                        SET @incrimentVal = 2
                    END

                  /*
                  Incriment the story for the specific activities 
                  */
                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
            
            */
            DELETE FROM @activities

            INSERT @activities
            VALUES(11),
                   (15),
                   (19)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencySmoothnessMastery > 0 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
              
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(10),
                   (14),
                   (20)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencyAccuracyMastery > 0 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sharedStoryId > 48 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
        END

      IF( @templateId = 3 )
        BEGIN
            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21),
                   (17),
                   (22),
                   (18),
                   (23)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sequencingMastery > 0
                AND @organizerMastery > 0
                AND @sharedStoryId < 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sharedStoryId >= 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES(11),
                   (15),
                   (19),
                   (10),
                   (14)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencySmoothnessMastery > 0
            and @fluencyAccuracyMastery > 0)
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
        END

      IF( @templateId = 4 )
      BEGIN
            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21),
                   (17),
                   (22),
                   (18),
                   (23)


            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sequencingMastery > 0
                AND @organizerMastery > 0
                AND @sharedStoryId < 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sharedStoryId >= 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES(11),
                   (15),
                   (19)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencySmoothnessMastery > 0)
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
               /*
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES (10),
                   (14),
                   (20)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencyAccuracyMastery > 0 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
        END
        IF( @templateId = 6 )
      BEGIN
            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21),
                   (17),
                   (22),
                   (18),
                   (23)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                AND @sequencingMastery > 0
                AND @organizerMastery > 0
                --AND @fluencySmoothnessMastery > 0
                --AND @fluencyAccuracyMastery > 0
                AND @sharedStoryId < 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                     */
            DELETE FROM @activities

            INSERT @activities
            VALUES(16),
                   (21)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @storyQuestionsMastery > 0
                --AND @fluencySmoothnessMastery > 0
                --AND @fluencyAccuracyMastery > 0
                AND @sharedStoryId > 49 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END

            /*
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES(11),
                   (15),
                   (19)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencySmoothnessMastery > 0)
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
               /*
                       */
            DELETE FROM @activities

            INSERT @activities
            VALUES (10),
                   (14),
                   (20)

            SELECT @sharedStoryId = Isnull(Max(SharedStoryId), @sharedStoryId)
            FROM   @storyProgressionMatrix
            WHERE  ActivityId IN (SELECT activityId
                                  FROM   @activities)

            IF( @fluencyAccuracyMastery > 0 )
              BEGIN
                  IF( @LanguageId = 3
                      AND @sharedStoryId IN ( 48, 50, 59, 61, 67 ) )
                    BEGIN
                        SET @incrimentVal = 2
                    END

                  UPDATE @storyProgressionMatrix
                  SET    SharedStoryId = SharedStoryId + @incrimentVal,
                         IncrimentVal = @incrimentVal
                  WHERE  ActivityId IN (SELECT activityId
                                        FROM   @activities)
              END
        END
            INSERT INTO @table
            SELECT *
            FROM   @storyProgressionMatrix

			 --select * from @table

            RETURN
        END
        
       
  
