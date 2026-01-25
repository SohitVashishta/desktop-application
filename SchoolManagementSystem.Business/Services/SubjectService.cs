using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repository;

        public SubjectService(ISubjectRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SubjectModel>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task AddAsync(SubjectModel model)
            => _repository.AddAsync(model);

        public Task UpdateAsync(SubjectModel model)
            => _repository.UpdateAsync(model);

        public Task DeleteAsync(int subjectId)
            => _repository.DeleteAsync(subjectId);
    }

}
