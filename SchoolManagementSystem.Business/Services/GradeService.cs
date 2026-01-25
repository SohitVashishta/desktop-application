using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _repo;

        public GradeService(IGradeRepository repo)
        {
            _repo = repo;
        }

        public Task<List<GradeModel>> GetActiveGradesAsync()
            => _repo.GetActiveGradesAsync();
    }
}
