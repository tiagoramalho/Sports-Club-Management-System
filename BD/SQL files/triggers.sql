use master
use CluSys
GO


DROP TRIGGER T_SessionNumber
GO
CREATE TRIGGER T_SessionNumber ON EvaluationSession AFTER INSERT AS
	DECLARE @EvalId int, @SessionId int, @SessionIdInserted int;
	SELECT @EvalId = EvalId FROM inserted;
	SELECT @SessionId = max(ID) FROM EvaluationSession WHERE EvalID = @EvalId
	SELECT @SessionIdInserted = ID FROM inserted;
	IF ( @SessionIdInserted != @SessionId)
		ROLLBACK TRAN;

GO
