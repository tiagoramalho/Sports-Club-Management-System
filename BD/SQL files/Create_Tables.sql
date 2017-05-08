-- Start clean
USE master;
DROP DATABASE CluSys;
CREATE DATABASE CluSys;
USE CluSys;

-- Create tables
CREATE TABLE Modality(
	Name		VARCHAR(20)		NOT NULL,
	RecognitionYear	SMALLINT	NOT NULL,
	PRIMARY KEY(Name));

CREATE TABLE Class(
	ModalityId	VARCHAR(20)		NOT NULL,
	Name		VARCHAR(20)	NOT NULL,
	InitialAge	TINYINT		NOT NULL,
	FinalAge	TINYINT		NOT NULL,
	PRiMARY KEY(ModalityId,Name),
	FOREIGN KEY(ModalityID) REFERENCES Modality (Name));

CREATE TABLE Athlete(
	CC			VARCHAR(12)	NOT NULL,
	FirstName	VARCHAR(15) NOT NULL,
	MiddleName	VARCHAR(25) ,
	LastName	VARCHAR(15) NOT NULL,
	Birthdate	DATE		NOT NULL,
	Photo		VARCHAR,
	Phone		VARCHAR(15)	NOT NULL,
	Email		VARCHAR(40)	NOT NULL,
	PWD			VARCHAR(20) NOT NULL,
	Job			VARCHAR(20),
	DominantSide VARCHAR(10) NOT NULL,
	ModalityId	VARCHAR(20)		NOT NULL,
	PRIMARY KEY(CC),
	UNIQUE(Email),
	FOREIGN KEY(ModalityID) REFERENCES Modality (Name));

CREATE TABLE Physiotherapist(
	CC			VARCHAR(12)	NOT NULL,
	FirstName	VARCHAR(15) NOT NULL,
	MiddleName	VARCHAR(25) ,
	LastName	VARCHAR(15) NOT NULL,
	Birthdate	DATE		NOT NULL,
	Photo		VARCHAR(100),
	Phone		VARCHAR(15)	NOT NULL,
	Email		VARCHAR(40)	NOT NULL,
	PWD			VARCHAR(20) NOT NULL,

	PRIMARY KEY(CC),
	UNIQUE(Email));

CREATE TABLE Coach(
	CC			VARCHAR(12)	NOT NULL,
	FirstName	VARCHAR(15) NOT NULL,
	MiddleName	VARCHAR(25) ,
	LastName	VARCHAR(15) NOT NULL,
	Birthdate	DATE		NOT NULL,
	Photo		VARCHAR,
	Phone		VARCHAR(15)	NOT NULL,
	Email		VARCHAR(40)	NOT NULL,
	PWD			VARCHAR(20) NOT NULL,
	Job			VARCHAR(20),
	PRIMARY KEY(CC),
	UNIQUE(Email));



CREATE TABLE Trains(
	ModalityId	VARCHAR(20)		NOT NULL,
	ClassName	VARCHAR(20)	NOT NULL,
	CoachCC		VARCHAR(12) NOT NULL,
	Edition		INT			NOT NULL,
	PRIMARY KEY(ModalityId,ClassName,CoachCC,Edition),
	FOREIGN KEY(CoachCC) REFERENCES Coach(CC),
	FOREIGN KEY(ModalityId) REFERENCES Modality(Name));

CREATE TABLE MedicalHistory(
	ID			INT			NOT NULL IDENTITY(1,1),
	Obs			VARCHAR(200),
	dateM		DATE		NOT NULL,
	AthleteCC	VARCHAR(12)	NOT NULL,
	PhysiotherapistCC VARCHAR(12)	NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY	(AthleteCC) REFERENCES Athlete(CC),
	FOREIGN KEY	(PhysiotherapistCC) REFERENCES Physiotherapist(CC));

CREATE TABLE MedicalHistoryExams(
	MHId		INT			NOT NULL,
	Exam		VARCHAR(100) NOT NULL,
	PRIMARY KEY (MHId,Exam),
	FOREIGN KEY (MHId) REFERENCES MedicalHistory(ID));

CREATE TABLE MedicalHistoryMedication(
	MHId		INT			NOT NULL,
	Medication	VARCHAR(100) NOT NULL,
	PRIMARY KEY (MHId,Medication),
	FOREIGN KEY (MHId) REFERENCES MedicalHistory(ID));

