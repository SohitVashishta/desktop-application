using System.Collections.Generic;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class StudentService
    {
        private readonly StudentRepository _repo = new StudentRepository();

        public List<Student> GetStudents()
        {
            return _repo.GetAll();
        }

        public void AddStudent(Student student)
        {
            _repo.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            _repo.Update(student);
        }

        // ✅ ADD THIS
        public void DeleteStudent(int studentId)
        {
            _repo.Delete(studentId);
        }
    }
}
