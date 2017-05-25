USE CluSys
GO

CREATE FUNCTION F_ActiveEvaluation (@CC INT) RETURNS INT AS
BEGIN
    RETURN (SELECT Id FROM MedicalEvaluation WHERE AthleteCC = @CC AND ClosingDate IS NULL);
END
GO
SELECT F_ActiveEvaluation(124);
SELECT F_ActiveEvaluation(1247);
GO

CREATE FUNCTION F_HasActiveEvaluation (@CC INT) RETURNS BIT AS
BEGIN
    DECLARE @ret BIT;

    IF (SELECT F_ActiveEvaluation(@CC)) IS NULL
        SET @ret = 0;
    ELSE
        SET @ret = 1;
    RETURN @ret;
END
GO
SELECT F_HasActiveEvaluation (124);
SELECT F_HasActiveEvaluation (1247);
GO

