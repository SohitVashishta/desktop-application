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
        Task<List<Teacher>> GetAllAsync();
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task DeleteAsync(int id);
        Task<Teacher> GetByIdAsync(int id);
    }
}
