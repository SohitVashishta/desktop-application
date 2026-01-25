CREATE PROCEDURE usp_Class_ToggleStatus
    @ClassId INT,
    @ModifiedBy NVARCHAR(50)
AS
BEGIN
    UPDATE ClassMaster
    SET
        IsActive = ~IsActive,
        ModifiedOn = GETDATE(),
        ModifiedBy = @ModifiedBy
    WHERE ClassId = @ClassId;
END;
