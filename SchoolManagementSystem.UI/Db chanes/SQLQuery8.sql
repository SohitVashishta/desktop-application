ALTER PROCEDURE usp_Class_GetAll
AS
BEGIN
    SELECT
        c.ClassId,
        c.ClassName,
        g.GradeName,
        c.Section,
		tm.TeacherName,
        c.IsActive
    FROM ClassMaster c
    INNER JOIN GradeMaster g ON g.GradeId = c.GradeId
	INNER JOIN TeacherMaster tm on tm.teacherid=c.LeadTeacherId;
END;

