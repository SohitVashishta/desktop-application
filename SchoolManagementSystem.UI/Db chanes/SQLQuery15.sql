CREATE PROCEDURE usp_DeleteSubject
    @SubjectId INT
AS
BEGIN
    UPDATE SubjectMaster
    SET IsActive = 0
    WHERE SubjectId = @SubjectId;
END
