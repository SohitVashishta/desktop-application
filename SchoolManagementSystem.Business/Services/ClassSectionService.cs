using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class ClassSectionService : IClassSectionService
    {
        private readonly IClassSectionRepository _repo;

        public ClassSectionService(IClassSectionRepository repo)
        {
            _repo = repo;
        }

        public Task<List<ClassSectionModel>> GetByClassAsync(int classId, int academicYearId)
            => _repo.GetByClassAsync(classId, academicYearId);

        public Task AddAsync(ClassSectionModel model)
            => _repo.AddAsync(model);

        public Task ToggleStatusAsync(int classSectionId)
            => _repo.ToggleStatusAsync(classSectionId);
    }

}
