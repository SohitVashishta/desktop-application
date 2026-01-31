using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IFeeRepository
    {
        Task<List<FeeDto>> GetFeesAsync();
        Task SaveFeeStructureAsync(FeeStructureModel model);
        Task RecordPaymentAsync(int feeId, decimal amount);
        Task<List<FeeHeadModel>> GetAllAsync();
        Task SaveAsync(FeeHeadModel model);
        Task DeleteAsync(int feeHeadId);
        Task SaveAsync(FeeStructureModel model);
        Task<List<FeeStructureDetailModel>> GetAsync(int academicYearId, int classId);
        Task<List<FeeDiscountModel>> GetDiscountAsync(int academicYearId, int classId);
        Task SaveDiscountAsync(List<FeeDiscountModel> discounts);
        Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model);
        Task<int> AddFeePaymentAsync(StudentFeePaymentModel model);
        Task ApplyLateFeesAsync();
        Task SaveReceiptAsync(FeeReceiptModel r);
        Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to);
        Task RefundAsync(FeeRefundModel model);
        Task ApplyFeeConcessionAsync(FeeConcessionModel model);
        Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId);
        Task<FeesDashboardKpiModel> GetKpiAsync();
        Task<List<MonthlyCollectionModel>> GetMonthlyAsync();
    }
}
