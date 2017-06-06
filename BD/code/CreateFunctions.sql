USE p1g2;
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
                         OVER (
                           ORDER BY CASE WHEN Weight IS NULL
                             THEN 1
                                    ELSE 0 END, OpeningDate DESC ) AS Weight
            FROM MedicalEvaluation
            WHERE AthleteCC = @CC);
  END
GO

CREATE FUNCTION CluSys.F_GetHeight(@CC CHAR(12))
  RETURNS FLOAT AS
  BEGIN
    RETURN (SELECT TOP 1 FIRST_VALUE(Height)
                         OVER (
                           ORDER BY CASE WHEN Height IS NULL
                             THEN 1
                                    ELSE 0 END, OpeningDate DESC ) AS Height
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
  RETURN(SELECT
           CC,
           FirstName,
           MiddleName,
           LastName,
           Birthdate,
           Photo,
           Phone,
           Email,
           Password,
           Job,
           DominantSide,
           ModalityId
         FROM Athlete
         WHERE ModalityId = ISNULL(@ModalityId, ModalityId));
GO

CREATE FUNCTION CluSys.F_GetModalities()
  RETURNS TABLE AS
  RETURN(SELECT
           Name,
           RecognitionYear
         FROM Modality);
GO

CREATE FUNCTION CluSys.F_GetEvaluation(@EvalId INT)
  RETURNS TABLE AS
  RETURN(SELECT *
         FROM MedicalEvaluation
         WHERE Id = @EvalId);
GO

CREATE FUNCTION CluSys.F_GetEvaluations(@AthleteCC CHAR(12) = NULL)
  RETURNS TABLE AS
  RETURN(SELECT
           Id,
           Weight,
           Height,
           Story,
           OpeningDate,
           ClosingDate,
           ExpectedRecovery,
           AthleteCC,
           PhysiotherapistCC
         FROM MedicalEvaluation
         WHERE AthleteCC = ISNULL(@AthleteCC, AthleteCC));
GO

CREATE FUNCTION CluSys.F_GetSessions(@EvalId INT)
  RETURNS TABLE AS
  RETURN(SELECT
           EvalId,
           Id,
           Date
         FROM EvaluationSession
         WHERE EvalId = @EvalId);
GO

CREATE FUNCTION CluSys.F_GetBodyViews()
  RETURNS TABLE AS
  RETURN(SELECT
           Id,
           Image,
           [Order]
         FROM BodyChartView);
GO

CREATE FUNCTION CluSys.F_GetAnnotations()
  RETURNS TABLE AS
  RETURN(SELECT
           Symbol,
           Meaning
         FROM Annotation);
GO

CREATE FUNCTION CluSys.F_GetBodyChartMarks(@EvalId INT, @SessionId INT = NULL)
  RETURNS TABLE AS
  RETURN(SELECT
           Id,
           x,
           y,
           PainLevel,
           Description,
           EvalId,
           SessionId,
           ViewId
         FROM BodyChartMark
         WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO

CREATE FUNCTION CluSys.F_GetProblems(@EvalId INT, @SessionId INT = NULL)
  RETURNS TABLE AS
  RETURN(SELECT
           Id,
           Description,
           EvalId,
           SessionId
         FROM MajorProblem
         WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO

CREATE FUNCTION CluSys.F_GetTreatments(@EvalId INT, @SessionId INT = NULL)
  RETURNS TABLE AS
  RETURN(SELECT
           Id,
           Description,
           Objective,
           EvalId,
           SessionId,
           ProbId
         FROM TreatmentPlan
         WHERE EvalId = @EvalId AND SessionId = ISNULL(@SessionId, SessionId));
GO

CREATE FUNCTION CluSys.F_GetBodyAnnotations(@BodyId INT)
  RETURNS TABLE AS
  RETURN(SELECT
           Symbol,
           Meaning
         FROM Annotation
           JOIN BodyAnnotation ON Annotation.Symbol = BodyAnnotation.AnnotSym
         WHERE BodyId = @BodyId)
GO

CREATE FUNCTION CluSys.F_GetClass(@CC CHAR(12))
  RETURNS TABLE AS
  RETURN(SELECT
           Class.ModalityId,
           Class.Name
         FROM Athlete
           JOIN Class ON Athlete.ModalityId = Class.ModalityId
         WHERE Athlete.CC = @CC AND
               FLOOR(datediff(DAY, Athlete.Birthdate, getdate()) / 365.25) BETWEEN Class.InitialAge AND Class.FinalAge);
GO

CREATE FUNCTION CluSys.F_GetAthletesInfo ()
  RETURNS TABLE AS
  RETURN(SELECT
           CC AS CC,
           CONCAT(FirstName, ' ', LastName) AS Name,
           Birthdate,
           FLOOR(datediff(day, Birthdate, getdate()) / 365.25) AS Age,
           Phone,
           Email,
           DominantSide,
           ModalityId AS ModalityName,
           Name AS ClassName,
           Id AS EvaluationId,
           ExpectedRecovery,
           PhysiotherapistName
         FROM (SELECT
                 CC,
                 FirstName,
                 MiddleName,
                 LastName,
                 Birthdate,
                 Phone,
                 Email,
                 Job,
                 DominantSide,
                 Athlete.ModalityId,
                 Class.Name
               FROM Athlete
                 JOIN Class ON Athlete.ModalityId = Class.ModalityId
               WHERE Class.Name IN (SELECT NAME FROM CluSys.F_GetClass(Athlete.CC))) AS T1
           LEFT OUTER JOIN
           (SELECT
              Id,
              AthleteCC,
              ExpectedRecovery,
              PhysiotherapistCC
            FROM MedicalEvaluation) AS T2 ON T1.CC = T2.AthleteCC
           LEFT OUTER JOIN
           (SELECT
              CC AS PhysiotherapistCC2,
              CONCAT(FirstName, ' ', LastName) AS PhysiotherapistName
            FROM Physiotherapist) AS P ON T2.PhysiotherapistCC = P.PhysiotherapistCC2
         WHERE T2.Id IS NULL OR T2.Id IN (SELECT CluSys.F_ActiveEvaluation(T1.CC)));
GO

-- SELECT * FROM CluSys.F_GetAthletesInfo()
