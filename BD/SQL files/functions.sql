USE CluSys
GO

DROP FUNCTION F_ActiveEvaluation;
GO
CREATE FUNCTION F_ActiveEvaluation (@CC INT) RETURNS INT AS
BEGIN
    RETURN (SELECT Id FROM MedicalEvaluation WHERE AthleteCC = @CC AND ClosingDate IS NULL);
END
GO
SELECT dbo.F_ActiveEvaluation(124);
SELECT dbo.F_ActiveEvaluation(1247);
GO

DROP FUNCTION F_HasActiveEvaluation;
GO
CREATE FUNCTION F_HasActiveEvaluation (@CC INT) RETURNS BIT AS
BEGIN
    DECLARE @ret BIT;

    IF (SELECT dbo.F_ActiveEvaluation(@CC)) IS NULL
        SET @ret = 0;
    ELSE
        SET @ret = 1;
    RETURN @ret;
END
GO
SELECT dbo.F_HasActiveEvaluation (124);
SELECT dbo.F_HasActiveEvaluation (1247);
GO

DROP PROC P_GetOrCreateEvaluation;
GO
CREATE PROC P_GetOrCreateEvaluation (@AthleteCC INT, @PhysiotherapistCC INT, @OpeningDate DATE = NULL) AS
BEGIN
    IF @OpeningDate IS NULL
        SET @OpeningDate = GETDATE();

    IF dbo.F_HasActiveEvaluation(@AthleteCC) = 0
        INSERT INTO MedicalEvaluation (AthleteCC, PhysiotherapistCC, OpeningDate) VALUES (@AthleteCC, @PhysiotherapistCC, @OpeningDate);

    RETURN dbo.F_ActiveEvaluation(@AthleteCC);
END
GO

DROP PROC P_GetOrCreateSession;
GO
CREATE PROC P_GetOrCreateSession (@AthleteCC INT, @PhysiotherapistCC INT, @EvalId INT = NULL, @Date DATE = NULL) AS
BEGIN
    IF @EvalId IS NULL
        EXEC @EvalId = P_GetOrCreateEvaluation @AthleteCC, @PhysiotherapistCC;
    ELSE IF NOT EXISTS (SELECT COUNT(*) FROM MedicalEvaluation WHERE Id = @EvalId AND AthleteCC = @AthleteCC AND PhysiotherapistCC = @PhysiotherapistCC)
        RAISERROR ('Combinação de parâmetros inválida.', 16, 1);

    IF @Date IS NULL
        SET @Date = GETDATE();

    INSERT INTO EvaluationSession (EvalId, DateSession) VALUES (@EvalId, @Date);

    -- complete getting session id
    RETURN dbo.F_ActiveEvaluation(@AthleteCC);
END
GO