CREATE PROCEDURE usp_AcademicYear_Add
(
    @YearName NVARCHAR(20),
    @StartDate DATE,
    @EndDate DATE,
    @IsCurrent BIT,
    @CreatedBy NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@IsCurrent = 1)
    BEGIN
        UPDATE AcademicYearMaster
        SET IsCurrent = 0;
    END

    INSERT INTO AcademicYearMaster
    (
        YearName,
        StartDate,
        EndDate,
        IsCurrent,
        IsActive,
        CreatedBy
    )
    VALUES
    (
        @YearName,
        @StartDate,
        @EndDate,
        @IsCurrent,
        1,
        @CreatedBy
    );
END
