using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        // ✅ Repository injected via DI
        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Student>> GetStudentsAsync()
            => _repository.GetAllAsync();

        public Task<Student?> GetStudentByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddStudentAsync(Student student)
            => _repository.AddAsync(student);

        public Task UpdateStudentAsync(Student student)
            => _repository.UpdateAsync(student);

        public Task DeleteStudentAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
