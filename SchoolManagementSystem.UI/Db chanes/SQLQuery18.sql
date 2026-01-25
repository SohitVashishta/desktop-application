CREATE PROCEDURE usp_AcademicYear_Update
(
    @AcademicYearId INT,
    @YearName NVARCHAR(20),
    @StartDate DATE,
    @EndDate DATE,
    @IsCurrent BIT,
    @IsActive BIT,
    @UpdatedBy NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@IsCurrent = 1)
    BEGIN
        UPDATE AcademicYearMaster
        SET IsCurrent = 0;
    END

    UPDATE AcademicYearMaster
    SET
        YearName = @YearName,
        StartDate = @StartDate,
        EndDate = @EndDate,
        IsCurrent = @IsCurrent,
        IsActive = @IsActive,
        UpdatedBy = @UpdatedBy,
        UpdatedOn = GETDATE()
    WHERE AcademicYearId = @AcademicYearId;
END
