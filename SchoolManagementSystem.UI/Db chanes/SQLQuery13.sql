CREATE PROCEDURE usp_AddSubject
    @SubjectName NVARCHAR(100),
    @GradeId INT,
    @CreatedBy NVARCHAR(50)
AS
BEGIN
    INSERT INTO SubjectMaster
    (
        SubjectName,
        GradeId,
        CreatedBy
    )
    VALUES
    (
        @SubjectName,
        @GradeId,
        @CreatedBy
    );
END
