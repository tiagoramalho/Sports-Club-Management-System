USE CluSys;
GO

CREATE TRIGGER AthleteCCCheck ON Athlete AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Physiotherapist)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Coach))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER PhysiotherapistCCCheck ON Physiotherapist AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Athlete)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Coach))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER CoachCCCheck ON Coach AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Athlete)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM Physiotherapist))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER EvaluationSessionIdIncrementer ON EvaluationSession INSTEAD OF INSERT AS
  INSERT INTO EvaluationSession
    SELECT i.EvalId, COALESCE(ES.Count, ROW_NUMBER() OVER (PARTITION BY i.EvalId ORDER BY i.EvalId, i.Date)), i.Date
    FROM inserted AS i
    LEFT JOIN (SELECT EvalId, MAX(Id) AS Count
               FROM EvaluationSession
               GROUP BY EvalId) AS ES
    ON i.EvalId = ES.EvalId
GO
