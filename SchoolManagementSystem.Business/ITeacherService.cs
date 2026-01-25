using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business
{
    public interface ITeacherService
    {
        Task<List<TeacherMaster>> GetTeachersAsync();
        Task<TeacherMaster?> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(TeacherMaster teacher);
        Task UpdateTeacherAsync(TeacherMaster teacher);
        Task DeleteTeacherAsync(int id);
    }
}
