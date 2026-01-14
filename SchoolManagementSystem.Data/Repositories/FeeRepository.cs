using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        public Task<List<FeeDto>> GetFeesAsync()
        {
            return Task.FromResult(new List<FeeDto>
            {
                new FeeDto
                {
                    FeeId = 1,
                    StudentName = "Rahul Sharma",
                    Class = "10-A",
                    TotalAmount = 45000,
                    PaidAmount = 30000,
                    DueDate = "30-Sep-2026"
                },
                new FeeDto
                {
                    FeeId = 2,
                    StudentName = "Anita Verma",
                    Class = "9-B",
                    TotalAmount = 42000,
                    PaidAmount = 42000,
                    DueDate = "30-Sep-2026"
                }
            });
        }

        public Task AddFeeAsync(FeeDto fee)
        {
            // INSERT Fee Structure
            return Task.CompletedTask;
        }

        public Task RecordPaymentAsync(int feeId, decimal amount)
        {
            // UPDATE PaidAmount
            return Task.CompletedTask;
        }
    }
}
