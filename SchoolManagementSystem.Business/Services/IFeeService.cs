using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IFeeService
    {
        Task<List<FeeDto>> GetFeesAsync();
        Task RecordPaymentAsync(int feeId, decimal amount);
        Task<List<FeeHeadModel>> GetAllAsync();
        Task SaveAsync(FeeHeadModel model);
        Task DeleteAsync(int id);
        Task SaveAsync(FeeStructureModel model);
        Task<List<FeeStructureDetailModel>> GetAsync(int yearId, int classId);

            Task<List<FeeDiscountModel>> GetDiscountsAsync(int academicYearId, int classId);
            Task SaveDiscountsAsync(List<FeeDiscountModel> discounts);

        Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model);
        Task<int> PayAsync(StudentFeePaymentModel model);
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
