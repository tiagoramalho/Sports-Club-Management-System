USE [p1g2]
GO

DROP PROC CluSys.[P_GetOrCreateEvaluation]
GO
DROP PROC CluSys.[P_GetOrCreateSession]
GO
DROP PROC CluSys.[P_UpdateEvaluation]
GO
DROP PROC CluSys.[P_AddBodyChartMark]
GO
DROP PROC CluSys.[P_RmBodyChartMark]
GO
DROP PROC CluSys.[P_AddProblem]
GO
DROP PROC CluSys.[P_RmProblem]
GO
DROP PROC CluSys.[P_AddTreatment]
GO
DROP PROC CluSys.[P_RmTreatment]
GO
DROP PROC CluSys.[P_AddObservation]
GO
DROP PROC CluSys.[P_SetObservation]
GO
DROP PROC CluSys.[P_AddBodyAnnotation]
GO
DROP PROC CluSys.[P_AddModality]
GO
DROP PROC CluSys.[P_AddClass]
GO
DROP PROC CluSys.[P_AddAthlete]
GO

DROP FUNCTION CluSys.[F_ActiveEvaluation]
GO
DROP FUNCTION CluSys.[F_HasActiveEvaluation]
GO
DROP FUNCTION CluSys.[F_GetWeight]
GO
DROP FUNCTION CluSys.[F_GetHeight]
GO
DROP FUNCTION CluSys.[F_GetAthletesWithOpenEvaluations]
GO
DROP FUNCTION CluSys.[F_GetNumberOfProblems]
GO
DROP FUNCTION CluSys.[F_GetNumberOfTreatments]
GO
DROP FUNCTION CluSys.[F_GetOpenObservations]
GO
DROP FUNCTION CluSys.[F_GetAthletes]
GO
DROP FUNCTION CluSys.[F_GetModalities]
GO
DROP FUNCTION CluSys.[F_GetEvaluation]
GO
DROP FUNCTION CluSys.[F_GetEvaluations]
GO
DROP FUNCTION CluSys.[F_GetSessions]
GO
DROP FUNCTION CluSys.[F_GetBodyViews]
GO
DROP FUNCTION CluSys.[F_GetAnnotations]
GO
DROP FUNCTION CluSys.[F_GetBodyChartMarks]
GO
DROP FUNCTION CluSys.[F_GetProblems]
GO
DROP FUNCTION CluSys.[F_GetTreatments]
GO
DROP FUNCTION CluSys.[F_GetClass]
GO
DROP FUNCTION CluSys.[F_GetBodyAnnotations]
GO
DROP FUNCTION CluSys.[F_GetAthletesInfo]
GO

DROP TABLE [CluSys].[TreatmentPlan]
GO
DROP TABLE [CluSys].[Trains]
GO
DROP TABLE [CluSys].[SessionObservation]
GO
DROP TABLE [CluSys].[MedicalHistoryMedication]
GO
DROP TABLE [CluSys].[MedicalHistoryExams]
GO
DROP TABLE [CluSys].[MedicalHistory]
GO
DROP TABLE [CluSys].[MajorProblem]
GO
DROP TABLE [CluSys].[FunctionalTestResult]
GO
DROP TABLE [CluSys].[FunctionalTestSet]
GO
DROP TABLE [CluSys].[Coach]
GO
DROP TABLE [CluSys].[Class]
GO
DROP TABLE [CluSys].[BodyAnnotation]
GO
DROP TABLE [CluSys].[BodyChartMark]
GO
DROP TABLE [CluSys].[BodyChartView]
GO
DROP TABLE [CluSys].[Annotation]
GO
DROP TABLE [CluSys].[EvaluationSession]
GO
DROP TABLE [CluSys].[MedicalEvaluation]
GO
DROP TABLE [CluSys].[Physiotherapist]
GO
DROP TABLE [CluSys].[Athlete]
GO
DROP TABLE [CluSys].[Modality]
GO

DROP SCHEMA [CluSys]
GO
