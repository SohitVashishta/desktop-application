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
        private readonly IFeeRepository _repository = new FeeRepository();

        public Task<List<FeeDto>> GetFeesAsync()
            => _repository.GetFeesAsync();

        public Task RecordPaymentAsync(int feeId, decimal amount)
            => _repository.RecordPaymentAsync(feeId, amount);
        

        public Task<List<FeeHeadModel>> GetAllAsync() => _repository.GetAllAsync();
        public Task SaveAsync(FeeHeadModel model) => _repository.SaveAsync(model);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
       

        public Task SaveAsync(FeeStructureModel model) => _repository.SaveAsync(model);
        public Task<List<FeeStructureDetailModel>> GetAsync(int y, int c) => _repository.GetAsync(y, c);
        public Task<List<FeeDiscountModel>> GetDiscountsAsync(int academicYearId, int classId)
            => _repository.GetDiscountAsync(academicYearId, classId);

        public Task SaveDiscountsAsync(List<FeeDiscountModel> discounts)
            => _repository.SaveDiscountAsync(discounts);
        public Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model)
            => _repository.SaveFeeAssignmentAsync(model);

        public Task<int> PayAsync(StudentFeePaymentModel model)
             => _repository.AddFeePaymentAsync(model);

        public Task ApplyLateFeesAsync() => _repository.ApplyLateFeesAsync();
        public Task SaveReceiptAsync(FeeReceiptModel r) => _repository.SaveReceiptAsync(r);
        public Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to)
=> _repository.GetDailyCollectionAsync(from, to);
        public Task RefundAsync(FeeRefundModel model)=> _repository.RefundAsync(model);
        public Task ApplyFeeConcessionAsync(FeeConcessionModel model)=> _repository.ApplyFeeConcessionAsync(model);
        public Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId)=> _repository.GetStudentLedgerAsync(studentId);
        public Task<FeesDashboardKpiModel> GetKpiAsync()=> _repository.GetKpiAsync();
        public Task<List<MonthlyCollectionModel>> GetMonthlyAsync()=> _repository.GetMonthlyAsync();
    }
}
