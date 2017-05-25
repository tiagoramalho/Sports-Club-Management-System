USE MASTER
USE Clusys
GO

DROP PROC CreateSession;
GO
CREATE PROC CreateSession(@EvalId INT, @DateSession DATE) AS
	DECLARE @SessionId int;
	SELECT @SessionId = max(ID) FROM EvaluationSession WHERE EvalId = @EvalId;
	IF (@SessionId is null)
		BEGIN
			SET @SessionId = 1;
			INSERT EvaluationSession VALUES(@EvalId, @DateSession, @SessionId);
		END
	ELSE
		BEGIN
			SET @SessionId += 1;
			INSERT EvaluationSession VALUES(@EvalId, @DateSession, @SessionId);
		END
GO
--EXEC CreateSession  '1', '20150622';
--SELECT * FROM EvaluationSession;
