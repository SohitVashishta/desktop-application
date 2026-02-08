using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _repository;

        public FeeService()
        {
            _repository = new FeeRepository();
        }

        // ================= Fee Head =================
        public Task<List<FeeHeadModel>> GetFeeHeadsAsync()
           => _repository.GetFeeHeadsAsync();
        public Task<List<FeeHeadModel>> GetFeeHeadsAsync(string feeType)
            => _repository.GetFeeHeadsAsync(feeType);

        public Task SaveFeeHeadAsync(FeeHeadModel model)
            => _repository.SaveFeeHeadAsync(model);

        public Task DeleteFeeHeadAsync(int id)
            => _repository.DeleteFeeHeadAsync(id);

        // ================= Fee Structure =================
        public Task SaveFeeStructureAsync(FeeStructureModel model)
            => _repository.SaveFeeStructureAsync(model);

        public Task<List<FeeStructureDetailModel>> GetFeeStructureAsync(
            int academicYearId,
            int classId,
            string feeType)
            => _repository.GetFeeStructureAsync(academicYearId, classId, feeType);

        // ================= Fee Rules =================
        public Task<List<FeeRuleRowModel>> GetFeeRulesAsync(
            int academicYearId,
            int classId,
            int? sectionId,
            string feeType)
            => _repository.GetFeeRulesAsync(
                academicYearId,
                classId,
                sectionId,
                feeType);

        public Task SaveFeeRulesAsync(
            int academicYearId,
            int classId,
            int? sectionId,
            string feeType,
            List<FeeRuleRowModel> rules)
            => _repository.SaveFeeRulesAsync(
                academicYearId,
                classId,
                sectionId,
                feeType,
                rules);

        // ================= Student Fee =================
        public Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model)
            => _repository.SaveFeeAssignmentAsync(model);

        public Task<int> PayAsync(StudentFeePaymentModel model)
            => _repository.AddFeePaymentAsync(model);

        public Task RecordPaymentAsync(int feeId, decimal amount)
            => _repository.RecordPaymentAsync(feeId, amount);

        public Task RefundAsync(FeeRefundModel model)
            => _repository.RefundAsync(model);

        public Task ApplyLateFeesAsync()
            => _repository.ApplyLateFeesAsync();

        public Task ApplyFeeConcessionAsync(FeeConcessionModel model)
            => _repository.ApplyFeeConcessionAsync(model);

        // ================= Reports =================
        public Task<List<FeeDto>> GetFeesAsync()
            => _repository.GetFeesAsync();

        public Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to)
            => _repository.GetDailyCollectionAsync(from, to);

        public Task<List<MonthlyCollectionModel>> GetMonthlyAsync()
            => _repository.GetMonthlyAsync();

        public Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId)
            => _repository.GetStudentLedgerAsync(studentId);

        public Task<FeesDashboardKpiModel> GetKpiAsync()
            => _repository.GetKpiAsync();

        // ================= Receipt =================
        public Task<string> SaveReceiptAsync(FeeReceiptModel model)
      => _repository.SaveReceiptAsync(model);

        public async Task AssignFeeAsync(int studentId,
    int academicYearId,
    string feeType,
    decimal totalAmount,
    decimal totalDiscount,
    decimal netAmount,
    List<StudentFeeAssignmentDetailModel> details)
       => _repository.AssignFeeAsync(studentId, academicYearId, feeType, totalAmount, totalDiscount, netAmount, details);

        public Task<List<FeeReceiptModel>> GetReceiptsAsync(int studentId)
    => _repository.GetReceiptsAsync(studentId);
    }

}
