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

    IF (SELECT TOP 1 1
        FROM MedicalEvaluation
        WHERE AthleteCC = @CC) IS NULL
      SET @ret = 0;
    ELSE IF (SELECT CluSys.F_ActiveEvaluation(@CC)) IS NULL
      SET @ret = 0;
    ELSE
      SET @ret = 1;
    RETURN @ret;
  END
GO

CREATE FUNCTION CluSys.F_GetWeight(@CC CHAR(12))
  RETURNS FLOAT AS
  BEGIN
    RETURN (SELECT TOP 1 FIRST_VALUE(Weight)
                        OVER ( ORDER BY CASE WHEN Weight IS NULL THEN 1 ELSE 0 END, OpeningDate DESC ) AS Weight
           FROM MedicalEvaluation
           WHERE AthleteCC = @CC);
  END
GO

CREATE FUNCTION CluSys.F_GetHeight(@CC CHAR(12))
  RETURNS FLOAT AS
  BEGIN
    RETURN (SELECT TOP 1 FIRST_VALUE(Height) OVER ( ORDER BY CASE WHEN Height IS NULL THEN 1 ELSE 0 END, OpeningDate DESC ) AS Height
           FROM MedicalEvaluation
           WHERE AthleteCC = @CC);
  END
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

CREATE FUNCTION CluSys.F_GetOpenObservations(@EvalId INT)
  RETURNS TABLE AS
  RETURN(SELECT *
         FROM SessionObservation
         WHERE EvalId = @EvalId AND DateClosed IS NULL);
GO

CREATE FUNCTION CluSys.F_GetAthletes(@ModalityId NVARCHAR(25) = NULL)
  RETURNS TABLE AS
  RETURN(SELECT CC, FirstName, MiddleName, LastName, Birthdate, Photo, Phone, Email, Password, Job, DominantSide, ModalityId FROM Athlete
         WHERE ModalityId = ISNULL(@ModalityId, ModalityId));
GO

CREATE FUNCTION CluSys.F_GetModalities()
  RETURNS TABLE AS
  RETURN(SELECT Name, RecognitionYear FROM Modality);
GO

CREATE FUNCTION CluSys.F_GetEvaluation(@EvalId INT) RETURNS TABLE AS
  RETURN (SELECT * FROM MedicalEvaluation WHERE Id = @EvalId);
GO

CREATE FUNCTION CluSys.F_GetEvaluations(@AthleteCC CHAR(12) = NULL) RETURNS TABLE AS
  RETURN (SELECT Id, Weight, Height, Story, OpeningDate, ClosingDate, ExpectedRecovery, AthleteCC, PhysiotherapistCC
          FROM MedicalEvaluation WHERE AthleteCC = ISNULL(@AthleteCC, AthleteCC));
GO

CREATE FUNCTION CluSys.F_GetSessions(@EvalId INT) RETURNS TABLE AS
  RETURN (SELECT EvalId, Id, Date FROM EvaluationSession WHERE EvalId = @EvalId);
GO

CREATE FUNCTION CluSys.F_GetBodyViews() RETURNS TABLE AS
  RETURN (SELECT Id, Image, [Order] FROM BodyChartView);
GO

CREATE FUNCTION CluSys.F_GetAnnotations() RETURNS TABLE AS
  RETURN (SELECT Symbol, Meaning FROM Annotation);
GO

CREATE FUNCTION CluSys.F_GetBodyChartMarks(@EvalId INT, @SessionId INT = NULL) RETURNS TABLE AS
  RETURN (SELECT Id, x, y, PainLevel, Description, EvalId, SessionId, ViewId FROM BodyChartMark WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO

CREATE FUNCTION CluSys.F_GetProblems(@EvalId INT, @SessionId INT = NULL) RETURNS TABLE AS
  RETURN (SELECT Id, Description, EvalId, SessionId FROM MajorProblem WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO

CREATE FUNCTION CluSys.F_GetTreatments(@EvalId INT, @SessionId INT = NULL) RETURNS TABLE AS
  RETURN (SELECT Id, Description, Objective, EvalId, SessionId, ProbId FROM TreatmentPlan WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO
