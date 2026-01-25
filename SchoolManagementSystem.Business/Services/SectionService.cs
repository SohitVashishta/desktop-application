using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _repository;

        public SectionService(ISectionRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SectionModel>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task AddAsync(SectionModel model)
            => _repository.AddAsync(model);

        public Task UpdateAsync(SectionModel model)
            => _repository.UpdateAsync(model);
    }
}
