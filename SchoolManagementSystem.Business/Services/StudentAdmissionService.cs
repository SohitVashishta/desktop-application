using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class StudentAdmissionService : IStudentAdmissionService
    {
        private readonly IStudentAdmissionRepository _repo;

        public StudentAdmissionService(IStudentAdmissionRepository repo)
        {
            _repo = repo;
        }

        public Task AddAsync(StudentProfileVM profile)
            => _repo.AddAsync(profile);

        public Task UpdateAsync(StudentProfileVM profile)
            => _repo.UpdateAsync(profile);

        public Task<StudentProfileVM> GetByIdAsync(int studentId)
            => _repo.GetByIdAsync(studentId);
    }


}
