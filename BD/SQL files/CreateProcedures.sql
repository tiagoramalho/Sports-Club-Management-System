USE CluSys
GO

CREATE PROC P_GetOrCreateEvaluation (@AthleteCC INT, @PhysiotherapistCC INT, @OpeningDate DATE = NULL) AS
  BEGIN
    IF @OpeningDate IS NULL
      SET @OpeningDate = GETDATE();

    IF F_HasActiveEvaluation(@AthleteCC) = 0
      INSERT INTO MedicalEvaluation (AthleteCC, PhysiotherapistCC, OpeningDate) VALUES (@AthleteCC, @PhysiotherapistCC, @OpeningDate);

    RETURN F_ActiveEvaluation(@AthleteCC);
  END
GO

CREATE PROC P_GetOrCreateSession (@AthleteCC INT, @PhysiotherapistCC INT, @EvalId INT = NULL, @Date DATE = NULL) AS
  BEGIN
    IF @EvalId IS NULL
      EXEC @EvalId = P_GetOrCreateEvaluation @AthleteCC, @PhysiotherapistCC;
    ELSE IF NOT EXISTS (SELECT COUNT(*) FROM MedicalEvaluation WHERE Id = @EvalId AND AthleteCC = @AthleteCC AND PhysiotherapistCC = @PhysiotherapistCC)
      RAISERROR ('Combinação de parâmetros inválida.', 16, 1);

    IF @Date IS NULL
      SET @Date = GETDATE();

    INSERT INTO EvaluationSession (EvalId, Date) VALUES (@EvalId, @Date);

    -- complete getting session id
    RETURN F_ActiveEvaluation(@AthleteCC);
  END
GO
