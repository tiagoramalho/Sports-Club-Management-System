USE CluSys
GO

CREATE FUNCTION F_ActiveEvaluation(@CC CHAR(12))
  RETURNS INT AS
  BEGIN
    RETURN (SELECT Id
            FROM MedicalEvaluation
            WHERE AthleteCC = @CC AND ClosingDate IS NULL);
  END
GO
-- SELECT dbo.F_ActiveEvaluation('124');
-- SELECT dbo.F_ActiveEvaluation('1247');
GO

CREATE FUNCTION F_HasActiveEvaluation(@CC CHAR(12))
  RETURNS BIT AS
  BEGIN
    DECLARE @ret BIT;

    If (SELECT TOP 1 1 FROM MedicalEvaluation WHERE AthleteCC = @CC) IS NULL
      SET @ret = 0;
    ELSE IF (SELECT dbo.F_ActiveEvaluation(@CC)) IS NULL
      SET @ret = 0;
    ELSE
      SET @ret = 1;
    RETURN @ret;
  END
GO
-- SELECT dbo.F_HasActiveEvaluation('124');
-- SELECT dbo.F_HasActiveEvaluation('1247');
GO

CREATE FUNCTION F_GetWeightAndHeight(@CC CHAR(12))
  RETURNS TABLE AS
  RETURN SELECT TOP 1
           FIRST_VALUE(Weight)
           OVER (
             ORDER BY CASE WHEN Weight IS NULL
               THEN 1
                      ELSE 0 END, OpeningDate DESC ) AS Weight,
           FIRST_VALUE(Height)
           OVER (
             ORDER BY CASE WHEN Height IS NULL
               THEN 1
                      ELSE 0 END, OpeningDate DESC ) AS Height
         FROM MedicalEvaluation
         WHERE AthleteCC = @CC;
GO
-- SELECT * FROM F_GetWeightAndHeight('1243');
-- SELECT * FROM F_GetWeightAndHeight('1247');
GO

CREATE FUNCTION F_GetAthletesWithOpenEvaluations()
  RETURNS TABLE AS
  RETURN(SELECT *
         FROM Athlete
         WHERE EXISTS(SELECT TOP 1 *
                      FROM MedicalEvaluation
                      WHERE AthleteCC = CC AND ClosingDate IS NULL))
GO
-- SELECT * FROM F_GetAthletesWithOpenEvaluations();
GO

CREATE FUNCTION F_GetNumberOfProblems(@EvalId INT, @SessionId INT)
  RETURNS INT AS
  BEGIN
    DECLARE @NumberOfRows INT;
    SELECT @NumberOfRows = COUNT(*)
    FROM MajorProblem
    WHERE EvalId = @EvalId AND SessionId = @SessionId;
    RETURN @NumberOfRows
  END
GO
-- SELECT dbo.F_GetNumberOfProblems(2, 3) AS NumberOfProblems;
GO

CREATE FUNCTION F_GetNumberOfTreatments(@EvalId INT, @SessionId INT)
  RETURNS INT AS
  BEGIN
    DECLARE @NumberOfRows INT;
    SELECT @NumberOfRows = COUNT(*)
    FROM TreatmentPlan
    WHERE EvalId = @EvalId AND SessionId = @SessionId;
    RETURN @NumberOfRows
  END
GO

CREATE FUNCTION F_GetOpenObservations(@EvalId INT) RETURNS TABLE AS
  RETURN (SELECT * FROM SessionObservation WHERE EvalId = @EvalId AND DateClosed IS NULL);
GO
