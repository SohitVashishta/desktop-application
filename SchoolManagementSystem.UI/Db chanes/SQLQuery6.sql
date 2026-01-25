ALTER PROCEDURE usp_Class_Update
    @ClassId INT,
    @ClassName NVARCHAR(100),
    @GradeId INT,
    @Section NVARCHAR(10),
    @RoomNumber NVARCHAR(20),
    @MaxStudents INT,
    @IsActive BIT,
    @RowVersion ROWVERSION
AS
BEGIN
    UPDATE ClassMaster
    SET
        ClassName = @ClassName,
        GradeId = @GradeId,
        Section = @Section,
        RoomNumber = @RoomNumber,
        MaxStudents = @MaxStudents,
        IsActive = @IsActive
    WHERE
        ClassId = @ClassId
        AND RowVersion = @RowVersion;

    IF @@ROWCOUNT = 0
        THROW 50001, 'This record was modified by another user.', 1;
END;
