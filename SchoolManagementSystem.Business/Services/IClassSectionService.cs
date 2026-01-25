using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IClassSectionService
    {
        Task<List<ClassSectionModel>> GetByClassAsync(int classId, int academicYearId);
        Task AddAsync(ClassSectionModel model);
        Task ToggleStatusAsync(int classSectionId);
    }
}
