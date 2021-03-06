USE [CustomerMgmt]
GO
/****** Object:  StoredProcedure [dbo].[_SALI_Report_TWA_MonitoringReport]    Script Date: 05/20/2015 14:49:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[_SALI_Report_TWA_MonitoringReport]
(
   @SchoolId AS INT
 , @SchoolYearId AS INT
 , @GradePeriodId AS INT
 , @SortOption AS INT
 , @TutorId AS INT = null
)
AS
BEGIN
	--exec _SALI_Report_TWA_MonitoringReport 26171,70,2,1,0
	--GET SCHOOL HIGH GRADING PERIOD ID
	DECLARE @HighGradingPeriodId int 
	SET @HighGradingPeriodId = Members.dbo._SFA_GetSchoolHighGradingPeriodId(@SchoolId, -1)
	
	IF @TutorId = 0 --RUN FOR ALL TUTORS
	BEGIN
		SET @TutorId = NULL
	END
	
	DECLARE @ActivityId INT = 14 --FLUENCY-ACCURACY (TWA MASTERY IS BASED OFF OF THIS ACTIVITY IN THIS REPORT)

	;WITH PartialResults AS (		
	SELECT DISTINCT studentInfo.StudentId, cc.CustomerContactId, LEFT(cc.FirstName, 1) + LEFT(cc.LastName, 1) AS TutorInitials
	FROM   vw_TWATeamTutoringPeriod period WITH (NOLOCK)
		 INNER JOIN vw_CustomersStaff cc WITH (NOLOCK)
				 ON cc.CustomerContactId = period.CustomerContactId		 
		 INNER JOIN TWATeam team WITH (NOLOCK)
				 ON team.Id = period.TeamId
		 INNER JOIN vw_TWATeamTutoringPeriodStudentInfo studentInfo WITH (NOLOCK)
				 ON studentInfo.TeamTutoringPeriodId = period.TeamTutoringPeriodId
		 INNER JOIN TWAStudent2Plan studentPlan WITH (NOLOCK)
				 ON studentPlan.StudentId = studentInfo.StudentId
		 INNER JOIN Students studentsView WITH (NOLOCK)
				 ON studentsView.StudentId = studentInfo.StudentId
	WHERE cc.CustomerId = @SchoolId
		 AND (period.CustomerContactId = @TutorId OR isnull(@TutorId, -1) = -1) --TUTOR FILTER OPTIONAL
		 AND period.SchoolYearId = Isnull(@SchoolYearId, period.SchoolYearId)
		 --AND team.GradingPeriodId = Isnull(@GradePeriodId, team.GradingPeriodId)
	)
	SELECT s.CustomerContactId, s.TutorInitials, dbo._DataTools_GetTeacherInitials(@SchoolYearId, @GradePeriodId, s.StudentId, 1) AS ReadingTeacherInitials
		, dbo._DataTools_GetStudentPlacement(@SchoolId, @SchoolYearId, @GradePeriodId, s.StudentId, 1) AS InstructionalPlacement
		, st.StudentId, st.FirstName, st.LastName, g.Grade
		, r.GroupHeading1DisplayOrder, r.GroupHeading1, r.GroupHeading2DisplayOrder, r.GroupHeading2, 
		CASE WHEN r.ReportDataTypeId = 1 THEN
			CAST(dbo._SALI_GetTWAStudentMasteryByGradingPeriod(@SchoolId, @SchoolYearId, r.GradingPeriodId, @ActivityId, st.StudentId) AS VARCHAR)
		WHEN r.ReportDataTypeId = 2 THEN
			CAST(dbo._SALI_GetTWAStudentExpectedMasteryByGradingPeriod(@SchoolId, r.GradingPeriodId, sgp.GradeId) AS VARCHAR)
		WHEN r.ReportDataTypeId = 3 THEN
			CAST(dbo._SALI_GetTWAStudentSessionCountByGradingPeriod(@SchoolId, @SchoolYearId, r.GradingPeriodId, st.StudentId) AS VARCHAR)
		WHEN r.ReportDataTypeId = 4 THEN
			CAST(dbo._DataTools_GetStudentGradingPeriodMasteryLevel(@SchoolYearId, r.GradingPeriodId, st.StudentId) AS VARCHAR)
		END AS Value
	FROM StudentGradingPeriods sgp WITH (NOLOCK)
		INNER JOIN PartialResults s ON s.StudentId = sgp.StudentId
		INNER JOIN Students st WITH (NOLOCK) ON st.StudentId = s.StudentId
		LEFT OUTER JOIN Grades g WITH (NOLOCK) ON g.GradeId = sgp.GradeId
		LEFT OUTER JOIN MasteryLevels ml WITH (NOLOCK) ON ml.MasteryLevelId = sgp.MasteryLevelId
		CROSS JOIN TWAReportDefinitionMonitoringReport r WITH (NOLOCK)
	WHERE sgp.SchoolYearId = @SchoolYearId
		AND sgp.GradingPeriodId = @GradePeriodId
		AND r.GradingPeriodId <= @HighGradingPeriodId
END