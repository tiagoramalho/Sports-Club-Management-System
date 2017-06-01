USE [p1g2]
GO

DROP PROCEDURE [CluSys].[P_UpdateEvaluation]
GO
DROP PROCEDURE [CluSys].[P_GetOrCreateSession]
GO
DROP PROCEDURE [CluSys].[P_GetOrCreateEvaluation]
GO

DROP FUNCTION [CluSys].[F_GetWeight]
GO
DROP FUNCTION [CluSys].[F_GetOpenObservations]
GO
DROP FUNCTION [CluSys].[F_GetHeight]
GO
DROP FUNCTION [CluSys].[F_GetAthletesWithOpenEvaluations]
GO
DROP FUNCTION [CluSys].[F_HasActiveEvaluation]
GO
DROP FUNCTION [CluSys].[F_GetNumberOfTreatments]
GO
DROP FUNCTION [CluSys].[F_GetNumberOfProblems]
GO
DROP FUNCTION [CluSys].[F_ActiveEvaluation]
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
