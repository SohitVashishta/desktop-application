using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IClassRepository
    {
        Task<List<ClassModel>> GetAllAsync();
        Task AddAsync(ClassModel model);
        Task UpdateAsync(ClassModel model);
        Task ToggleStatusAsync(int classId);
    }
}
