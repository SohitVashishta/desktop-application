using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly IAcademicYearRepository _repository;

        public AcademicYearService(IAcademicYearRepository repository)
        {
            _repository = repository;
        }

        public Task<List<AcademicYearModel>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task AddAsync(AcademicYearModel model)
            => _repository.AddAsync(model);

        public Task UpdateAsync(AcademicYearModel model)
            => _repository.UpdateAsync(model);
    }
}
