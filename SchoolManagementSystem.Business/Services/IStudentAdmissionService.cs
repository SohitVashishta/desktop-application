using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IStudentAdmissionService
    {
        Task AddAsync(StudentProfileVM profile);
        Task UpdateAsync(StudentProfileVM profile);
        Task<StudentProfileVM> GetByIdAsync(int studentId);
    }

}
