alter PROCEDURE usp_Class_Add
    @ClassName NVARCHAR(100),
    @GradeId INT,
    @Section NVARCHAR(10),
    @LeadTeacherId INT = NULL
AS
BEGIN
    INSERT INTO ClassMaster (
        ClassName, GradeId, Section,
       LeadTeacherId,
        CreatedBy
    )
    VALUES (
        @ClassName, @GradeId, @Section, @LeadTeacherId,
        'Admin'
    );
END;

