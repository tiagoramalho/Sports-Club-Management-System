USE p1g2
GO

CREATE FUNCTION CluSys.F_ActiveEvaluation(@CC CHAR(12))
  RETURNS INT AS
  BEGIN
    RETURN (SELECT Id
            FROM MedicalEvaluation
            WHERE AthleteCC = @CC AND ClosingDate IS NULL);
  END
GO

CREATE FUNCTION CluSys.F_HasActiveEvaluation(@CC CHAR(12))
  RETURNS BIT AS
  BEGIN
    DECLARE @ret BIT;

    If (SELECT TOP 1 1 FROM MedicalEvaluation WHERE AthleteCC = @CC) IS NULL
      SET @ret = 0;
    ELSE IF (SELECT CluSys.F_ActiveEvaluation(@CC)) IS NULL
      SET @ret = 0;
    ELSE
      SET @ret = 1;
    RETURN @ret;
  END
GO

CREATE FUNCTION CluSys.F_GetWeight(@CC CHAR(12))
  RETURNS TABLE AS
  RETURN SELECT TOP 1
           FIRST_VALUE(Weight)
           OVER (
             ORDER BY CASE WHEN Weight IS NULL
               THEN 1
                      ELSE 0 END, OpeningDate DESC ) AS Weight
         FROM MedicalEvaluation
         WHERE AthleteCC = @CC;
GO

CREATE FUNCTION CluSys.F_GetHeight(@CC CHAR(12))
  RETURNS TABLE AS
  RETURN SELECT TOP 1
           FIRST_VALUE(Height)
           OVER (
             ORDER BY CASE WHEN Height IS NULL
               THEN 1
                      ELSE 0 END, OpeningDate DESC ) AS Height
         FROM MedicalEvaluation
         WHERE AthleteCC = @CC;
GO

CREATE FUNCTION CluSys.F_GetAthletesWithOpenEvaluations()
  RETURNS TABLE AS
  RETURN(SELECT *
         FROM Athlete
         WHERE EXISTS(SELECT TOP 1 *
                      FROM MedicalEvaluation
                      WHERE AthleteCC = CC AND ClosingDate IS NULL))
GO

CREATE FUNCTION CluSys.F_GetNumberOfProblems(@EvalId INT, @SessionId INT)
  RETURNS INT AS
  BEGIN
    DECLARE @NumberOfRows INT;
    SELECT @NumberOfRows = COUNT(*)
    FROM MajorProblem
    WHERE EvalId = @EvalId AND SessionId = @SessionId;
    RETURN @NumberOfRows
  END
GO

CREATE FUNCTION CluSys.F_GetNumberOfTreatments(@EvalId INT, @SessionId INT)
  RETURNS INT AS
  BEGIN
    DECLARE @NumberOfRows INT;
    SELECT @NumberOfRows = COUNT(*)
    FROM TreatmentPlan
    WHERE EvalId = @EvalId AND SessionId = @SessionId;
    RETURN @NumberOfRows
  END
GO

CREATE FUNCTION CluSys.F_GetOpenObservations(@EvalId INT) RETURNS TABLE AS
  RETURN (SELECT * FROM SessionObservation WHERE EvalId = @EvalId AND DateClosed IS NULL);
GO
