USE CluSys
GO

CREATE INDEX Idx_MedicalHistoryDate ON MedicalHistory (Date);
CREATE INDEX Idx_MedicalHistoryAthlete ON MedicalHistory (AthleteCC);
GO

CREATE INDEX Idx_MedicalEvaluationOpeningDate ON MedicalEvaluation (OpeningDate);
CREATE INDEX Idx_MedicalEvaluationAthlete ON MedicalEvaluation (AthleteCC);
GO

CREATE INDEX Idx_EvaluationSessionDate ON EvaluationSession (Date);
GO

CREATE INDEX Idx_BodyChartMarkEvalSession ON BodyChartMark (EvalId, SessionId);
GO

CREATE INDEX Idx_FunctionalTestResultEvalSession ON FunctionalTestResult (EvalId, SessionId);
GO

CREATE INDEX Idx_MajorProblemEvalSession ON MajorProblem (EvalId, SessionId);
GO

CREATE INDEX Idx_TreatmentPlanEvalSessionProb ON TreatmentPlan (EvalId, SessionId, ProbId);
GO

CREATE INDEX Idx_SessionObsEvalSession ON SessionObservation (EvalId, SessionId);
GO
