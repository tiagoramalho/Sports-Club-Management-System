USE CluSys
GO

CREATE PROC P_GetOrCreateEvaluation(@AthleteCC CHAR(12), @PhysiotherapistCC CHAR(12), @OpeningDate DATE = NULL, @EvalId INT OUTPUT) AS
  BEGIN
    IF @OpeningDate IS NULL
      SET @OpeningDate = GETDATE();

    IF dbo.F_HasActiveEvaluation(@AthleteCC) = 0
      INSERT INTO MedicalEvaluation (AthleteCC, PhysiotherapistCC, OpeningDate)
      VALUES (@AthleteCC, @PhysiotherapistCC, @OpeningDate);

    SET @EvalId = dbo.F_ActiveEvaluation(@AthleteCC);
    RETURN;
  END
GO

CREATE PROC P_GetOrCreateSession(@AthleteCC CHAR(12), @PhysiotherapistCC CHAR(12), @Date DATE = NULL, @EvalId INT = NULL, @SessionId INT OUTPUT) AS
  BEGIN
    IF @EvalId IS NULL
      EXEC P_GetOrCreateEvaluation @AthleteCC, @PhysiotherapistCC, @Date, @EvalId;
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

CREATE PROC P_UpdateEvaluation(@EvalId INT, @Weight DECIMAL(5, 2) = NULL, @Height DECIMAL(3, 2) = NULL, @Story NVARCHAR(MAX) = NULL, @ClosingDate DATE = NULL, @ExpectedRecovery Date = NULL) AS
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