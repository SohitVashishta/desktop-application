using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class HrService : IHrService
    {
        private readonly IHrRepository _repository = new HrRepository();

        public Task<List<StaffDto>> GetStaffAsync()
            => _repository.GetStaffAsync();

        public Task AddStaffAsync(StaffDto staff)
            => _repository.AddStaffAsync(staff);

        public Task ProcessPayrollAsync(int staffId)
            => _repository.ProcessPayrollAsync(staffId);
    }
}
