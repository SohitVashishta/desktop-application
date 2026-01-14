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
    }
}
