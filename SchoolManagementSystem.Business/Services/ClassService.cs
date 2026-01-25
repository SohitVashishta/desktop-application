using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _repo;

        public ClassService(IClassRepository repo)
        {
            _repo = repo;
        }

        public Task<List<ClassModel>> GetClassesAsync()
            => _repo.GetAllAsync();

        public Task AddClassAsync(ClassModel model)
            => _repo.AddAsync(model);

        public Task UpdateClassAsync(ClassModel model)
            => _repo.UpdateAsync(model);

        public Task ToggleStatusAsync(int classId)
            => _repo.ToggleStatusAsync(classId);
    }
}
