using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _repository = teacherRepository;
        }

        public Task<List<Teacher>> GetTeachersAsync()
            => _repository.GetAllAsync();

        public Task<Teacher?> GetTeacherByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddTeacherAsync(Teacher teacher)
            => _repository.AddAsync(teacher);

        public Task UpdateTeacherAsync(Teacher teacher)
            => _repository.UpdateAsync(teacher);

        public Task DeleteTeacherAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
