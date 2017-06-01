USE p1g2;
GO

CREATE TRIGGER AthleteCCCheck ON CluSys.Athlete AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Physiotherapist)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Coach))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER PhysiotherapistCCCheck ON CluSys.Physiotherapist AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Athlete)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Coach))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER CoachCCCheck ON CluSys.Coach AFTER INSERT, UPDATE AS
  IF EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Athlete)) OR
     EXISTS(SELECT 1 FROM inserted WHERE CC IN (SELECT CC FROM CluSys.Physiotherapist))
    BEGIN
      RAISERROR ('CC already in use.', 10, 1);
      ROLLBACK TRAN;
    END
GO

CREATE TRIGGER EvaluationSessionIdIncrementer ON CluSys.EvaluationSession INSTEAD OF INSERT AS
  INSERT INTO CluSys.EvaluationSession
    SELECT i.EvalId, COALESCE(ES.Count, 0) + ROW_NUMBER() OVER (PARTITION BY i.EvalId ORDER BY i.EvalId, i.Date), i.Date
    FROM inserted AS i
    LEFT JOIN (SELECT EvalId, MAX(Id) AS Count
               FROM CluSys.EvaluationSession
               GROUP BY EvalId) AS ES
    ON i.EvalId = ES.EvalId
GO
