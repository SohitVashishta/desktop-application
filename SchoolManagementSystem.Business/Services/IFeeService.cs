using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;

public interface IFeeService
{
    // ================= Fee Head (MASTER) =================
    Task<List<FeeHeadModel>> GetFeeHeadsAsync();
    Task<List<FeeHeadModel>> GetFeeHeadsAsync(string feeType);
    Task SaveFeeHeadAsync(FeeHeadModel model);
    Task DeleteFeeHeadAsync(int id);

    // ================= Fee Structure =================
    Task SaveFeeStructureAsync(FeeStructureModel model);
    Task<List<FeeStructureDetailModel>> GetFeeStructureAsync(
        int academicYearId,
        int classId,
        string feeType);

    // ================= Fee Rules (Discount + Due Date) =================
    Task<List<FeeRuleRowModel>> GetFeeRulesAsync(
        int academicYearId,
        int classId,
        int? sectionId,
        string feeType);

    Task SaveFeeRulesAsync(
        int academicYearId,
        int classId,
        int? sectionId,
        string feeType,
        List<FeeRuleRowModel> rules);

    // ================= Student Fee =================
    Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model);
    Task<int> PayAsync(StudentFeePaymentModel model);
    Task RecordPaymentAsync(int feeId, decimal amount);
    Task RefundAsync(FeeRefundModel model);
    Task ApplyLateFeesAsync();
    Task ApplyFeeConcessionAsync(FeeConcessionModel model);

    // ================= Reports =================
    Task<List<FeeDto>> GetFeesAsync();
    Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to);
    Task<List<MonthlyCollectionModel>> GetMonthlyAsync();
    Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId);
    Task<FeesDashboardKpiModel> GetKpiAsync();

    // ================= Receipt =================
    Task<string> SaveReceiptAsync(FeeReceiptModel model);

    Task AssignFeeAsync(int studentId,
    int academicYearId,
    string feeType,
    decimal totalAmount,
    decimal totalDiscount,
    decimal netAmount,
    List<StudentFeeAssignmentDetailModel> details);
    Task<List<FeeReceiptModel>> GetReceiptsAsync(int studentId);
    
}
