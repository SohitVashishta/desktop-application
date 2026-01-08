using System.Collections.Generic;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repo = new TeacherRepository();

        public List<Teacher> GetTeachers() => _repo.GetAll();
        public void AddTeacher(Teacher teacher) => _repo.Add(teacher);
        public void UpdateTeacher(Teacher teacher) => _repo.Update(teacher);
        public void DeleteTeacher(int id) => _repo.Delete(id);
    }
}
