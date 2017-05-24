-- Start clean
USE master;
GO
DROP DATABASE CluSys;
GO
CREATE DATABASE CluSys;
GO
USE CluSys;
GO

CREATE TABLE Modality (
  Name            NVARCHAR(25) NOT NULL,
  RecognitionYear SMALLINT     NOT NULL,

  PRIMARY KEY (Name)
);

CREATE TABLE Class (
  ModalityId NVARCHAR(25) NOT NULL,
  Name       NVARCHAR(25) NOT NULL,
  InitialAge TINYINT      NOT NULL,
  FinalAge   TINYINT      NOT NULL,

  PRIMARY KEY (ModalityId, Name),
  FOREIGN KEY (ModalityId) REFERENCES Modality (Name)
    ON UPDATE CASCADE
);

CREATE TABLE Athlete (
  CC           CHAR(12)     NOT NULL,
  FirstName    NVARCHAR(25) NOT NULL,
  MiddleName   NVARCHAR(25),
  LastName     NVARCHAR(25) NOT NULL,
  Birthdate    DATE         NOT NULL,
  Photo        VARCHAR(100),
  Phone        CHAR(9),
  Email        VARCHAR(50)  NOT NULL,
  Password     BINARY(64)   NOT NULL, -- SHA512 of password
  Job          VARCHAR(25),
  DominantSide VARCHAR(10)  NOT NULL,
  ModalityId   NVARCHAR(25),

  PRIMARY KEY (CC),
  UNIQUE (Phone),
  UNIQUE (Email),
  FOREIGN KEY (ModalityId) REFERENCES Modality (Name)
    ON UPDATE CASCADE
);

CREATE TABLE Physiotherapist (
  CC         CHAR(12)     NOT NULL,
  FirstName  NVARCHAR(25) NOT NULL,
  MiddleName NVARCHAR(25),
  LastName   NVARCHAR(25) NOT NULL,
  Birthdate  DATE         NOT NULL,
  Photo      VARCHAR(100),
  Phone      CHAR(9),
  Email      VARCHAR(50)  NOT NULL,
  Password   BINARY(64)   NOT NULL, -- SHA512 of password

  PRIMARY KEY (CC),
  UNIQUE (Phone),
  UNIQUE (Email)
);

CREATE TABLE Coach (
  CC         CHAR(12)     NOT NULL,
  FirstName  NVARCHAR(25) NOT NULL,
  MiddleName NVARCHAR(25),
  LastName   NVARCHAR(25) NOT NULL,
  Birthdate  DATE         NOT NULL,
  Photo      VARCHAR(100),
  Phone      CHAR(9),
  Email      VARCHAR(50)  NOT NULL,
  Password   BINARY(64)   NOT NULL, -- SHA512 of password
  Job        VARCHAR(25),

  PRIMARY KEY (CC),
  UNIQUE (Phone),
  UNIQUE (Email)
);

