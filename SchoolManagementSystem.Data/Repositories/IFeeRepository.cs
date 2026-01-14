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
        Task AddFeeAsync(FeeDto fee);
        Task RecordPaymentAsync(int feeId, decimal amount);
    }
}
