USE CluSys;
SELECT Weight, Height, ClosingDATE FROM
	(SELECT Weight, Height, OpeningDate, ClosingDATE FROM MedicalEvaluation WHERE AthleteCC=1247)  AS T
	WHERE OpeningDate >= all (SELECT OpeningDate FROM MedicalEvaluation WHERE AthleteCC=1247);

SELECT * FROM Athlete WHERE CC in (SELECT AthleteCC FROM MedicalEvaluation WHERE ClosingDate is NULL);

SELECT * FROM MedicalEvaluation WHERE AthleteCC=1247;

SELECT * FROM EvaluationSession WHERE EvalId = 1;

SELECT * FROM MajorProblem;

SELECT * FROM TreatmentPlan;

SELECT COUNT (*) as NumberProblems FROM MajorProblem WHERE EvalId = 1 and SessionId = 1;

SELECT COUNT (*) as NumberTreatments FROM TreatmentPlan WHERE EvalId = 1 and SessionId = 1;

--SELECT T.EvalId, T.ID, T.dateSession, T.ProbId, P.ID FROM (SELECT E.EvalId, E.ID, E.dateSession, M.ID as ProbId FROM (EvaluationSession as E full outer join MajorProblem as M ON E.EvalId = M.EvalId and E.ID=M.SessionId)) as T full outer join TreatmentPlan as P on T.EvalId = P.EvalId and T.ID = P.SessionId ; 

SELECT * FROM MedicalEvaluation WHERE ID = 1;
UPDATE MedicalEvaluation SET Height = 1.0 WHERE ID = 1;

SELECT ExpectedRecovery FROM MedicalEvaluation WHERE AthleteCC = '1241' and ClosingDATE is null;
SELECT * FROM MedicalEvaluation
