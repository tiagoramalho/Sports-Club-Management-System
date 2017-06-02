USE p1g2
GO

CREATE PROC CluSys.P_GetOrCreateEvaluation(@AthleteCC CHAR(12), @PhysiotherapistCC CHAR(12), @OpeningDate DATETIME = NULL, @EvalId INT OUTPUT) AS
  BEGIN
    IF @OpeningDate IS NULL
      SET @OpeningDate = GETDATE();

    IF CluSys.F_HasActiveEvaluation(@AthleteCC) = 0
      INSERT INTO MedicalEvaluation (AthleteCC, PhysiotherapistCC, OpeningDate)
      VALUES (@AthleteCC, @PhysiotherapistCC, @OpeningDate);

    SET @EvalId = Clusys.F_ActiveEvaluation(@AthleteCC);
    RETURN;
  END
GO

CREATE PROC CluSys.P_GetOrCreateSession(@AthleteCC CHAR(12), @PhysiotherapistCC CHAR(12), @Date DATETIME = NULL, @EvalId INT = NULL, @SessionId INT OUTPUT) AS
  BEGIN
    IF @EvalId IS NULL
      EXEC CluSys.P_GetOrCreateEvaluation @AthleteCC, @PhysiotherapistCC, @Date, @EvalId;
    ELSE IF NOT EXISTS(SELECT COUNT(*)
                       FROM MedicalEvaluation
                       WHERE Id = @EvalId AND AthleteCC = @AthleteCC AND PhysiotherapistCC = @PhysiotherapistCC)
      RAISERROR ('Wrong parameter combination.', 16, 1);

    IF @Date IS NULL
      SET @Date = GETDATE();

    INSERT INTO EvaluationSession (EvalId, Date) VALUES (@EvalId, @Date);
    SELECT @SessionId = MAX(Id) FROM EvaluationSession WHERE EvalId = @EvalId;
    RETURN;
  END
GO

CREATE PROC CluSys.P_UpdateEvaluation(@EvalId INT, @Weight DECIMAL(5, 2) = NULL, @Height DECIMAL(3, 2) = NULL, @Story NVARCHAR(MAX) = NULL, @ClosingDate DATETIME = NULL, @ExpectedRecovery DATETIME = NULL) AS
  BEGIN
    UPDATE MedicalEvaluation SET
      Weight = ISNULL(@Weight, Weight),
      Height = ISNULL(@Height, Height),
      Story = ISNULL(@Story, Story),
      ClosingDate = ISNULL(@ClosingDate, ClosingDate),
      ExpectedRecovery = ISNULL(@ExpectedRecovery, ExpectedRecovery)
    WHERE Id = @EvalId
  END
GO

CREATE PROC CluSys.P_AddBodyChartMark(@X FLOAT, @Y FLOAT, @PainLevel TINYINT = NULL, @Description NVARCHAR(MAX) = NULL, @EvalId INT, @SessionId INT, @ViewId INT, @MarkId INT OUTPUT) AS
  BEGIN
    INSERT INTO BodyChartMark (x, y, PainLevel, Description, EvalId, SessionId, ViewId) VALUES (@X, @Y, @PainLevel, @Description, @EvalId, @SessionId, @ViewId);
    SET @MarkId = SCOPE_IDENTITY();
  END
GO

CREATE PROC CluSys.P_RmBodyChartMark(@MarkId INT) AS
  BEGIN
    DELETE FROM BodyChartMark WHERE Id = @MarkId;
  END
GO

CREATE PROC CluSys.P_AddProblem(@Description NVARCHAR(MAX), @EvalId INT, @SessionId INT, @ProbId INT OUTPUT) AS
  BEGIN
    INSERT INTO MajorProblem (Description, EvalId, SessionId) VALUES (@Description, @EvalId, @SessionId);
    SET @ProbId = SCOPE_IDENTITY();
  END
GO

CREATE PROC CluSys.P_RmProblem(@ProbId INT) AS
  BEGIN
    DELETE FROM MajorProblem WHERE Id = @ProbId;
  END
GO

CREATE PROC CluSys.P_AddTreatment(@Description NVARCHAR(MAX) = NULL, @Objective NVARCHAR(MAX) = NULL, @EvalId INT, @SessionId INT, @ProbId INT = NULL, @TreatmentId INT OUTPUT) AS
  BEGIN
    INSERT INTO TreatmentPlan (Description, Objective, EvalId, SessionId, ProbId) VALUES (@Description, @Objective, @EvalId, @SessionId, @ProbId);
    SET @TreatmentId = SCOPE_IDENTITY();
  END
GO

CREATE PROC CluSys.P_RmTreatment(@TreatmentId INT) AS
  BEGIN
    DELETE FROM TreatmentPlan WHERE Id = @TreatmentId;
  END
GO

CREATE PROC CluSys.P_AddObservation(@Obs NVARCHAR(MAX), @DateClosed DATETIME = NULL, @EvalId INT, @SessionId INT, @ObsId INT OUTPUT) AS
  BEGIN
    INSERT INTO SessionObservation (Obs, DateClosed, EvalId, SessionId) VALUES (@Obs, @DateClosed, @EvalId, @SessionId);
    SET @ObsId = SCOPE_IDENTITY();
  END
GO

CREATE PROC CluSys.P_SetObservation(@ObsId INT, @Obs NVARCHAR(MAX) = NULL, @DateClosed DATETIME = NULL) AS
  BEGIN
    UPDATE SessionObservation SET Obs=ISNULL(@Obs, Obs), DateClosed = ISNULL(@DateClosed, DateClosed) WHERE Id = @ObsId;
  END
GO

CREATE PROC CluSys.P_AddBodyAnnotation(@MarkId INT, @AnnotSym VARCHAR(25)) AS
  BEGIN
    INSERT BodyAnnotation (BodyId, AnnotSym) VALUES (@MarkId, @AnnotSym);
  END
GO
 
CREATE PROC Clusys.P_AddModality(@Name VARCHAR(25), @Year SMALLINT) AS
    BEGIN
        INSERT Modality (Name, RecognitionYear) VALUES (@Name, @Year);
    END
GO

CREATE PROC Clusys.P_AddClass(@ModalityId VARCHAR(25), @Name VARCHAR(25), @InitialAge TINYINT, @FinalAge TINYINT) AS
    BEGIN
        INSERT Class (ModalityId, Name, InitialAge, FinalAge) VALUES (@ModalityId , @Name, @InitialAge, @FinalAge);
    END
GO

