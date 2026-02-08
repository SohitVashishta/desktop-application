using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IFeeRepository
    {
        // ================= Fees =================
        Task<List<FeeDto>> GetFeesAsync();
        Task RecordPaymentAsync(int feeId, decimal amount);

        // ================= Fee Head =================
        Task<List<FeeHeadModel>> GetFeeHeadsAsync();
        Task<List<FeeHeadModel>> GetFeeHeadsAsync(string feeType);
        Task SaveFeeHeadAsync(FeeHeadModel model);
        Task DeleteFeeHeadAsync(int feeHeadId);

        // ================= Fee Structure =================
        Task SaveFeeStructureAsync(FeeStructureModel model);
        Task<List<FeeStructureDetailModel>> GetFeeStructureAsync(int academicYearId, int classId,string feeType);

        // ================= Discounts =================
        Task<List<FeeDiscountModel>> GetDiscountsAsync(int academicYearId, int classId);
        Task SaveDiscountsAsync(List<FeeDiscountModel> discounts);

        // ================= Student Fee =================
        Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model);
        Task<int> AddFeePaymentAsync(StudentFeePaymentModel model);
        Task ApplyLateFeesAsync();
        Task RefundAsync(FeeRefundModel model);
        Task ApplyFeeConcessionAsync(FeeConcessionModel model);

        // ================= Receipt =================
        Task<string> SaveReceiptAsync(FeeReceiptModel model);

        // ================= Reports / Dashboard =================
        Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to);
        Task<List<MonthlyCollectionModel>> GetMonthlyAsync();
        Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId);
        Task<FeesDashboardKpiModel> GetKpiAsync();
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
        Task AssignFeeAsync(int studentId,
    int academicYearId,
    string feeType,
    decimal totalAmount,
    decimal totalDiscount,
    decimal netAmount,
    List<StudentFeeAssignmentDetailModel> details);
        Task<List<FeeReceiptModel>> GetReceiptsAsync(int studentId);


    }
}
