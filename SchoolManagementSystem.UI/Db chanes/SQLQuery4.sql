CREATE TABLE GradeMaster (
    GradeId        INT IDENTITY(1,1) PRIMARY KEY,
    GradeName      NVARCHAR(50) NOT NULL,
    IsActive       BIT NOT NULL DEFAULT 1,
    CreatedOn      DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE TeacherMaster (
    TeacherId      INT IDENTITY(1,1) PRIMARY KEY,
    TeacherName    NVARCHAR(100) NOT NULL,
    IsActive       BIT NOT NULL DEFAULT 1,
    CreatedOn      DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE ClassMaster (
    ClassId              INT IDENTITY(1,1) PRIMARY KEY,

    ClassName            NVARCHAR(100) NOT NULL,
    GradeId              INT NOT NULL,
    Section              NVARCHAR(10) NOT NULL,

    RoomNumber           NVARCHAR(20) NOT NULL,
    MaxStudents          INT NOT NULL CHECK (MaxStudents > 0),

    LeadTeacherId        INT NULL,
    AssistantTeacherId  INT NULL,

    IsActive             BIT NOT NULL DEFAULT 1,

    CreatedOn            DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy            NVARCHAR(50) NULL,

    ModifiedOn           DATETIME2 NULL,
    ModifiedBy           NVARCHAR(50) NULL,

    CONSTRAINT FK_Class_Grade
        FOREIGN KEY (GradeId) REFERENCES GradeMaster(GradeId),

    CONSTRAINT FK_Class_LeadTeacher
        FOREIGN KEY (LeadTeacherId) REFERENCES TeacherMaster(TeacherId),

    CONSTRAINT FK_Class_AssistantTeacher
        FOREIGN KEY (AssistantTeacherId) REFERENCES TeacherMaster(TeacherId),

    CONSTRAINT UQ_Class UNIQUE (GradeId, Section)
);

select * from ClassMaster
ALTER TABLE ClassMaster
ADD RowVersion ROWVERSION;

INSERT INTO GradeMaster (GradeName)
VALUES
('Pre-Nursery'),
('Nursery'),
('LKG'),
('UKG'),
('Grade 1'),
('Grade 2'),
('Grade 3'),
('Grade 4'),
('Grade 5'),
('Grade 6'),
('Grade 7'),
('Grade 8'),
('Grade 9'),
('Grade 10'),
('Grade 11'),
('Grade 12');

CREATE TABLE SectionMaster (
    SectionId     INT IDENTITY(1,1) PRIMARY KEY,
    SectionName   NVARCHAR(10) NOT NULL,
    IsActive      BIT NOT NULL DEFAULT 1,
    CreatedOn     DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_Section UNIQUE (SectionName)
);

INSERT INTO SectionMaster (SectionName)
VALUES
('A'),
('B'),
('C'),
('D'),
('E');

INSERT INTO TeacherMaster (TeacherName)
VALUES
('Amit Sharma'),
('Neha Verma'),
('Rohit Singh'),
('Pooja Mehta');
select * from  TeacherMaster;
GO

CREATE TABLE TeacherMaster (
    TeacherId     INT IDENTITY(1,1) PRIMARY KEY,
    TeacherName   NVARCHAR(100) NOT NULL,
    IsActive      BIT NOT NULL DEFAULT 1,
    CreatedOn     DATETIME2 NOT NULL DEFAULT GETDATE()
);

Select * from TeacherMaster
drop table teacherMaster

ALTER TABLE ClassMaster
DROP CONSTRAINT FK_Class_LeadTeacher;

ALTER TABLE ClassMaster
DROP CONSTRAINT FK_Class_AssistantTeacher;

ALTER TABLE ClassMaster
ADD CONSTRAINT FK_Class_LeadTeacher
FOREIGN KEY (LeadTeacherId)
REFERENCES TeacherMaster(TeacherId);

ALTER TABLE ClassMaster
ADD CONSTRAINT FK_Class_AssistantTeacher
FOREIGN KEY (AssistantTeacherId)
REFERENCES TeacherMaster(TeacherId);


CREATE TABLE TeacherMaster
(
    TeacherId     INT IDENTITY(1,1) PRIMARY KEY,

    TeacherName   NVARCHAR(150) NOT NULL,
    FirstName     NVARCHAR(100) NOT NULL,
    LastName      NVARCHAR(100) NOT NULL,

    Email         NVARCHAR(150) NOT NULL,
    Subject       NVARCHAR(100) NOT NULL,

    IsActive      BIT NOT NULL DEFAULT 1,

    CreatedDate   DATETIME2 NOT NULL DEFAULT GETDATE()
);

INSERT INTO TeacherMaster
(
    TeacherName,
    FirstName,
    LastName,
    Email,
    Subject
)
VALUES
('Anjali Gupta', 'Anjali', 'Gupta', 'anjali.gupta@school.com', 'Physics');

CREATE TABLE ClassMaster
(
    ClassId INT IDENTITY(1,1) PRIMARY KEY,

    -- ================= BASIC INFO =================
    ClassName NVARCHAR(100) NOT NULL,
    GradeId INT NOT NULL,
    Section NVARCHAR(10) NOT NULL,
    RoomNumber NVARCHAR(20)  NULL,
    MaxStudents INT  NULL CHECK (MaxStudents > 0),

    -- ================= TEACHERS =================
    LeadTeacherId INT NULL,
    AssistantTeacherId INT NULL,

    -- ================= STATUS =================
    IsActive BIT NOT NULL DEFAULT 1,

    -- ================= AUDIT =================
    CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedBy NVARCHAR(50) NULL,

    ModifiedOn DATETIME2 NULL,
    ModifiedBy NVARCHAR(50) NULL,

    -- ================= CONCURRENCY =================
    RowVersion ROWVERSION NOT NULL,

    -- ================= CONSTRAINTS =================
    CONSTRAINT FK_ClassMaster_Grade
        FOREIGN KEY (GradeId)
        REFERENCES GradeMaster (GradeId),

    CONSTRAINT FK_ClassMaster_LeadTeacher
        FOREIGN KEY (LeadTeacherId)
        REFERENCES TeacherMaster (TeacherId),

    CONSTRAINT FK_ClassMaster_AssistantTeacher
        FOREIGN KEY (AssistantTeacherId)
        REFERENCES TeacherMaster (TeacherId),

    -- One section per grade (VERY IMPORTANT)
    CONSTRAINT UQ_ClassMaster_Grade_Section
        UNIQUE (GradeId, Section)
);


CREATE TABLE SubjectMaster
(
    SubjectId INT IDENTITY(1,1) PRIMARY KEY,
    SubjectName NVARCHAR(100) NOT NULL,

    GradeId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,

    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50),

    UpdatedOn DATETIME NULL,
    UpdatedBy NVARCHAR(50),

    CONSTRAINT FK_Subject_Grade
        FOREIGN KEY (GradeId) REFERENCES GradeMaster(GradeId)
);


CREATE TABLE AcademicYearMaster
(
    AcademicYearId INT IDENTITY(1,1) PRIMARY KEY,

    YearName NVARCHAR(20) NOT NULL,     -- e.g. 2024-2025

    StartDate DATE NOT NULL,
    EndDate   DATE NOT NULL,

    IsCurrent BIT NOT NULL DEFAULT 0,
    IsActive  BIT NOT NULL DEFAULT 1,

    CreatedBy NVARCHAR(50) NOT NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedBy NVARCHAR(50) NULL,
    UpdatedOn DATETIME NULL
);


INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2024-2025', '2024-04-01', '2025-03-31', 1, 1, 'System');

UPDATE AcademicYearMaster
SET IsCurrent = 0;


INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2021-2022', '2021-04-01', '2022-03-31', 0, 1, 'System');


INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2022-2023', '2022-04-01', '2023-03-31', 0, 1, 'System');

INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2023-2024', '2023-04-01', '2024-03-31', 0, 1, 'System');

INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2024-2025', '2024-04-01', '2025-03-31', 1, 1, 'System');

INSERT INTO AcademicYearMaster
(YearName, StartDate, EndDate, IsCurrent, IsActive, CreatedBy)
VALUES
('2026-2027', '2026-04-01', '2027-03-31', 0, 1, 'System');



