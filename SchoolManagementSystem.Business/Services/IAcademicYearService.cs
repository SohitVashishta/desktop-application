using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IAcademicYearService
    {
        Task<List<AcademicYearModel>> GetAllAsync();
        Task AddAsync(AcademicYearModel model);
        Task UpdateAsync(AcademicYearModel model);
    }
}
