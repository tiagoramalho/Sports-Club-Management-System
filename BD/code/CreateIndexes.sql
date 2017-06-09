USE p1g2
GO
--Índices criados porque a Historia Médica de um atleta e filtrada por data e por numero do cartão do cidadão
--	do atleta
CREATE INDEX Idx_MedicalHistoryDate ON CluSys.MedicalHistory (Date);
CREATE INDEX Idx_MedicalHistoryAthlete ON CluSys.MedicalHistory (AthleteCC);
GO

--Indice criado porque as avaliações medicas sao filtradas por data de abertura na time line, e por atleta 
--	para serem apresentadas na página do mesmo 
CREATE INDEX Idx_MedicalEvaluationOpeningDate ON CluSys.MedicalEvaluation (OpeningDate);
CREATE INDEX Idx_MedicalEvaluationAthlete ON CluSys.MedicalEvaluation (AthleteCC);
GO

--Indice criado porque as sessoes de avaliação sao filtradas por data na atime line
CREATE INDEX Idx_EvaluationSessionDate ON CluSys.EvaluationSession (Date);
GO


--Todos os indices criados porque na consulta das fichas das consultas clinicas e preciso filtrar 
--	todas as marcas no body chart, os teste funcionais, os problemas, os tratamentos e as observaçoes 
--	por o id da avaliação e da sessão correspondente

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
