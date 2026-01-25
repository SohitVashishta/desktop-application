using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IClassService
    {
        Task<List<ClassModel>> GetClassesAsync();
        Task AddClassAsync(ClassModel model);
        Task UpdateClassAsync(ClassModel model);
        Task ToggleStatusAsync(int classId);
    }
}
