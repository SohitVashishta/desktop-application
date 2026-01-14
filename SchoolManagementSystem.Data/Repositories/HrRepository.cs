using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class HrRepository : IHrRepository
    {
        private readonly List<StaffDto> _staff = new()
        {
            new StaffDto
            {
                StaffId = 1,
                StaffName = "Mr. Sharma",
                Role = "Teacher",
                BasicSalary = 35000,
                Allowance = 5000,
                Deduction = 2000
            },
            new StaffDto
            {
                StaffId = 2,
                StaffName = "Ms. Verma",
                Role = "Accountant",
                BasicSalary = 30000,
                Allowance = 4000,
                Deduction = 1500
            }
        };

        public Task<List<StaffDto>> GetStaffAsync()
            => Task.FromResult(_staff);

        public Task AddStaffAsync(StaffDto staff)
        {
            staff.StaffId = _staff.Count + 1;
            _staff.Add(staff);
            return Task.CompletedTask;
        }

        public Task ProcessPayrollAsync(int staffId)
        {
            // Payroll record insert (future)
            return Task.CompletedTask;
        }
    }
}
