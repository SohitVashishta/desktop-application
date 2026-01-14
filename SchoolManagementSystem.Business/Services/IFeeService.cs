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
    }
}
