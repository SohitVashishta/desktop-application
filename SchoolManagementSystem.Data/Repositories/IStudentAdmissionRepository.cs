using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IStudentAdmissionRepository
    {
        Task AddAsync(StudentProfileVM profile);
        Task UpdateAsync(StudentProfileVM profile);
        Task<StudentProfileVM> GetByIdAsync(int studentId);
        Task <int>SaveAdmissionAsync(StudentProfileVM profile);

    }
}