CREATE TABLE MedicalEvaluation(
	ID			INT			NOT NULL IDENTITY(1,1),
	Weightt		DECIMAL(5,2),
	Height		DECIMAL(3,2),
	Story		VARCHAR(200),
	OpeningDate	DATE		NOT NULL,
	ClosingDATE	DATE,
	ExpectedRecovery DATE,
	AthleteCC	VARCHAR(12)	NOT NULL,
	PhysiotherapistCC VARCHAR(12)	NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY	(AthleteCC) REFERENCES Athlete(CC),
	FOREIGN KEY	(PhysiotherapistCC) REFERENCES Physiotherapist(CC));

CREATE TABLE EvaluationSession(
	EvalId		INT			NOT NULL,
	ID			INT			NOT NULL,
	dateSession	DATE		NOT NULL,
	PRIMARY KEY(EvalId,ID),
	FOREIGN KEY(EvalId) REFERENCES MedicalEvaluation (ID));

CREATE TABLE Annotation(
	Symbol		nchar,
	Meaning		varchar(50)
	PRIMARY KEY (Symbol));

CREATE TABLE BodyChartView(
	ID			INT			NOT NULL IDENTITY(1,1),
	ImageBody	VARCHAR		NOT NULL,
	OrderImage	TINYINT		NOT NULL,
	PRIMARY KEY(ID));

CREATE TABLE BocyChartMark(
	ID			INT			NOT NULL IDENTITY(1,1),
	x			DECIMAL(8,4)NOT NULL,
	y			DECIMAL(8,4)NOT NULL,
	PainLevel	TINYINT,
	Obs			VARCHAR(200),
	EvalId		INT			NOT NULL,
	SessionId	INT			NOT NULL,
	ViewId		INT			NOT NULL,
	PRIMARY KEY(ID),
	FOREIGN KEY(EvalId, SEssionId) REFERENCES EvaluationSession(EvalId, ID),
	FOREIGN KEY(ViewId) REFERENCES BodyChartView(ID));

CREATE TABLE BodyAnnotation(
	BodyId		INT			NOT NULL,
	AnnotSym	nchar,
	PRIMARY KEY(BodyId,AnnotSym),
	FOREIGN KEY(BodyId) REFERENCES BocyChartMark(ID),
	FOREIGN KEY(AnnotSym) REFERENCES Annotation(Symbol));

CREATE TABLE FunctionalTestSet(
	Name		VARCHAR(20)	NOT NULL,
	Obs			VARCHAR(200),
	PRIMARY KEY(Name));

CREATE TABLE FunctionalTestResult(
	ID			INT			NOT NULL IDENTITY(1,1),
	Result		VARCHAR(200),
	EvalId		INT			NOT NULL,
	SessionId	INT			NOT NULL,
	TestName	VARCHAR(20)	NOT NULL,
	PRIMARY KEY(ID),
	FOREIGN KEY(EvalId, SEssionId) REFERENCES EvaluationSession(EvalId, ID),
	FOREIGN KEY(TestName) REFERENCES FunctionalTestSet(Name));

CREATE TABLE MajorProblem(
	ID			INT			NOT NULL IDENTITY(1,1),
	Obs			VARCHAR(200),
	EvalId		INT			NOT NULL,
	SessionId	INT			NOT NULL,
	PRIMARY KEY(ID),
	FOREIGN KEY(EvalId, SEssionId) REFERENCES EvaluationSession(EvalId, ID));

CREATE TABLE TreatmentPlan(
	ID			INT			NOT NULL IDENTITY(1,1),
	Obs			VARCHAR(200),
	objective	VARCHAR(200),
	EvalId		INT			NOT NULL,
	SessionId	INT			NOT NULL,
	ProbId		INT			NOT NULL,
	PRIMARY KEY(ID),
	FOREIGN KEY(EvalId, SEssionId) REFERENCES EvaluationSession(EvalId, ID),
	FOREIGN KEY(ProbId) REFERENCES MajorProblem(ID));

CREATE TABLE SessionObs(
	ID			INT			NOT NULL IDENTITY(1,1),
	Obs			VARCHAR(200),
	DateClosed	DATE,
	EvalId		INT			NOT NULL,
	SessionId	INT			NOT NULL,
	PRIMARY KEY(ID),
	FOREIGN KEY(EvalId, SEssionId) REFERENCES EvaluationSession(EvalId,ID));