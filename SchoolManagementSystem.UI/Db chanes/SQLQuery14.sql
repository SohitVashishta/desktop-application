CREATE PROCEDURE usp_UpdateSubject
    @SubjectId INT,
    @SubjectName NVARCHAR(100),
    @GradeId INT,
    @UpdatedBy NVARCHAR(50)
AS
BEGIN
    UPDATE SubjectMaster
    SET
        SubjectName = @SubjectName,
        GradeId = @GradeId,
        UpdatedOn = GETDATE(),
        UpdatedBy = @UpdatedBy
    WHERE SubjectId = @SubjectId;
END