CREATE TABLE Trains (
  ModalityId NVARCHAR(25) NOT NULL,
  ClassName  NVARCHAR(25) NOT NULL,
  CoachCC    CHAR(12)     NOT NULL,
  Edition    SMALLINT     NOT NULL,

  PRIMARY KEY (ModalityId, ClassName, CoachCC, Edition),
  FOREIGN KEY (ModalityId, ClassName) REFERENCES Class (ModalityId, Name)
    ON UPDATE CASCADE,
  FOREIGN KEY (CoachCC) REFERENCES Coach (CC)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE MedicalHistory (
  Id                INT IDENTITY (1, 1) NOT NULL,
  Obs               NVARCHAR(MAX),
  Date              DATE                NOT NULL,
  AthleteCC         CHAR(12)            NOT NULL,
  PhysiotherapistCC CHAR(12)            NOT NULL,

  PRIMARY KEY (ID),
  FOREIGN KEY (AthleteCC) REFERENCES Athlete (CC)
    ON UPDATE CASCADE,
  FOREIGN KEY (PhysiotherapistCC) REFERENCES Physiotherapist (CC)
    ON UPDATE CASCADE
);

CREATE TABLE MedicalHistoryExams (
  MHId INT           NOT NULL,
  Exam NVARCHAR(100) NOT NULL,

  PRIMARY KEY (MHId, Exam),
  FOREIGN KEY (MHId) REFERENCES MedicalHistory (Id)
);

CREATE TABLE MedicalHistoryMedication (
  MHId       INT           NOT NULL,
  Medication NVARCHAR(100) NOT NULL,

  PRIMARY KEY (MHId, Medication),
  FOREIGN KEY (MHId) REFERENCES MedicalHistory (Id)
);

CREATE TABLE MedicalEvaluation (
  Id                INT IDENTITY (1, 1) NOT NULL,
  Weight            DECIMAL(5, 2),
  Height            DECIMAL(3, 2),
  Story             NVARCHAR(MAX),
  OpeningDate       DATE                NOT NULL,
  ClosingDate       DATE,
  ExpectedRecovery  DATE,
  AthleteCC         CHAR(12)            NOT NULL,
  PhysiotherapistCC CHAR(12)            NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (AthleteCC) REFERENCES Athlete (CC)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  FOREIGN KEY (PhysiotherapistCC) REFERENCES Physiotherapist (CC)
    ON UPDATE CASCADE
);

CREATE TABLE EvaluationSession (
  EvalId INT  NOT NULL,
  Id     INT  NOT NULL, -- sequential within the evaluation
  Date   DATE NOT NULL,

  PRIMARY KEY (EvalId, Id),
  FOREIGN KEY (EvalId) REFERENCES MedicalEvaluation (Id)
);

CREATE TABLE Annotation (
  Symbol  VARCHAR(10) NOT NULL,
  Meaning NVARCHAR(50),

  PRIMARY KEY (Symbol)
);

CREATE TABLE BodyChartView (
  Id      INT IDENTITY (1, 1) NOT NULL,
  Image   VARCHAR(100)        NOT NULL, -- Path
  [Order] TINYINT             NOT NULL,

  PRIMARY KEY (Id)
);

CREATE TABLE BodyChartMark (
  Id          INT IDENTITY (1, 1) NOT NULL,
  x           FLOAT               NOT NULL,
  y           FLOAT               NOT NULL,
  PainLevel   TINYINT,
  Description NVARCHAR(MAX),
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,
  ViewId      INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SEssionId) REFERENCES EvaluationSession (EvalId, Id),
  FOREIGN KEY (ViewId) REFERENCES BodyChartView (Id)
);

CREATE TABLE BodyAnnotation (
  BodyId   INT         NOT NULL,
  AnnotSym VARCHAR(10) NOT NULL,

  PRIMARY KEY (BodyId, AnnotSym),
  FOREIGN KEY (BodyId) REFERENCES BodyChartMark (Id),
  FOREIGN KEY (AnnotSym) REFERENCES Annotation (Symbol)
    ON UPDATE CASCADE
);

CREATE TABLE FunctionalTestSet (
  Name        NVARCHAR(25) NOT NULL,
  Description NVARCHAR(MAX),

  PRIMARY KEY (Name)
);

CREATE TABLE FunctionalTestResult (
  Id        INT IDENTITY (1, 1) NOT NULL,
  Result    NVARCHAR(MAX),
  EvalId    INT                 NOT NULL,
  SessionId INT                 NOT NULL,
  TestName  NVARCHAR(25)        NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES EvaluationSession (EvalId, Id),
  FOREIGN KEY (TestName) REFERENCES FunctionalTestSet (Name)
    ON UPDATE CASCADE
);

CREATE TABLE MajorProblem (
  Id          INT IDENTITY (1, 1) NOT NULL,
  Description NVARCHAR(MAX),
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES EvaluationSession (EvalId, Id)
);

CREATE TABLE TreatmentPlan (
  Id          INT IDENTITY (1, 1) NOT NULL,
  Description NVARCHAR(MAX),
  Objective   NVARCHAR(MAX),
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,
  ProbId      INT,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES EvaluationSession (EvalId, Id),
  FOREIGN KEY (ProbId) REFERENCES MajorProblem (Id)
);

CREATE TABLE SessionObservation (
  Id         INT IDENTITY (1, 1) NOT NULL,
  Obs        NVARCHAR(MAX),
  DateClosed DATE,
  EvalId     INT                 NOT NULL,
  SessionId  INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES EvaluationSession (EvalId, Id)
);
