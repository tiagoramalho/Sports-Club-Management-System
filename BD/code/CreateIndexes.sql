USE p1g2
GO

CREATE INDEX Idx_MedicalHistoryDate ON CluSys.MedicalHistory (Date);
CREATE INDEX Idx_MedicalHistoryAthlete ON CluSys.MedicalHistory (AthleteCC);
GO

CREATE INDEX Idx_MedicalEvaluationOpeningDate ON CluSys.MedicalEvaluation (OpeningDate);
CREATE INDEX Idx_MedicalEvaluationAthlete ON CluSys.MedicalEvaluation (AthleteCC);
GO

CREATE INDEX Idx_EvaluationSessionDate ON CluSys.EvaluationSession (Date);
GO

CREATE INDEX Idx_BodyChartMarkEvalSession ON CluSys.BodyChartMark (EvalId, SessionId);
GO

CREATE INDEX Idx_FunctionalTestResultEvalSession ON CluSys.FunctionalTestResult (EvalId, SessionId);
GO

CREATE INDEX Idx_MajorProblemEvalSession ON CluSys.MajorProblem (EvalId, SessionId);
GO

CREATE INDEX Idx_TreatmentPlanEvalSessionProb ON CluSys.TreatmentPlan (EvalId, SessionId, ProbId);
GO

CREATE INDEX Idx_SessionObsEvalSession ON CluSys.SessionObservation (EvalId, SessionId);
GO
