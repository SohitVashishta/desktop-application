CREATE PROCEDURE usp_AcademicYear_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        AcademicYearId,
        YearName,
        StartDate,
        EndDate,
        IsCurrent,
        IsActive
    FROM AcademicYearMaster
    ORDER BY StartDate DESC;
END
