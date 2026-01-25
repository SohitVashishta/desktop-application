using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data
{
    public interface ITeacherRepository
    {
        Task<List<TeacherMaster>> GetAllAsync();
        Task AddAsync(TeacherMaster teacher);
        Task UpdateAsync(TeacherMaster teacher);
        Task DeleteAsync(int id);
        Task<TeacherMaster> GetByIdAsync(int id);
    }
}
