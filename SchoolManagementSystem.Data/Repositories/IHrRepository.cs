using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IHrRepository
    {
        Task<List<StaffDto>> GetStaffAsync();
        Task AddStaffAsync(StaffDto staff);
        Task ProcessPayrollAsync(int staffId);
    }
}
