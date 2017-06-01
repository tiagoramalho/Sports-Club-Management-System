USE p1g2;
GO

CREATE SCHEMA CluSys;
GO

CREATE TABLE CluSys.Modality (
  Name            NVARCHAR(25) NOT NULL,
  RecognitionYear SMALLINT     NOT NULL,

  PRIMARY KEY (Name)
);

CREATE TABLE CluSys.Class (
  ModalityId NVARCHAR(25) NOT NULL,
  Name       NVARCHAR(25) NOT NULL,
  InitialAge TINYINT      NOT NULL,
  FinalAge   TINYINT      NOT NULL,

  PRIMARY KEY (ModalityId, Name),
  FOREIGN KEY (ModalityId) REFERENCES CluSys.Modality (Name)
    ON UPDATE CASCADE,
  CHECK (FinalAge > InitialAge)
);

CREATE TABLE CluSys.Athlete (
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
  FOREIGN KEY (ModalityId) REFERENCES CluSys.Modality (Name)
    ON UPDATE CASCADE
);

CREATE TABLE CluSys.Physiotherapist (
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

CREATE TABLE CluSys.Coach (
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

CREATE TABLE CluSys.Trains (
  ModalityId NVARCHAR(25) NOT NULL,
  ClassName  NVARCHAR(25) NOT NULL,
  CoachCC    CHAR(12)     NOT NULL,
  Edition    SMALLINT     NOT NULL,

  PRIMARY KEY (ModalityId, ClassName, CoachCC, Edition),
  FOREIGN KEY (ModalityId, ClassName) REFERENCES CluSys.Class (ModalityId, Name)
    ON UPDATE CASCADE,
  FOREIGN KEY (CoachCC) REFERENCES CluSys.Coach (CC)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE CluSys.MedicalHistory (
  Id                INT IDENTITY (1, 1) NOT NULL,
  Obs               NVARCHAR(MAX),
  Date              DATE                NOT NULL,
  AthleteCC         CHAR(12)            NOT NULL,
  PhysiotherapistCC CHAR(12)            NOT NULL,

  PRIMARY KEY (ID),
  FOREIGN KEY (AthleteCC) REFERENCES CluSys.Athlete (CC)
    ON UPDATE CASCADE,
  FOREIGN KEY (PhysiotherapistCC) REFERENCES CluSys.Physiotherapist (CC)
    ON UPDATE CASCADE
);

CREATE TABLE CluSys.MedicalHistoryExams (
  MHId INT           NOT NULL,
  Exam NVARCHAR(100) NOT NULL,

  PRIMARY KEY (MHId, Exam),
  FOREIGN KEY (MHId) REFERENCES CluSys.MedicalHistory (Id)
);

CREATE TABLE CluSys.MedicalHistoryMedication (
  MHId       INT           NOT NULL,
  Medication NVARCHAR(100) NOT NULL,

  PRIMARY KEY (MHId, Medication),
  FOREIGN KEY (MHId) REFERENCES CluSys.MedicalHistory (Id)
);

CREATE TABLE CluSys.MedicalEvaluation (
  Id                INT IDENTITY (1, 1) NOT NULL,
  Weight            DECIMAL(5, 2),
  Height            DECIMAL(3, 2),
  Story             NVARCHAR(MAX),
  OpeningDate       DATETIME            NOT NULL,
  ClosingDate       DATETIME,
  ExpectedRecovery  DATETIME,
  AthleteCC         CHAR(12)            NOT NULL,
  PhysiotherapistCC CHAR(12)            NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (AthleteCC) REFERENCES CluSys.Athlete (CC)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  FOREIGN KEY (PhysiotherapistCC) REFERENCES CluSys.Physiotherapist (CC)
    ON UPDATE CASCADE,
  CHECK (Weight IS NULL OR Weight > 0),
  CHECK (Height IS NULL OR Height > 0),
  CHECK (ClosingDate IS NULL OR ClosingDate >= OpeningDate),
  CHECK (ExpectedRecovery IS NULL OR ExpectedRecovery >= OpeningDate)
);

CREATE TABLE CluSys.EvaluationSession (
  EvalId INT            NOT NULL,
  Id     INT DEFAULT -1 NOT NULL, -- sequential within the evaluation
  Date   DATETIME       NOT NULL,

  PRIMARY KEY (EvalId, Id),
  FOREIGN KEY (EvalId) REFERENCES CluSys.MedicalEvaluation (Id)
);

CREATE TABLE CluSys.Annotation (
  Symbol  VARCHAR(25) NOT NULL,
  Meaning NVARCHAR(50),

  PRIMARY KEY (Symbol)
);

CREATE TABLE CluSys.BodyChartView (
  Id      INT IDENTITY (1, 1) NOT NULL,
  Image   VARCHAR(100)        NOT NULL, -- Path
  [Order] TINYINT             NOT NULL,

  PRIMARY KEY (Id)
);

CREATE TABLE CluSys.BodyChartMark (
  Id          INT IDENTITY (1, 1) NOT NULL,
  x           FLOAT               NOT NULL,
  y           FLOAT               NOT NULL,
  PainLevel   TINYINT,
  Description NVARCHAR(MAX),
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,
  ViewId      INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SEssionId) REFERENCES CluSys.EvaluationSession (EvalId, Id),
  FOREIGN KEY (ViewId) REFERENCES CluSys.BodyChartView (Id)
);

CREATE TABLE CluSys.BodyAnnotation (
  BodyId   INT         NOT NULL,
  AnnotSym VARCHAR(25) NOT NULL,

  PRIMARY KEY (BodyId, AnnotSym),
  FOREIGN KEY (BodyId) REFERENCES CluSys.BodyChartMark (Id),
  FOREIGN KEY (AnnotSym) REFERENCES CluSys.Annotation (Symbol)
    ON UPDATE CASCADE
);

CREATE TABLE CluSys.FunctionalTestSet (
  Name        NVARCHAR(25) NOT NULL,
  Description NVARCHAR(MAX),

  PRIMARY KEY (Name)
);

CREATE TABLE CluSys.FunctionalTestResult (
  Id        INT IDENTITY (1, 1) NOT NULL,
  Result    NVARCHAR(MAX),
  EvalId    INT                 NOT NULL,
  SessionId INT                 NOT NULL,
  TestName  NVARCHAR(25)        NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES CluSys.EvaluationSession (EvalId, Id),
  FOREIGN KEY (TestName) REFERENCES CluSys.FunctionalTestSet (Name)
    ON UPDATE CASCADE
);

CREATE TABLE CluSys.MajorProblem (
  Id          INT IDENTITY (1, 1) NOT NULL,
  Description NVARCHAR(MAX)       NOT NULL,
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES CluSys.EvaluationSession (EvalId, Id)
);

CREATE TABLE CluSys.TreatmentPlan (
  Id          INT IDENTITY (1, 1) NOT NULL,
  Description NVARCHAR(MAX),
  Objective   NVARCHAR(MAX),
  EvalId      INT                 NOT NULL,
  SessionId   INT                 NOT NULL,
  ProbId      INT,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES CluSys.EvaluationSession (EvalId, Id),
  FOREIGN KEY (ProbId) REFERENCES CluSys.MajorProblem (Id)
);

CREATE TABLE CluSys.SessionObservation (
  Id         INT IDENTITY (1, 1) NOT NULL,
  Obs        NVARCHAR(MAX)       NOT NULL,
  DateClosed DATETIME,
  EvalId     INT                 NOT NULL,
  SessionId  INT                 NOT NULL,

  PRIMARY KEY (Id),
  FOREIGN KEY (EvalId, SessionId) REFERENCES CluSys.EvaluationSession (EvalId, Id)
);
