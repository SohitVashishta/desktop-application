CREATE PROCEDURE usp_GetSubjects
AS
BEGIN
    SELECT
        S.SubjectId,
        S.SubjectName,
        S.GradeId,
        G.GradeName,
        S.IsActive
    FROM SubjectMaster S
    INNER JOIN GradeMaster G ON G.GradeId = S.GradeId
    WHERE S.IsActive = 1
    ORDER BY G.GradeName, S.SubjectName;
END
